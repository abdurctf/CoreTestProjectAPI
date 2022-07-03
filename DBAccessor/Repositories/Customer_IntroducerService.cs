using Core.TestProjectModels.Entities;
using DBAccessor.Contracts;
using DBContext;
using DBContext.ContextHelper;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Utilities.Helpers;
using System.Linq;
using Dapper;

namespace DBAccessor.Repositories
{
    public class Customer_IntroducerService : ICustomer_IntroducerService
    {
        private readonly DatabaseContextReadOnly _dbConnection;
        private readonly DatabaseContext _context;
        private readonly AppSettings _appSettings;
        public Customer_IntroducerService(DatabaseContextReadOnly dbConnection, DatabaseContext context, IOptions<AppSettings> appSettings)
        {
            _dbConnection = dbConnection;
            _context = context;
            _appSettings = appSettings.Value;
        }

        public DatabaseContext Context => _context;

        public CUSTOMER_INTRODUCER GetCustomerIntroducerById(string pIntroducer_Id)
        {
            var param = new OracleDynamicParameters();
            param.Add("pintroducer_id", OracleDbType.NVarchar2, ParameterDirection.Input, pIntroducer_Id);
            param.Add("presult_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "customer.customer_introducer_gk";

            var objCUSTOMER_PROFILE = _dbConnection.Db.Query<CUSTOMER_INTRODUCER>(query, param, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return objCUSTOMER_PROFILE;
        }
        public CUSTOMER_INTRODUCER GetCustomerIntroducerByCustomerId(string pCustomer_Id, Int16 pName_Value_List)
        {
            var param = new OracleDynamicParameters();
            param.Add("pname_value_list", OracleDbType.Int16, ParameterDirection.Input, pName_Value_List);
            param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, pCustomer_Id);
            param.Add("presult_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "customer.customer_introducer_ga";

            var objCUSTOMER_PROFILE = _dbConnection.Db.Query<CUSTOMER_INTRODUCER>(query, param, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return objCUSTOMER_PROFILE;
        }

        public string SaveCustomerIntroducer(CUSTOMER_INTRODUCER objCustIntroducer)
        {

            string vMsg = string.Empty;
            var actionStatus = string.Empty;
            var remarks = string.Empty;

            using (var transaction = _dbConnection.Db.BeginTransaction())
            {
                try
                {
                    var param = new OracleDynamicParameters();
                    if (objCustIntroducer.isAdd)
                    {

                        param.Add("pintroducer_type", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_type);
                        param.Add("pintroducer_account_br", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_br);
                        param.Add("pintroducer_account_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_no);
                        param.Add("pemployee_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_id);

                        param.Add("pemployee_pa_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_pa_no);
                        param.Add("pintroducer_details", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_details);
                        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.customer_id);
                        param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.make_by);
                        param.Add("pintroducer_id", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);


                        string query = "customer.customer_introducer_i";
                        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        objCustIntroducer.introducer_id = param.Get<string>("pintroducer_id");
                        if (string.IsNullOrEmpty(objCustIntroducer.introducer_id))
                        {
                            transaction.Rollback();
                            vMsg = "Failed to Save Data!";
                            return vMsg;
                        }
                        else
                        {
                            
                            actionStatus = "ADD";
                            remarks = "CustomerID";
                            vMsg = "Saved Successfully!";
                            transaction.Commit();
                        }

                    }
                    else if (objCustIntroducer.isOld)
                    {
                        //if (objCustIntroducer.isDelete)
                        //{
                        //    CUSTOMER_INTRODUCER obj = GetCustomerIntroducerById(objCustIntroducer.introducer_id);
                        //    if (string.IsNullOrEmpty(obj.error_msg) || obj.error_msg == "null")
                        //    {
                        //        //==delete sp
                        //        param = new OracleDynamicParameters();
                        //        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.customer_id);
                        //        param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output);
                        //        param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output);

                        //        string query = _appSettings.SpPrefix + "CUSTOMER.customer_profile_d";
                        //        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        //        objCustIntroducer.error_msg = param.Get<string>("perrormassage");

                        //        if (string.IsNullOrEmpty(objCustIntroducer.error_msg))
                        //        {
                        //            transaction.Commit();
                        //            //vMsg = "Failed to Save Data!";

                        //            actionStatus = "DEL";
                        //            remarks = "Country Id:" + objCustIntroducer.customer_id;
                        //            vMsg = "Deleted Successfully!";
                        //        }
                        //        else
                        //        {
                        //            transaction.Rollback();
                        //        }

                        //    }
                        //    else
                        //    {

                        //        return obj.error_msg;
                        //    }
                    }
                    else if (objCustIntroducer.isOld && !objCustIntroducer.isDelete)
                    {
                    //    //edit sp with try catch  
                    //    param.Add("pintroducer_type", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_type);
                    //    param.Add("pintroducer_account_br", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_br);
                    //    param.Add("pintroducer_account_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_no);
                    //    param.Add("pemployee_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_id);

                    //    param.Add("pemployee_pa_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_pa_no);
                    //    param.Add("pintroducer_details", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_details);
                    //    param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.customer_id);
                    //    param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.make_by);
                    //    param.Add("pintroducer_id", OracleDbType.NVarchar2, ParameterDirection.Output, objCustIntroducer.introducer_id);


                    //    string query = _appSettings.SpPrefix + "customer.customer_profile_u";
                    //    _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                    //    objCustIntroducer.error_msg = param.Get<string>("perrormassage");
                    //    if (!string.IsNullOrEmpty(objCustIntroducer.error_msg) || objCustIntroducer.error_msg == "null")
                    //    {
                    //        transaction.Rollback();
                    //        vMsg = "Failed to update!";
                    //        return vMsg;
                    //    }
                    //    else
                    //    {
                    //        vMsg = "Updated Successfully!";
                    //        transaction.Commit();
                    //    }
                    //}
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

        public string SaveCustomerIntroducerDetails(CUSTOMER_INTRODUCER objCustIntroducer)
        {
            string vMsg = string.Empty;
            var actionStatus = string.Empty;
            var remarks = string.Empty;

            try
            {
                var param = new OracleDynamicParameters();
                if (objCustIntroducer.isAdd)
                {
                    param.Add("pintroducer_type", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_type);
                    param.Add("pintroducer_account_br", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_br);
                    param.Add("pintroducer_account_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_no);
                    param.Add("pemployee_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_id);
                    param.Add("pemployee_pa_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_pa_no);
                    param.Add("pintroducer_details", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_details);
                    param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.customer_id);
                    param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.make_by);
                    param.Add("pintroducer_id", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);


                    string query = "customer.CUSTOMER_INTRODUCER_i";
                    _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                    objCustIntroducer.introducer_id = param.Get<string>("pintroducer_id");
                    if (string.IsNullOrEmpty(objCustIntroducer.introducer_id))
                    {

                        vMsg = "Failed to Save Data!";
                        return vMsg;
                    }
                    else
                    {

                        actionStatus = "ADD";
                        remarks = "CustomerID";
                        vMsg = "Saved Successfully!";

                    }

                }
                else if (objCustIntroducer.isOld)
                {
                    if (objCustIntroducer.isDelete)
                    {
                        CUSTOMER_INTRODUCER obj = GetCustomerIntroducerById(objCustIntroducer.introducer_id);
                        if (string.IsNullOrEmpty(obj.introducer_id))
                        {
                            //==delete sp
                            param = new OracleDynamicParameters();
                            param.Add("pintroducer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.customer_id);
                            param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output);
                            param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output);

                            string query = _appSettings.SpPrefix + "customer.customer_introducer_d";
                            _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                            objCustIntroducer.error_msg = param.Get<string>("perrormassage");

                            if (string.IsNullOrEmpty(objCustIntroducer.error_msg))
                            {
                                actionStatus = "DEL";
                                remarks = "Country Id:" + objCustIntroducer.customer_id;
                                vMsg = "Deleted Successfully!";
                            }
                            else
                            {
                                vMsg = "Failed";
                            }

                        }
                        else
                        {

                            return obj.error_msg;
                        }
                    }
                    else if (objCustIntroducer.isOld && !objCustIntroducer.isDelete)
                    {
                        //    //edit sp with try catch  
                        param.Add("pintroducer_type", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_type);
                        param.Add("pintroducer_account_br", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_br);
                        param.Add("pintroducer_account_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_account_no);
                        param.Add("pemployee_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_id);

                        param.Add("pemployee_pa_no", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.employee_pa_no);
                        param.Add("pintroducer_details", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.introducer_details);
                        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.customer_id);
                        param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objCustIntroducer.make_by);
                        param.Add("pintroducer_id", OracleDbType.NVarchar2, ParameterDirection.Output, objCustIntroducer.introducer_id);


                        string query = _appSettings.SpPrefix + "customer.customer_introducer_u";
                        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        objCustIntroducer.error_msg = param.Get<string>("perrormassage");
                        if (!string.IsNullOrEmpty(objCustIntroducer.error_msg) || objCustIntroducer.error_msg == "null")
                        {
                            vMsg = "Failed to update!";
                            return vMsg;
                        }
                        else
                        {
                            vMsg = "Updated Successfully!";

                        }
                    }
                }

                return vMsg;
            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }
            }

        }
    }

