using Authorization.AuthDBAccess.NFT.Contracts;
using Authorization.AuthModels.Entities;
using DBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using Utilities.CommonState;
using Utilities.Helpers;

namespace Authorization.AuthDBAccess.NFT.Repositories
{
    public class CoreAuthorizeGenerateLogService : ICoreAuthorizeGenerateLogService, IDisposable
    {
        #region Private Properties
        private readonly DatabaseContextReadOnly _dbConnection;
        private readonly DatabaseContext _context;
        private readonly IConfiguration _config;
        //private readonly string vSpPrefix;
        //private readonly string vPackage;
        private readonly AppSettings _appSettings;
        #endregion
        public CoreAuthorizeGenerateLogService(DatabaseContext context, DatabaseContextReadOnly dbConnection, IConfiguration config, IOptions<AppSettings> appSettings)
        {
            _config = config;
            _context = context;
            _dbConnection = dbConnection;
            _appSettings = appSettings.Value;
            //vPackage = _config.GetValue<string>("AuthPackage").ToUpper();
            //vSpPrefix = _config.GetValue<string>("SpPrefix").ToUpper();
        }

        #region Generating Authorization Log        
        public void GenerateNftAuthDtlsMainLog(object NewObject, string pLogId, ref List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain, ref List<CorNftAuthDtls> listObjCorNftAuthDtls)
        {
            CorNftAuthDtlsMain corNftAuthDtlsMain = CreateNftAuthDtlsMainObj(NewObject, pLogId);
            listCorNftAuthDtlsMain.Add(corNftAuthDtlsMain);
            GenerateNftAuthDtlsLog(NewObject, pLogId, corNftAuthDtlsMain.LogDtlsMainId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
        }
        public void GenerateNftAuthDtlsLog(object NewObject, string pLogId, string pLogDtlsMainId, ref List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain, ref List<CorNftAuthDtls> listObjCorNftAuthDtls)
        {
            string typeNameOldObj = string.Empty;
            string typeNameNewObj = NewObject.GetType().Name;
            object OldObject = NewObject.GetType().GetProperties().Where(p => p.Name.ToUpper() == "CLONEOBJ").First().GetValue(NewObject);

            if (OldObject != null)
            {
                typeNameOldObj = OldObject.GetType().Name;
            }
            else
            {
                OldObject = NewObject;
                typeNameOldObj = typeNameNewObj;
            }

            if (typeNameOldObj != "List`1" && typeNameNewObj != "List`1" && string.Compare(typeNameOldObj, typeNameNewObj) == 0)
            {
                //edit block
                if (Convert.ToBoolean(GetColumnValue(NewObject, "isOld")) && !Convert.ToBoolean(GetColumnValue(NewObject, "isDelete")))
                {
                    PropertyInfo[] propertyInfoNew = NewObject.GetType().GetProperties();
                    foreach (PropertyInfo piNew in propertyInfoNew)
                    {

                        if (!GetAnnotationNftLog(OldObject, piNew.Name))
                        {
                            continue;
                        }

                        // if property if Object or list object
                        if (piNew.PropertyType.Name == "List`1")
                        {
                            //List<object> LIST_OBJECT = (List<object>)piNew.GetValue(NewObject, null);
                            System.Collections.IList LIST_OBJECT = (System.Collections.IList)piNew.GetValue(NewObject, null);

                            if (LIST_OBJECT != null)
                            {
                                var notMapped = piNew.GetCustomAttribute<NotMappedAttribute>();
                                if (notMapped != null)
                                {
                                    continue;
                                }

                                bool isDeletable = false;
                                try
                                {
                                    isDeletable = LIST_OBJECT.GetType().GetCustomAttribute<BUAttributes>().IsDeletable;

                                    //var buAttr = LIST_OBJECT.GetType().GetCustomAttribute<BUAttributes>();
                                    //if (buAttr != null)
                                    //{
                                    //    isDeletable = buAttr.IsDeletable;
                                    //}
                                }
                                catch
                                {

                                    isDeletable = false;
                                }


                                foreach (object objLoop in LIST_OBJECT)
                                {
                                    if (!isDeletable)
                                    {
                                        //deleted
                                        if (GetColumnValue(objLoop, "isOld").ToUpper() == "TRUE" && GetColumnValue(objLoop, "isDelete").ToUpper() == "TRUE")
                                        {
                                            throw new Exception("Invalid Child data");
                                        }
                                    }

                                    GenerateNftAuthDtlsMainLog(NewObject, pLogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
                                }
                            }
                        }
                        else if (piNew.PropertyType.Name != "Byte[]" &&
                            piNew.PropertyType.Name != "Guid" &&
                            piNew.PropertyType.Name != "Char" &&
                            piNew.PropertyType.Name != "SByte" &&
                            piNew.PropertyType.Name != "Byte" &&
                            piNew.PropertyType.Name != "Decimal" &&
                            piNew.PropertyType.Name != "UInt16" &&
                            piNew.PropertyType.Name != "UInt32" &&
                            piNew.PropertyType.Name != "UInt64" &&
                            piNew.PropertyType.Name != "bool" &&
                            piNew.PropertyType.Name != "Boolean" &&
                            piNew.PropertyType.Name != "DateTime" &&
                            piNew.PropertyType.Name != "Double" &&
                            piNew.PropertyType.Name != "Single" &&
                            piNew.PropertyType.Name != "Int16[]" &&
                            piNew.PropertyType.Name != "Int32" &&
                            piNew.PropertyType.Name != "Int64" &&
                            piNew.PropertyType.Name != "String" &&
                            !piNew.PropertyType.Name.Contains("Nullable`1")
                            )
                        {
                            object childObject = piNew.GetValue(NewObject, null);

                            if (childObject == null)
                            {
                                continue;
                            }
                            var notMapped = piNew.GetCustomAttribute<NotMappedAttribute>();
                            if (notMapped != null)
                            {
                                continue;
                            }

                            bool isDeletable = false;
                            try
                            {
                                isDeletable = childObject.GetType().GetCustomAttribute<BUAttributes>().IsDeletable;
                                //var buAttr = childObject.GetType().GetCustomAttribute<BUAttributes>();
                                //if (buAttr != null)
                                //{
                                //    isDeletable = buAttr.IsDeletable;
                                //}
                            }
                            catch
                            {
                                isDeletable = false;
                            }
                            if (!isDeletable)
                            {
                                if (GetColumnValue(childObject, "isOld").ToUpper() == "TRUE" && GetColumnValue(childObject, "isDelete").ToUpper() == "TRUE")
                                {
                                    throw new Exception("Invalid Child data");
                                }
                            }
                            GenerateNftAuthDtlsMainLog(childObject, pLogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
                        }
                        //
                        else
                        {
                            dynamic vNewValue = piNew.GetValue(NewObject, null);
                            dynamic vOldValue = piNew.GetValue(OldObject, null);

                            if (vOldValue != null && !string.IsNullOrEmpty(vOldValue.ToString()) && vNewValue != null && !string.IsNullOrEmpty(vNewValue.ToString()))
                            {
                                try
                                {
                                    var notMapped = piNew.GetCustomAttribute<NotMappedAttribute>();
                                    if (notMapped != null)
                                        continue;
                                }
                                catch (Exception) { }
                                string colName = piNew.GetCustomAttribute<ColumnAttribute>() != null ? piNew.GetCustomAttribute<ColumnAttribute>().Name : string.Empty;
                                string displayName = piNew.GetCustomAttribute<DisplayAttribute>() != null ? piNew.GetCustomAttribute<DisplayAttribute>().Name : string.Empty;
                                string oldDisplayVal = piNew.GetCustomAttribute<BUAttributes>() != null ? GetColumnValue(OldObject, piNew.GetCustomAttribute<BUAttributes>().displayValProperty) : string.Empty;
                                string newDisplayVal = piNew.GetCustomAttribute<BUAttributes>() != null ? GetColumnValue(NewObject, piNew.GetCustomAttribute<BUAttributes>().displayValProperty) : string.Empty;
                                listObjCorNftAuthDtls.Add(CreateNftAuthDtlsObj(pLogId, pLogDtlsMainId, colName, displayName, oldDisplayVal, vOldValue, vNewValue, newDisplayVal));
                            }
                        }
                    }
                }
                else if (Convert.ToBoolean(GetColumnValue(NewObject, "isAdd")) || (Convert.ToBoolean(GetColumnValue(NewObject, "isOld")) && Convert.ToBoolean(GetColumnValue(NewObject, "isDelete")))) //Add and Delete block
                {
                    PropertyInfo[] propertyInfoNew = NewObject.GetType().GetProperties();
                    foreach (PropertyInfo piNew in propertyInfoNew)
                    {
                        if (!GetAnnotationNftLog(OldObject, piNew.Name))
                        {
                            continue;
                        }
                        if (piNew.PropertyType.Name == "List`1")
                        {
                            System.Collections.IList LIST_OBJECT = (System.Collections.IList)piNew.GetValue(NewObject, null);
                            if (LIST_OBJECT != null)
                            {
                                var notMapped = piNew.GetCustomAttribute<NotMappedAttribute>();
                                if (notMapped != null)
                                {
                                    continue;
                                }

                                bool isDeletable = false;
                                try
                                {
                                    isDeletable = LIST_OBJECT.GetType().GetCustomAttribute<BUAttributes>().IsDeletable;
                                    //BUAttributes buAttr = piNew.GetCustomAttribute<BUAttributes>();
                                    //if (buAttr != null)
                                    //{
                                    //    isDeletable = buAttr.IsDeletable;
                                    //}

                                }
                                catch
                                {
                                    isDeletable = false;
                                }


                                foreach (object objLoop in LIST_OBJECT)
                                {
                                    if (!isDeletable)
                                    {
                                        if (GetColumnValue(objLoop, "isOld").ToUpper() == "TRUE" || GetColumnValue(objLoop, "isDelete").ToUpper() == "TRUE")
                                        {
                                            throw new Exception("Invalid Child data");
                                        }
                                    }
                                    GenerateNftAuthDtlsMainLog(NewObject, pLogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
                                }
                            }
                        }
                        else if (piNew.PropertyType.Name != "Byte[]" &&
                            piNew.PropertyType.Name != "Guid" &&
                            piNew.PropertyType.Name != "Char" &&
                            piNew.PropertyType.Name != "SByte" &&
                            piNew.PropertyType.Name != "Byte" &&
                            piNew.PropertyType.Name != "Decimal" &&
                            piNew.PropertyType.Name != "UInt16" &&
                            piNew.PropertyType.Name != "UInt32" &&
                            piNew.PropertyType.Name != "UInt64" &&
                            piNew.PropertyType.Name != "bool" &&
                            piNew.PropertyType.Name != "Boolean" &&
                            piNew.PropertyType.Name != "DateTime" &&
                            piNew.PropertyType.Name != "Double" &&
                            piNew.PropertyType.Name != "Single" &&
                            piNew.PropertyType.Name != "Int16[]" &&
                            piNew.PropertyType.Name != "Int32" &&
                            piNew.PropertyType.Name != "Int64" &&
                            piNew.PropertyType.Name != "String" &&
                            !piNew.PropertyType.Name.Contains("Nullable`1")
                            )
                        {
                            object childObject = piNew.GetValue(NewObject, null);

                            if (childObject == null)
                            {
                                continue;
                            }

                            NotMappedAttribute notMapped = piNew.GetCustomAttribute<NotMappedAttribute>();
                            if (notMapped != null)
                            {
                                continue;
                            }

                            bool isDeletable = false;

                            try
                            {
                                isDeletable = piNew.GetCustomAttribute<BUAttributes>().IsDeletable;
                                //BUAttributes buAttr = piNew.GetCustomAttribute<BUAttributes>();
                                //if (buAttr != null)
                                //{
                                //    isDeletable = buAttr.IsDeletable;
                                //}
                            }
                            catch
                            {
                                isDeletable = false;
                            }

                            if (!isDeletable)
                            {
                                if (GetColumnValue(childObject, "isOld").ToUpper() == "TRUE" || GetColumnValue(childObject, "isDelete").ToUpper() == "TRUE")
                                {
                                    throw new Exception("Invalid Child data");
                                }
                            }
                            GenerateNftAuthDtlsMainLog(childObject, pLogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
                        }
                        else
                        {
                            dynamic value = piNew.GetValue(NewObject, null);
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                try
                                {
                                    var notMapped = piNew.GetCustomAttribute<NotMappedAttribute>();
                                    if (notMapped != null)
                                        continue;
                                }
                                catch (Exception) { }
                                string colName = piNew.GetCustomAttribute<ColumnAttribute>() != null ? piNew.GetCustomAttribute<ColumnAttribute>().Name : string.Empty;
                                string displayName = piNew.GetCustomAttribute<DisplayAttribute>() != null ? piNew.GetCustomAttribute<DisplayAttribute>().Name : string.Empty;
                                string newDisplayVal = piNew.GetCustomAttribute<BUAttributes>() != null ? GetColumnValue(NewObject, piNew.GetCustomAttribute<BUAttributes>().displayValProperty) : string.Empty;
                                listObjCorNftAuthDtls.Add(CreateNftAuthDtlsObj(pLogId, pLogDtlsMainId, colName, displayName, string.Empty, string.Empty, value.ToString(), newDisplayVal));
                            }
                        }
                    }
                }
            }
        }
        public CorNftAuthReg CreateNftAuthObj(string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy)
        {
            CorNftAuthReg objCorNftAuthReg = new CorNftAuthReg();
            objCorNftAuthReg.BranchId = pBranchID;
            objCorNftAuthReg.LogId = Guid.NewGuid().ToString();
            objCorNftAuthReg.FunctionId = pFunctionID;
            objCorNftAuthReg.ReasonDecline = string.Empty;
            objCorNftAuthReg.ActionStatus = pActionStatus;
            objCorNftAuthReg.Remarks = pRemarks;
            objCorNftAuthReg.MakeBy = pMakeBy;
            objCorNftAuthReg.MakeDate = System.DateTime.Now;
            objCorNftAuthReg.AuthLevelPending = 1;
            objCorNftAuthReg.AuthLevelMax = 1;
            objCorNftAuthReg.AuthStatusId = "U";
            return objCorNftAuthReg;
        }
        public CorNftAuthDtlsMain CreateNftAuthDtlsMainObj(object pOldObject, string pLogId)
        {
            CorNftAuthDtlsMain objCorNftAuthDtlsMain = new CorNftAuthDtlsMain();
            objCorNftAuthDtlsMain.LogDtlsMainId = Guid.NewGuid().ToString();
            objCorNftAuthDtlsMain.LogId = pLogId;
            objCorNftAuthDtlsMain.TableName = GetAnnotationTableName(pOldObject);
            objCorNftAuthDtlsMain.TableDisplayName = GetAnnotationTableDisplayBlock(pOldObject);
            objCorNftAuthDtlsMain.PkColumnName = GetAnnotationPrimaryColumnName(pOldObject);
            objCorNftAuthDtlsMain.PkColumnValue = GetAnnotationPrimaryColumnValue(pOldObject);
            // objCorNftAuthDtlsMain.PkDisplayName = GetColumnValue(pOldObject, objCorNftAuthDtlsMain.PkColumnName);
            if (Convert.ToBoolean(GetColumnValue(pOldObject, "isOld")) && Convert.ToBoolean(GetColumnValue(pOldObject, "isDelete")))
                objCorNftAuthDtlsMain.ActionStatus = "DEL";
            else if (Convert.ToBoolean(GetColumnValue(pOldObject, "isOld")) && !Convert.ToBoolean(GetColumnValue(pOldObject, "isDelete")))
                objCorNftAuthDtlsMain.ActionStatus = "EDT";
            else
                objCorNftAuthDtlsMain.ActionStatus = "ADD";
            return objCorNftAuthDtlsMain;
        }
        public CorNftAuthDtls CreateNftAuthDtlsObj(string pLogId, string pLogDtlsMainId, string pColumnName, string pColDisplayName, string pOldValueDisName, string pOldValue, string pNewValue, string pNewValueDisName)
        {
            CorNftAuthDtls objCorNftAuthDtls = new CorNftAuthDtls();
            objCorNftAuthDtls.LogDtlsId = Guid.NewGuid().ToString();
            objCorNftAuthDtls.LogId = pLogId;
            objCorNftAuthDtls.LogDtlsMainId = pLogDtlsMainId;
            objCorNftAuthDtls.ColumnName = pColumnName;
            objCorNftAuthDtls.ColDisplayName = pColDisplayName;
            objCorNftAuthDtls.OldValue = pOldValue;
            objCorNftAuthDtls.NewValue = pNewValue;
            objCorNftAuthDtls.OldValueDisName = pOldValueDisName;
            objCorNftAuthDtls.NewValueDisName = pNewValueDisName;
            return objCorNftAuthDtls;
        }
        private string GetAnnotationColumnName(object obj, string pColumnName)
        {
            string vColumnName = pColumnName;
            try
            {
                vColumnName = ((ColumnAttribute)((obj.GetType().GetProperty(pColumnName).GetCustomAttributes(typeof(ColumnAttribute), true))).First()).Name;
            }
            catch
            {
                vColumnName = pColumnName;
            }
            return vColumnName;
        }
        private bool GetAnnotationNftLog(object obj, string pColumnName)
        {
            bool visLog = true;
            try
            {

                BUAttributes a = ((BUAttributes)((obj.GetType().GetProperty(pColumnName)
                                .GetCustomAttributes(typeof(BUAttributes), true))).First());

                visLog = a.isNftLog;

            }
            catch
            {
                visLog = true;
            }
            return visLog;
        }
        private bool GetAnnotationNotMapped(object obj, string pColumnName)
        {
            bool visNotMapped = false;
            try
            {
                var notMapped = obj.GetType().GetProperty(pColumnName).GetCustomAttributes(typeof(NotMappedAttribute), false);

                if (notMapped.Length > 0)
                {
                    visNotMapped = true;
                }

            }
            catch
            {
                visNotMapped = false;
            }
            return visNotMapped;
        }
        private bool GetAnnotationIsDetail(object obj, string pColumnName)
        {
            //HideDetailView

            bool vHideDetailView = true;
            try
            {
                var hideDetailView = obj.GetType().GetProperty(pColumnName).GetCustomAttributes(typeof(HideDetailViewAttribute), false);

                if (hideDetailView.Length > 0)
                {
                    vHideDetailView = false;
                }

            }
            catch
            {
                vHideDetailView = true;
            }
            return vHideDetailView;
        }
        private string GetAnnotationTableName(object obj)
        {
            string vTableName = string.Empty;
            try
            {
                vTableName = ((TableAttribute)((obj.GetType().GetCustomAttributes(typeof(TableAttribute), true))).First()).Name;
            }
            catch
            {
                vTableName = obj.GetType().Name;
            }
            return vTableName;
        }
        private string GetColumnValue(object pOldObject, string pColumnName)
        {
            string vValue = string.Empty;
            try
            {
                var pValue = pOldObject.GetType().GetProperties().Where(p => p.Name.ToUpper() == pColumnName.ToUpper()).First().GetValue(pOldObject);
                vValue = pValue != null ? pValue.ToString() : null;
            }
            catch (Exception ex)
            {
            }
            return vValue;
        }
        private string GetAnnotationTableDisplayBlock(object obj)
        {
            string vTableDisplayBlock = string.Empty;
            try
            {
                vTableDisplayBlock = ((TableBlockAttribute)((obj.GetType().GetCustomAttributes(typeof(TableBlockAttribute), true))).First()).DisplayBlock;
            }
            catch
            {
                vTableDisplayBlock = obj.GetType().Name;
            }
            return vTableDisplayBlock;
        }
        private string GetAnnotationColumnDisplayLabel(object obj, string pColumnName)
        {
            string vColumnDisplayName = pColumnName;
            try
            {
                vColumnDisplayName = ((DisplayAttribute)((obj.GetType().GetProperty(pColumnName).GetCustomAttributes(typeof(DisplayAttribute), true))).First()).Name;
            }
            catch (Exception ex)
            {
                vColumnDisplayName = pColumnName;
            }
            return vColumnDisplayName;
        }
        private string GetColumnDisplayForADD(object obj)
        {
            Newtonsoft.Json.Linq.JObject tokenResponse = new Newtonsoft.Json.Linq.JObject();
            string result = "{";
            PropertyInfo[] pi = obj.GetType().GetProperties();
            foreach (PropertyInfo p in pi)
            {
                string vDisp = string.Empty;
                string vValue = string.Empty;

                if (!GetAnnotationNftLog(obj, p.Name) || GetAnnotationNotMapped(obj, p.Name) || !GetAnnotationIsDetail(obj, p.Name))
                {
                    continue;
                }

                vDisp = GetAnnotationColumnDisplayLabel(obj, p.Name);
                vValue = GetColumnValue(obj, p.Name);

                tokenResponse.Add(new Newtonsoft.Json.Linq.JProperty(vDisp, vValue));

            }
            result = Newtonsoft.Json.JsonConvert.SerializeObject(tokenResponse);
            return result;
        }
        private string GetColumnDisplayForEDT(object obj)
        {
            Newtonsoft.Json.Linq.JObject tokenResponse = new Newtonsoft.Json.Linq.JObject();
            string result = "{\"\":\"\", \"\":\"\", \"\":\"\"}";

            result = Newtonsoft.Json.JsonConvert.SerializeObject(tokenResponse);
            return result;
        }
        private string GetAnnotationPrimaryColumnValue(object obj)
        {
            List<string> pKNames = new List<string>();
            string consolidatedPKValue = string.Empty;
            try
            {
                pKNames = GetAnnotationPrimaryColumns(obj);
                foreach (string item in pKNames)
                {
                    consolidatedPKValue = consolidatedPKValue + GetColumnValue(obj, item) + ",";
                }
                consolidatedPKValue = consolidatedPKValue.Remove(consolidatedPKValue.Length - 1, 1);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return consolidatedPKValue;
        }
        private string GetAnnotationPrimaryColumnName(object obj)
        {
            List<string> pKNames = new List<string>();
            string consolidatedPKName = string.Empty;
            try
            {
                pKNames = GetAnnotationPrimaryColumns(obj);
                consolidatedPKName = string.Join(",", pKNames);
            }
            catch (Exception ex)
            {
                return consolidatedPKName;
            }
            return consolidatedPKName;
        }
        private object[] GetPrimaryColumns(object obj)
        {
            try
            {
                var entry = _context.Entry(obj);
                object[] keyParts = entry.Metadata.FindPrimaryKey()
                             .Properties
                             .Select(p => entry.Property(p.Name).CurrentValue)
                             .ToArray();
                return keyParts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<string> GetAnnotationPrimaryColumns(object obj)
        {
            List<string> pKNames = new List<string>();
            try
            {
                var properties = obj.GetType().GetProperties();
                pKNames = properties.Where(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(KeyAttribute))).Select(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                return pKNames;
            }
            return pKNames;
        }

        #endregion
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
        public void Save()
        {
            if (_context != null)
                _context.SaveChanges();
        }
    }
}
