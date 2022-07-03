using Authorization.AuthDBAccess.NFT.Contracts;
using Authorization.AuthModels.Entities;
using DBContext;
using DBContext.ContextHelper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data;
using Utilities.Helpers;
using Microsoft.Extensions.Options;

namespace Authorization.AuthDBAccess.NFT.Repositories
{
    public class CoreAuthorizeSaveLogService : ICoreAuthorizeSaveLogService, IDisposable
    {
        #region Private Properties
        private DatabaseContext _context;
        private readonly DatabaseContextReadOnly _dbConnection;
        private readonly IConfiguration _config;
        private readonly ICoreAuthorizeGenerateLogService _coreAuthorizeGenerateLogService;
        //private readonly string vSpPrefix;
        //private readonly string vPackage;
        private readonly  AppSettings _appSettings;



        #endregion
        public CoreAuthorizeSaveLogService(DatabaseContext context, DatabaseContextReadOnly dbConnection, IConfiguration config, ICoreAuthorizeGenerateLogService coreAuthorizeGenerateLogService, IOptions<AppSettings> appSettings)
        {
            _config = config;
            _context = context;
            _dbConnection = dbConnection;
            _coreAuthorizeGenerateLogService = coreAuthorizeGenerateLogService;
            _appSettings = appSettings.Value;
           // vPackage = _config.GetValue<string>("AuthPackage").ToUpper();
            //vSpPrefix = _config.GetValue<string>("SpPrefix").ToUpper();
        }
        public string CreateNftAuthLogForList(IEnumerable<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy)
        {
            List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
            List<CorNftAuthDtls> listObjCorNftAuthDtls = new List<CorNftAuthDtls>();
            CorNftAuthReg corCorNftAuthReg = _coreAuthorizeGenerateLogService.CreateNftAuthObj(pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy);
            if (corCorNftAuthReg != null)
            {
                foreach (var item in NewObject)
                {
                    _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsMainLog(item, corCorNftAuthReg.LogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
                }
                _context.Add(corCorNftAuthReg);
                if (listCorNftAuthDtlsMain.Count > 0)
                {
                    _context.AddRange(listCorNftAuthDtlsMain);
                }
                else
                    throw new Exception("Could not generate details main");

                if (listObjCorNftAuthDtls.Count > 0)
                {
                    _context.AddRange(listObjCorNftAuthDtls);
                }
                else
                    throw new Exception("Could not generate details column value");

                return corCorNftAuthReg.LogId;
            }
            else
                return null;
        }
        public string CreateNftAuthLog(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy)
        {
            List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
            List<CorNftAuthDtls> listObjCorNftAuthDtls = new List<CorNftAuthDtls>();
            CorNftAuthReg corCorNftAuthReg = _coreAuthorizeGenerateLogService.CreateNftAuthObj(pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy);
            if (corCorNftAuthReg != null)
            {
                _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsMainLog(NewObject, corCorNftAuthReg.LogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
                _context.Add(corCorNftAuthReg);
                if (listCorNftAuthDtlsMain.Count > 0)
                {
                    _context.AddRange(listCorNftAuthDtlsMain);
                }
                else
                    throw new Exception("Could not generate details main");

                if (listObjCorNftAuthDtls.Count > 0)
                {
                    _context.AddRange(listObjCorNftAuthDtls);
                }
                else
                    throw new Exception("Could not generate details column value");

                return corCorNftAuthReg.LogId;
            }
            else
                return null;
        }
        // Parameter ORM -- 0 = "EF", 1 == "Dapper", Default 0
        public string CreateNftAuthLogForListUsingSP(IEnumerable<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM = "0")
        {
            List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
            List<CorNftAuthDtls> listObjCorNftAuthDtls = new List<CorNftAuthDtls>();
            CorNftAuthReg corCorNftAuthReg = _coreAuthorizeGenerateLogService.CreateNftAuthObj(pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy);
            if (ORM == "1")
            {
                if (corCorNftAuthReg != null)
                {
                    string result = CreateNftAuthRegSPByDapper(corCorNftAuthReg);
                    if (result != "1")
                        throw new Exception(result);
                    foreach (var item in NewObject)
                    {
                        listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
                        listObjCorNftAuthDtls = new List<CorNftAuthDtls>();
                        _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsMainLog(item, corCorNftAuthReg.LogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);

                        if (listCorNftAuthDtlsMain.Count > 0)
                        {
                            for (int i = 0; i < listCorNftAuthDtlsMain.Count; i++)
                            {
                                result = CreateNftAuthDtlsMainSPByDapper(listCorNftAuthDtlsMain[i]);
                                if (result != "1")
                                    throw new Exception(result);
                            }
                        }
                        else
                            throw new Exception("Could not generate details main");

                        if (listObjCorNftAuthDtls.Count > 0)
                        {
                            for (int i = 0; i < listObjCorNftAuthDtls.Count; i++)
                            {
                                result = CreateNftAuthDtlsSPByDapper(listObjCorNftAuthDtls[i]);
                                if (result != "1")
                                    throw new Exception(result);
                            }
                        }
                        else
                            throw new Exception("Could not generate details column value");
                    }
                    return corCorNftAuthReg.LogId;
                }
                else
                    return null;
            }
            else
            {
                if (corCorNftAuthReg != null)
                {
                    string result = CreateNftAuthRegSPByEF(corCorNftAuthReg);
                    if (result != "1")
                        throw new Exception(result);
                    foreach (var item in NewObject)
                    {
                        listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
                        listObjCorNftAuthDtls = new List<CorNftAuthDtls>();

                        _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsMainLog(item, corCorNftAuthReg.LogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);

                        if (listCorNftAuthDtlsMain.Count > 0)
                        {
                            for (int i = 0; i < listCorNftAuthDtlsMain.Count; i++)
                            {
                                result = CreateNftAuthDtlsMainSPByEF(listCorNftAuthDtlsMain[i]);
                                if (result != "1")
                                    throw new Exception(result);
                            }
                        }
                        else
                            throw new Exception("Could not generate details main");

                        if (listObjCorNftAuthDtls.Count > 0)
                        {
                            for (int i = 0; i < listObjCorNftAuthDtls.Count; i++)
                            {
                                result = CreateNftAuthDtlsSPByEF(listObjCorNftAuthDtls[i]);
                                if (result != "1")
                                    throw new Exception(result);
                            }
                        }
                        else
                            throw new Exception("Could not generate details column value");
                    }
                    return corCorNftAuthReg.LogId;
                }
                else
                    return null;
            }
        }
        // Parameter ORM -- 0 = "EF", 1 == "Dapper", Default 0
        public string CreateNftAuthLogUsingSP(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM = "0")
        {
            List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
            List<CorNftAuthDtls> listObjCorNftAuthDtls = new List<CorNftAuthDtls>();
            CorNftAuthReg corCorNftAuthReg = _coreAuthorizeGenerateLogService.CreateNftAuthObj(pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy);
            if (ORM == "1")
            {
                if (corCorNftAuthReg != null)
                {
                    string result = CreateNftAuthRegSPByDapper(corCorNftAuthReg);
                    if (result != "1")
                        throw new Exception(result);

                    _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsMainLog(NewObject, corCorNftAuthReg.LogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);

                    if (listCorNftAuthDtlsMain.Count > 0)
                    {
                        for (int i = 0; i < listCorNftAuthDtlsMain.Count; i++)
                        {
                            result = CreateNftAuthDtlsMainSPByDapper(listCorNftAuthDtlsMain[i]);
                            if (result != "1")
                                throw new Exception(result);
                        }
                    }
                    else
                        throw new Exception("Could not generate details main");

                    if (listObjCorNftAuthDtls.Count > 0)
                    {
                        for (int i = 0; i < listObjCorNftAuthDtls.Count; i++)
                        {
                            result = CreateNftAuthDtlsSPByDapper(listObjCorNftAuthDtls[i]);
                            if (result != "1")
                                throw new Exception(result);
                        }
                    }
                    else
                        throw new Exception("Could not generate details column value");

                    return corCorNftAuthReg.LogId;
                }
                else
                    return null;
            }
            else
            {
                if (corCorNftAuthReg != null)
                {
                    string result = CreateNftAuthRegSPByEF(corCorNftAuthReg);
                    if (result != "1")
                        throw new Exception(result);

                    _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsMainLog(NewObject, corCorNftAuthReg.LogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);

                    if (listCorNftAuthDtlsMain.Count > 0)
                    {
                        for (int i = 0; i < listCorNftAuthDtlsMain.Count; i++)
                        {
                            result = CreateNftAuthDtlsMainSPByEF(listCorNftAuthDtlsMain[i]);
                            if (result != "1")
                                throw new Exception(result);
                        }
                    }
                    else
                        throw new Exception("Could not generate details main");

                    if (listObjCorNftAuthDtls.Count > 0)
                    {
                        for (int i = 0; i < listObjCorNftAuthDtls.Count; i++)
                        {
                            result = CreateNftAuthDtlsSPByEF(listObjCorNftAuthDtls[i]);
                            if (result != "1")
                                throw new Exception(result);
                        }
                    }
                    else
                        throw new Exception("Could not generate details column value");

                    return corCorNftAuthReg.LogId;
                }
                else
                    return null;
            }
        }
        private string CreateNftAuthRegSPByDapper(CorNftAuthReg corCorNftAuthReg)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("PLOG_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corCorNftAuthReg.LogId);
                dyParam.Add("PFUNCTION_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corCorNftAuthReg.FunctionId);
                dyParam.Add("PACTION_STATUS", OracleDbType.NVarchar2, ParameterDirection.Input, corCorNftAuthReg.ActionStatus);
                dyParam.Add("PAUTH_STATUS_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corCorNftAuthReg.AuthStatusId);
                dyParam.Add("PAUTH_LEVEL_MAX", OracleDbType.Int16, ParameterDirection.Input, corCorNftAuthReg.AuthLevelMax);
                dyParam.Add("PAUTH_LEVEL_PENDING", OracleDbType.Int16, ParameterDirection.Input, corCorNftAuthReg.AuthLevelPending);
               // dyParam.Add("PREASON_DECLINE", OracleDbType.NVarchar2, ParameterDirection.Input, corCorNftAuthReg.ReasonDecline);
                dyParam.Add("PBRANCH_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corCorNftAuthReg.BranchId);
                dyParam.Add("PMAKE_BY", OracleDbType.NVarchar2, ParameterDirection.Input, corCorNftAuthReg.MakeBy);
                dyParam.Add("PMAKE_DATE", OracleDbType.Date, ParameterDirection.Input, corCorNftAuthReg.MakeDate);
                dyParam.Add("PERRORCODE", OracleDbType.Int32, ParameterDirection.Output);
                dyParam.Add("PERRORMSG", OracleDbType.NVarchar2, ParameterDirection.Output);

                if (_dbConnection.Db.State == ConnectionState.Closed)
                {
                    _dbConnection.Db.Open();
                }
                if (_dbConnection.Db.State == ConnectionState.Open)
                {
                    //var query = vSpPrefix + vPackage + "NFT_AUTH_REG_I";
                    var query = _appSettings.SpPrefix + _appSettings.AuthPackage + "NFT_AUTH_REG_I";
                    
                    _dbConnection.Db.Execute(query, param: dyParam, commandType: CommandType.StoredProcedure);
                }
                var error = dyParam.Get("PERRORMSG");
                if (error == null)
                    return "1";
                else
                    return error.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string CreateNftAuthRegSPByEF(CorNftAuthReg corCorNftAuthReg)
        {
            try
            {
                _context.LoadStoredProc(_appSettings.SpPrefix + _appSettings.AuthPackage + "NFT_AUTH_REG_I")
                    .AddParam("PLOG_ID", corCorNftAuthReg.LogId)
                    .AddParam("PFUNCTION_ID", corCorNftAuthReg.FunctionId)
                    .AddParam("PACTION_STATUS", corCorNftAuthReg.ActionStatus)
                    .AddParam("PAUTH_STATUS_ID", corCorNftAuthReg.AuthStatusId)
                    .AddParam("PAUTH_LEVEL_MAX", corCorNftAuthReg.AuthLevelMax)
                    .AddParam("PAUTH_LEVEL_PENDING", corCorNftAuthReg.AuthLevelPending)
                   // .AddParam("PREASON_DECLINE", corCorNftAuthReg.ReasonDecline)
                    .AddParam("PBRANCH_ID", corCorNftAuthReg.BranchId)
                    .AddParam("PMAKE_BY", corCorNftAuthReg.MakeBy)
                    .AddParam("PMAKE_DATE", GetDateFormat(corCorNftAuthReg.MakeDate))
                    .AddParam("PERRORCODE", out IOutParam<Int32> errCode)
                    .AddParam("PERRORMSG", out IOutParam<string> errMsg)
                    .ExecNonQuery();
                string error = errMsg.Value.ToString();
                if (string.IsNullOrEmpty(error))
                    return "1";
                else
                    return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string CreateNftAuthDtlsMainSPByDapper(CorNftAuthDtlsMain corNftAuthDtlsMain)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("PLOG_DTLS_MAIN_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.LogDtlsMainId);
                dyParam.Add("PLOG_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.LogId);
                dyParam.Add("PPK_COLUMN_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.PkColumnName);
                dyParam.Add("PPK_DISPLAY_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.PkDisplayName);
                dyParam.Add("PPK_COLUMN_VALUE", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.PkColumnValue);
                dyParam.Add("PTABLE_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.TableName);
                dyParam.Add("PTABLE_DISPLAY_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.TableDisplayName);
                dyParam.Add("PACTION_STATUS", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtlsMain.ActionStatus);
                dyParam.Add("PERRORCODE", OracleDbType.Int32, ParameterDirection.Output);
                dyParam.Add("PERRORMSG", OracleDbType.NVarchar2, ParameterDirection.Output);

                if (_dbConnection.Db.State == ConnectionState.Closed)
                {
                    _dbConnection.Db.Open();
                }
                if (_dbConnection.Db.State == ConnectionState.Open)
                {
                    //var query = vSpPrefix + vPackage + "NFT_AUTH_DTLS_MAIN_I";
                    var query = _appSettings.SpPrefix + _appSettings.AuthPackage + "NFT_AUTH_DTLS_MAIN_I";
                    _dbConnection.Db.Execute(query, param: dyParam, commandType: CommandType.StoredProcedure);
                }
                var error = dyParam.Get("PERRORMSG");
                if (error == null)
                    return "1";
                else
                    return error.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string CreateNftAuthDtlsMainSPByEF(CorNftAuthDtlsMain corNftAuthDtlsMain)
        {
            try
            {
                _context.LoadStoredProc(  _appSettings.SpPrefix + _appSettings.AuthPackage + "NFT_AUTH_DTLS_MAIN_I")
                    .AddParam("PLOG_DTLS_MAIN_ID", corNftAuthDtlsMain.LogDtlsMainId)
                    .AddParam("PLOG_ID", corNftAuthDtlsMain.LogId)
                    .AddParam("PPK_COLUMN_NAME", corNftAuthDtlsMain.PkColumnName)
                    .AddParam("PPK_DISPLAY_NAME", corNftAuthDtlsMain.PkDisplayName)
                    .AddParam("PPK_COLUMN_VALUE", corNftAuthDtlsMain.PkColumnValue)
                    .AddParam("PTABLE_NAME", corNftAuthDtlsMain.TableName)
                    .AddParam("PTABLE_DISPLAY_NAME", corNftAuthDtlsMain.TableDisplayName)
                    .AddParam("PACTION_STATUS", corNftAuthDtlsMain.ActionStatus)
                    .AddParam("PERRORCODE", out IOutParam<Int32> errCode)
                    .AddParam("PERRORMSG", out IOutParam<string> errMsg)
                    .ExecNonQuery();
                string error = errMsg.Value.ToString();
                if (string.IsNullOrEmpty(error))
                    return "1";
                else
                    return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string CreateNftAuthDtlsSPByDapper(CorNftAuthDtls corNftAuthDtls)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("PLOG_DTLS_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.LogDtlsId);
                dyParam.Add("PLOG_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.LogId);
                dyParam.Add("PCOLUMN_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.ColumnName);
                dyParam.Add("PCOL_DISPLAY_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.ColDisplayName);
                dyParam.Add("POLD_VALUE", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.OldValue);
                dyParam.Add("PNEW_VALUE", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.NewValue);
                dyParam.Add("POLD_VALUE_DIS_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.OldValueDisName);
                dyParam.Add("PNEW_VALUE_DIS_NAME", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.NewValueDisName);
                dyParam.Add("PLOG_DTLS_MAIN_ID", OracleDbType.NVarchar2, ParameterDirection.Input, corNftAuthDtls.LogDtlsMainId);
                dyParam.Add("PERRORCODE", OracleDbType.Int32, ParameterDirection.Output);
                dyParam.Add("PERRORMSG", OracleDbType.NVarchar2, ParameterDirection.Output);

                if (_dbConnection.Db.State == ConnectionState.Closed)
                {
                    _dbConnection.Db.Open();
                }
                if (_dbConnection.Db.State == ConnectionState.Open)
                {
                    var query =  _appSettings.SpPrefix + _appSettings.AuthPackage + "NFT_AUTH_DTLS_I";
                    _dbConnection.Db.Execute(query, param: dyParam, commandType: CommandType.StoredProcedure);
                }
                var error = dyParam.Get("PERRORMSG");
                if (error == null)
                    return "1";
                else
                    return error.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string CreateNftAuthDtlsSPByEF(CorNftAuthDtls corNftAuthDtls)
        {
            try
            {
                _context.LoadStoredProc( _appSettings.SpPrefix + _appSettings.AuthPackage + "NFT_AUTH_DTLS_I")
                    .AddParam("PLOG_DTLS_ID", corNftAuthDtls.LogDtlsId)
                    .AddParam("PLOG_ID", corNftAuthDtls.LogId)
                    .AddParam("PCOLUMN_NAME", corNftAuthDtls.ColumnName)
                    .AddParam("PCOL_DISPLAY_NAME", corNftAuthDtls.ColDisplayName)
                    .AddParam("POLD_VALUE", corNftAuthDtls.OldValue)
                    .AddParam("PNEW_VALUE", corNftAuthDtls.NewValue)
                    .AddParam("POLD_VALUE_DIS_NAME", corNftAuthDtls.OldValueDisName)
                    .AddParam("PNEW_VALUE_DIS_NAME", corNftAuthDtls.NewValueDisName)
                    .AddParam("PLOG_DTLS_MAIN_ID", corNftAuthDtls.LogDtlsMainId)
                    .AddParam("PERRORCODE", out IOutParam<Int32> errCode)
                    .AddParam("PERRORMSG", out IOutParam<string> errMsg)
                    .ExecNonQuery();
                string error = errMsg.Value.ToString();
                if (string.IsNullOrEmpty(error))
                    return "1";
                else
                    return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static string GetDateFormat(DateTime pDateValue)
        {
            string vDateFormate = null;
            try
            {
                vDateFormate = pDateValue.ToString("dd-MMM-yyyy");
            }
            catch
            {
                if (vDateFormate == null)
                {
                    return "InValid";
                }
            }
            return vDateFormate;
        }
        public void Save()
        {
            if (_context != null)
                _context.SaveChanges();
        }
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
