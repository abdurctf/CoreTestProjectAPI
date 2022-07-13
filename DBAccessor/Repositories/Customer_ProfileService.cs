using Authorization.AuthDBAccess.NFT.Contracts;
using Core.TestProjectModels.Entities;
using Dapper;
using DBAccessor.Contracts;
using DBContext;
using DBContext.ContextHelper;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utilities.Common;
using Utilities.Helpers;
using Utilities.Models;

namespace DBAccessor.Repositories
{
    public class Customer_ProfileService : ICustomer_ProfileService
    {
        private readonly DatabaseContextReadOnly _dbConnection;
        private readonly DatabaseContext _context;
        private readonly AppSettings _appSettings;
        private readonly ICoreAuthorizeSaveLogService _coreAuthorizeSaveLogService;
        private readonly ICustomer_AddressService _customer_AddressService;
        private readonly ICustomer_IntroducerService _customer_IntroducerService;
        public Customer_ProfileService(DatabaseContextReadOnly dbConnection, DatabaseContext context, IOptions<AppSettings> appSettings, ICoreAuthorizeSaveLogService coreAuthorizeSaveLogService, ICustomer_AddressService customer_AddressService, ICustomer_IntroducerService customer_IntroducerService)
        {
            _dbConnection = dbConnection;
            _context = context;
            _appSettings = appSettings.Value;
            _coreAuthorizeSaveLogService = coreAuthorizeSaveLogService;
            _customer_AddressService = customer_AddressService;
            _customer_IntroducerService = customer_IntroducerService;
        }
        public CUSTOMER_PROFILE GetCustomerProfileById(string pCustomerId)
        {

            var param = new OracleDynamicParameters();
            param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, pCustomerId);
            param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output);
            param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output);
            param.Add("presult_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "CUSTOMER.customer_profile_gk";

            var objCUSTOMER_PROFILE = _dbConnection.Db.Query<CUSTOMER_PROFILE>(query, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            objCUSTOMER_PROFILE.Error_Msg = param.Get<string>("perrormassage");

            return objCUSTOMER_PROFILE;
        }


        public CUSTOMER_PROFILE GetCustomerProfileDetailsById(string pCustomerId)
        {

            var param = new OracleDynamicParameters();
            param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, pCustomerId);
            param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output);
            param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output);
            param.Add("presult_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "CUSTOMER.customer_profile_gk";

            CUSTOMER_PROFILE objCUSTOMER_PROFILE = _dbConnection.Db.Query<CUSTOMER_PROFILE>(query, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            objCUSTOMER_PROFILE.Error_Msg = param.Get<string>("perrormassage");

            if (string.IsNullOrEmpty(objCUSTOMER_PROFILE.Error_Msg) || objCUSTOMER_PROFILE.customer_id != "null")
            {
                List<CUSTOMER_ADDRESS> lstAddress = _customer_AddressService.GetCustomerAddressByCustomerId(objCUSTOMER_PROFILE.customer_id, 1);
                if (lstAddress != null && lstAddress.Count > 0)
                {
                    objCUSTOMER_PROFILE.List_Customer_Address.AddRange(lstAddress);
                }
            }
            if (string.IsNullOrEmpty(objCUSTOMER_PROFILE.Error_Msg) || objCUSTOMER_PROFILE.customer_id != "null")
            {
                var objIntroducer = _customer_IntroducerService.GetCustomerIntroducerByCustomerId(objCUSTOMER_PROFILE.customer_id, 1);
                objCUSTOMER_PROFILE.Obj_Customer_Introducer = objIntroducer;
            }

            return objCUSTOMER_PROFILE;
        }
        public CUSTOMER_PROFILE SaveCustomerProfile(CUSTOMER_PROFILE objcustomer, AuthParam authParam)
        {


            var actionStatus = string.Empty;
            var remarks = string.Empty;

            using (var transaction = _dbConnection.Db.BeginTransaction())
            {
                try
                {
                    var param = new OracleDynamicParameters();
                    if (objcustomer.isAdd)
                    {

                        param.Add("pcustomer_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_name);
                        param.Add("pfather_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.father_name);
                        param.Add("pmother_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.mother_name);
                        param.Add("pgender", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.gender);
                        param.Add("pdate_of_birth", OracleDbType.Date, ParameterDirection.Input, LSCommonFunctions.GetDate(objcustomer.date_of_birth));
                        param.Add("pmarital_status", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.marital_status);
                        param.Add("pspouse_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.spouse_name);
                        param.Add("pnid", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.nid);
                        param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.make_by);
                        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                        param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                        param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                        string query = _appSettings.SpPrefix + "customer.customer_profile_i";
                        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);
                        objcustomer.Error_Code = param.Get<string>("perrorcode");
                        objcustomer.Error_Msg = param.Get<string>("perrormassage");
                        if (!string.IsNullOrEmpty(objcustomer.Error_Msg) && objcustomer.Error_Msg != "null")
                        {
                            transaction.Rollback();

                        }
                        else
                        {
                            objcustomer.customer_id = param.Get<string>("pcustomer_id");
                            actionStatus = "ADD";
                            remarks = "CustomerID";

                            transaction.Commit();
                            objcustomer.IsRequestSuccess = true;
                        }

                    }
                    else if (objcustomer.isOld)
                    {
                        if (objcustomer.isDelete)
                        {
                            CUSTOMER_PROFILE obj = GetCustomerProfileById(objcustomer.customer_id);
                            if (string.IsNullOrEmpty(obj.Error_Msg) || obj.Error_Msg == "null")
                            {
                                //==delete sp
                                param = new OracleDynamicParameters();
                                param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_id);
                                param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                                param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                                string query = _appSettings.SpPrefix + "CUSTOMER.customer_profile_d";
                                _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);
                                objcustomer.Error_Code = param.Get<string>("perrorcode");
                                objcustomer.Error_Msg = param.Get<string>("perrormassage");

                                if (string.IsNullOrEmpty(objcustomer.Error_Msg))
                                {
                                    transaction.Commit();
                                    objcustomer.IsRequestSuccess = true;

                                    actionStatus = "DEL";
                                    remarks = "Country Id:" + objcustomer.customer_id;

                                }
                                else
                                {
                                    transaction.Rollback();
                                }

                            }

                        }
                        else if (objcustomer.isOld && !objcustomer.isDelete)
                        {
                            //edit sp with try catch  

                            param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_id);
                            param.Add("pcustomer_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_name);
                            param.Add("pfather_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.father_name);
                            param.Add("pmother_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.mother_name);
                            param.Add("pgender", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.gender);
                            param.Add("pdate_of_birth", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.date_of_birth);
                            param.Add("pmarital_status", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.marital_status);
                            param.Add("pspouse_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.spouse_name);
                            param.Add("pnid", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.nid);
                            param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                            param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                            string query = _appSettings.SpPrefix + "customer.customer_profile_u";
                            _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);
                            objcustomer.Error_Code = param.Get<string>("perrorcode");
                            objcustomer.Error_Msg = param.Get<string>("perrormassage");
                            if (!string.IsNullOrEmpty(objcustomer.Error_Msg) || objcustomer.Error_Msg == "null")
                            {
                                transaction.Rollback();

                            }
                            else
                            {
                                transaction.Commit();
                                objcustomer.IsRequestSuccess = true;
                            }
                        }
                    }

                    return objcustomer;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

        }

        public string SaveCustomerProfileNFT(CUSTOMER_PROFILE objcustomer, AuthParam authParam)
        {

            string vMsg = string.Empty;
            var actionStatus = string.Empty;
            var remarks = string.Empty;

            using (var transaction = _dbConnection.Db.BeginTransaction())
            {
                try
                {
                    var param = new OracleDynamicParameters();
                    if (objcustomer.isAdd)
                    {
                        param.Add("pcustomer_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_name);
                        param.Add("pfather_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.father_name);
                        param.Add("pmother_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.mother_name);
                        param.Add("pgender", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.gender);
                        param.Add("pdate_of_birth", OracleDbType.Date, ParameterDirection.Input, LSCommonFunctions.GetDate(objcustomer.date_of_birth));
                        param.Add("pmarital_status", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.marital_status);
                        param.Add("pspouse_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.spouse_name);
                        param.Add("pnid", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.nid);
                        param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.make_by);
                        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                        param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                        param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                        string query = _appSettings.SpPrefix + "customer.customer_profile_i";
                        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        objcustomer.Error_Msg = param.Get<string>("perrormassage");
                        if (!string.IsNullOrEmpty(objcustomer.Error_Msg) && objcustomer.Error_Msg != "null")
                        {
                            vMsg = "Failed to Save Data!";
                            return vMsg;
                        }
                        else
                        {
                            objcustomer.customer_id = param.Get<string>("pcustomer_id");
                            actionStatus = "ADD";
                            remarks = "CustomerID";
                            vMsg = "Saved";
                        }

                    }
                    else if (objcustomer.isOld)
                    {
                        if (objcustomer.isDelete)
                        {
                            CUSTOMER_PROFILE obj = GetCustomerProfileById(objcustomer.customer_id);
                            if (string.IsNullOrEmpty(obj.Error_Msg) || obj.Error_Msg == "null")
                            {
                                actionStatus = "DEL";
                                remarks = "Country Id:" + objcustomer.customer_id;
                                vMsg = "Deleted";
                            }
                            else
                            {

                                return obj.Error_Msg;
                            }
                        }
                        else
                        {
                            actionStatus = "EDT";
                            remarks = "Customer Id:" + objcustomer.customer_id;
                            vMsg = "Updated Successfully!";
                        }
                    }

                    authParam.BranchId = "0031";
                    authParam.UserId = objcustomer.make_by;


                    var result = _coreAuthorizeSaveLogService.CreateNftAuthLogUsingSP(objcustomer, authParam.BranchId, authParam.FunctionId, remarks, actionStatus, authParam.UserId, "1");


                    if (result != null)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        vMsg = "Failed to Save Data!";
                    }
                    return vMsg;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

        }
        public string SaveCustomerProfileWithDetails(CUSTOMER_PROFILE objcustomer)
        {

            string vMsg = string.Empty;
            var actionStatus = string.Empty;
            var remarks = string.Empty;

            using (var transaction = _dbConnection.Db.BeginTransaction())
            {
                try
                {
                    var param = new OracleDynamicParameters();
                    if (objcustomer.isAdd)
                    {
                        //add sp
                        param.Add("pcustomer_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_name);
                        param.Add("pfather_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.father_name);
                        param.Add("pmother_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.mother_name);
                        param.Add("pgender", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.gender);
                        param.Add("pdate_of_birth", OracleDbType.Date, ParameterDirection.Input, LSCommonFunctions.GetDate(objcustomer.date_of_birth));
                        param.Add("pmarital_status", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.marital_status);
                        param.Add("pspouse_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.spouse_name);
                        param.Add("pnid", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.nid);
                        param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.make_by);
                        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                        param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                        param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                        string query = "customer.customer_profile_i";
                        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        objcustomer.Error_Msg = param.Get<string>("perrormassage");

                        if (!string.IsNullOrEmpty(objcustomer.Error_Msg) && objcustomer.Error_Msg != "null")
                        {
                            transaction.Rollback();
                            vMsg = "Failed to Save Data!";
                            return vMsg;
                        }
                        else
                        {
                            objcustomer.customer_id = param.Get<string>("pcustomer_id");

                            if (!string.IsNullOrEmpty(objcustomer.customer_id))
                            {
                                if (objcustomer.Obj_Customer_Introducer != null)
                                {
                                    objcustomer.Obj_Customer_Introducer.customer_id = objcustomer.customer_id;
                                    objcustomer.Obj_Customer_Introducer.make_by = objcustomer.make_by;
                                    var result = _customer_IntroducerService.SaveCustomerIntroducerDetails(objcustomer.Obj_Customer_Introducer);
                                }

                                foreach (var custAddress in objcustomer.List_Customer_Address)
                                {
                                    custAddress.customer_id = objcustomer.customer_id;
                                    custAddress.make_by = objcustomer.make_by;
                                    var result = _customer_AddressService.SaveCustomerAddressDetails(custAddress);
                                }
                            }

                            actionStatus = "ADD";
                            remarks = "CustomerID";
                            vMsg = "Saved";
                            transaction.Commit();
                        }

                    }
                    else if (objcustomer.isOld)
                    {
                        if (objcustomer.isDelete)
                        {
                            CUSTOMER_PROFILE obj = GetCustomerProfileById(objcustomer.customer_id);
                            if (string.IsNullOrEmpty(obj.Error_Msg) && obj.Error_Msg != "null")
                            {
                                //==delete sp
                                param = new OracleDynamicParameters();
                                param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_id);
                                param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                                param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                                string query = _appSettings.SpPrefix + "CUSTOMER.customer_profile_d";
                                _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                                objcustomer.Error_Msg = param.Get<string>("perrormassage");

                                if (!string.IsNullOrEmpty(obj.Error_Msg) && obj.Error_Msg != "null")
                                {

                                    if (!string.IsNullOrEmpty(objcustomer.customer_id))
                                    {
                                        if (objcustomer.Obj_Customer_Introducer != null)
                                        {
                                            objcustomer.Obj_Customer_Introducer.customer_id = objcustomer.customer_id;
                                            objcustomer.Obj_Customer_Introducer.make_by = objcustomer.make_by;
                                            var result = _customer_IntroducerService.SaveCustomerIntroducerDetails(objcustomer.Obj_Customer_Introducer);
                                        }

                                        foreach (var custAddress in objcustomer.List_Customer_Address)
                                        {
                                            custAddress.customer_id = objcustomer.customer_id;
                                            custAddress.make_by = objcustomer.make_by;
                                            var result = _customer_AddressService.SaveCustomerAddressDetails(custAddress);
                                        }
                                    }

                                    transaction.Commit();
                                    //vMsg = "Failed to Save Data!";

                                    actionStatus = "DEL";
                                    remarks = "Country Id:" + objcustomer.customer_id;
                                    vMsg = "Deleted";
                                }
                                else
                                {
                                    transaction.Rollback();
                                }

                            }
                            else
                            {

                                return obj.Error_Msg;
                            }
                        }
                        else if (objcustomer.isOld && !objcustomer.isDelete)
                        {
                            //edit sp with try catch  

                            param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_id);
                            param.Add("pcustomer_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.customer_name);
                            param.Add("pfather_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.father_name);
                            param.Add("pmother_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.mother_name);
                            param.Add("pgender", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.gender);
                            param.Add("pdate_of_birth", OracleDbType.Date, ParameterDirection.Input, LSCommonFunctions.GetDate(objcustomer.date_of_birth));
                            param.Add("pmarital_status", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.marital_status);
                            param.Add("pspouse_name", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.spouse_name);
                            param.Add("pnid", OracleDbType.NVarchar2, ParameterDirection.Input, objcustomer.nid);
                            param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);
                            param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                            string query = _appSettings.SpPrefix + "customer.customer_profile_u";
                            _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                            objcustomer.Error_Msg = param.Get<string>("perrormassage");
                            if (string.IsNullOrEmpty(objcustomer.Error_Msg) || objcustomer.Error_Msg == "null")
                            {
                                if (!string.IsNullOrEmpty(objcustomer.customer_id))
                                {
                                    if (objcustomer.Obj_Customer_Introducer == null)
                                    {
                                        objcustomer.Obj_Customer_Introducer.customer_id = objcustomer.customer_id;
                                        objcustomer.Obj_Customer_Introducer.make_by = objcustomer.make_by;
                                        var result = _customer_IntroducerService.SaveCustomerIntroducerDetails(objcustomer.Obj_Customer_Introducer);
                                    }

                                    foreach (var custAddress in objcustomer.List_Customer_Address)
                                    {
                                        custAddress.customer_id = objcustomer.customer_id;
                                        custAddress.make_by = objcustomer.make_by;
                                        var result = _customer_AddressService.SaveCustomerAddressDetails(custAddress);
                                    }
                                }

                                
                                //return vMsg;
                                //}
                                // else
                                // {
                                //   foreach (var custAddress in objcustomer.List_Customer_Address)
                                //  {
                                // custAddress.customer_id = objcustomer.customer_id;
                                // custAddress.make_by = objcustomer.make_by;
                                //  var result = _customer_AddressService.SaveCustomerAddressDetails(custAddress);
                                //  }


                                //  }
                                // }
                            }


                        }
                    }
                    vMsg = "Updated";
                    transaction.Commit();
                    return vMsg;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

        }

    }
}
