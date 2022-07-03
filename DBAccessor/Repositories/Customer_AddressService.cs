using Core.TestProjectModels.Entities;
using DBAccessor.Contracts;
using DBContext;
using DBContext.ContextHelper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Utilities.CommonState;
using Utilities.Helpers;
using Microsoft.Extensions.Options;
using Dapper;
using System.Linq;
using Utilities.Models;

namespace DBAccessor.Repositories
{
    public class Customer_AddressService : ICustomer_AddressService
    {
        private readonly DatabaseContextReadOnly _dbConnection;
        private readonly DatabaseContext _context;
        private readonly AppSettings _appSettings;
        public Customer_AddressService(DatabaseContextReadOnly dbConnection, DatabaseContext context, IOptions<AppSettings> appSettings)
        {
            _dbConnection = dbConnection;
            _context = context;
            _appSettings = appSettings.Value;
        }

        

        public CUSTOMER_ADDRESS GetCustomerAddressById(string address_Id)
        {
            var param = new OracleDynamicParameters();
            param.Add("paddr_id", OracleDbType.NVarchar2, ParameterDirection.Input, address_Id);
            param.Add("presult_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "customer.customer_address_gk";

            var objCUSTOMER_ADDRESS = _dbConnection.Db.Query<CUSTOMER_ADDRESS>(query, param, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return objCUSTOMER_ADDRESS;
        }

        public List<CUSTOMER_ADDRESS> GetCustomerAddressByCustomerId(string pCustomerId, Int16 pName_Value_List)
        {
            var param = new OracleDynamicParameters();
            param.Add("pname_value_list", OracleDbType.Int32, ParameterDirection.Input, pName_Value_List);
            param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, pCustomerId);
            param.Add("presult_cur", OracleDbType.RefCursor, ParameterDirection.Output);


            string query = _appSettings.SpPrefix + "customer.customer_address_ga";

            var objCUSTOMER_ADDRESS = _dbConnection.Db.Query<CUSTOMER_ADDRESS>(query, param, commandType: CommandType.StoredProcedure).ToList();

            return objCUSTOMER_ADDRESS;
        }
        public List<LSListItem> GetCountryDDL()
        {
            List<LSListItem> countryList = new List<LSListItem>();
            var param = new OracleDynamicParameters();
            param.Add("presult_set_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "customer.country_ga";
            var resultList = _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure).ToList();

            foreach (var item in resultList)
            {
                LSListItem country = new LSListItem();
                country.label = item.COUNTRY_NM.ToString();
                country.value = item.COUNTRY_ID.ToString();

                countryList.Add(country);
            }

            return countryList;
        }
        public List<LSListItem> GetDivisionDDL(string countryId)
        {
            List<LSListItem> divisionList = new List<LSListItem>();
            var param = new OracleDynamicParameters();
            param.Add("pcountry_id", OracleDbType.NVarchar2, ParameterDirection.Input, countryId);
            param.Add("presult_set_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "customer.division_ga";
            var resultList = _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure).ToList();

            foreach (var item in resultList)
            {
                LSListItem division = new LSListItem();
                division.label = item.DIVISION_NM.ToString();
                division.value = item.DIVISION_ID.ToString();

                divisionList.Add(division);
            }

            return divisionList;
        }

        public List<LSListItem> GetDistrictDDL(string divisionId)
        {
            try
            {
                var param = new OracleDynamicParameters();
                param.Add("pdivision_id", OracleDbType.Decimal, ParameterDirection.Input, divisionId);
                param.Add("presult_set_cur", OracleDbType.RefCursor, ParameterDirection.Output, size: 3200);
                string query = _appSettings.SpPrefix + "customer.district_ga";
                var resultList = _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure).ToList();

                List<LSListItem> districtList = resultList
                    .Select(s => new LSListItem
                    {
                        label = s.DISTRICT_NM.ToString(),
                        value = s.DISTRICT_ID.ToString()
                    }).ToList();

                return districtList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<LSListItem> GetThanaDDL(string districtId)
        {
            List<LSListItem> thanaList = new List<LSListItem>();
            var param = new OracleDynamicParameters();
            param.Add("pdistrict_id", OracleDbType.NVarchar2, ParameterDirection.Input, districtId);
            param.Add("presult_set_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            string query = _appSettings.SpPrefix + "customer.thana_ga";
            var resultList = _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure).ToList();

            foreach (var item in resultList)
            {
                LSListItem thana = new LSListItem();
                thana.label = item.THANA_NM.ToString();
                thana.value = item.THANA_ID.ToString();

                thanaList.Add(thana);
            }

            return thanaList;
        }


        public string SaveCustomerAddress(CUSTOMER_ADDRESS objCustAddress)
        {

            string vMsg = string.Empty;
            var actionStatus = string.Empty;
            var remarks = string.Empty;

            using (var transaction = _dbConnection.Db.BeginTransaction())
            {
                try
                {
                    var param = new OracleDynamicParameters();
                    if (objCustAddress.isAdd)
                    {

                        param.Add("paddr_type_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.addr_type_id);
                        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.customer_id);
                        param.Add("paddress", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.address);
                        param.Add("pcity", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.city);
                        param.Add("pzip_code", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.zip_code);
                        param.Add("pcountry_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.country_id);
                        param.Add("pdivision_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.division_id);
                        param.Add("pdistrict_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.district_id);
                        param.Add("pthana_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.thana_id);
                        param.Add("pphone", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.phone);
                        param.Add("pmobile", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.mobile);
                        param.Add("pemail", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.email);
                        param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.make_by);
                        param.Add("paddr_id", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                        string query = "customer.customer_address_i";
                        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        objCustAddress.addr_id = param.Get<string>("pcustomer_id");

                        if (!string.IsNullOrEmpty(objCustAddress.addr_id ))
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
                    else if (objCustAddress.isOld)
                    {
                        //if (objCustAddress.isDelete)
                        //{
                        //    CUSTOMER_ADDRESS obj = GetCustomerAddressById(objCustAddress.addr_id);
                        //    if (string.IsNullOrEmpty(obj.error_msg) )
                        //    {
                        //        //==delete sp
                        //        param = new OracleDynamicParameters();
                        //        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.customer_id);
                        //        param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output);
                        //        param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output);

                        //        string query = _appSettings.SpPrefix + "CUSTOMER.customer_profile_d";
                        //        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        //        objCustAddress.error_msg = param.Get<string>("perrormassage");

                        //        if (string.IsNullOrEmpty(objCustAddress.error_msg))
                        //        {
                        //            transaction.Commit();
                        //            //vMsg = "Failed to Save Data!";

                        //            actionStatus = "DEL";
                        //            remarks = "Country Id:" + objCustAddress.customer_id;
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
                        //}
                        //else if (objCustAddress.isOld && !objCustAddress.isDelete)
                        //{
                        //    //edit sp with try catch  

                        //    param.Add("paddr_type_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.addr_type_id);
                        //    param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.customer_id);
                        //    param.Add("paddress", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.address);
                        //    param.Add("pcity", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.city);
                        //    param.Add("pzip_code", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.zip_code);
                        //    param.Add("pcountry_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.country_id);
                        //    param.Add("pdivision_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.division_id);
                        //    param.Add("pdistrict_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.district_id);
                        //    param.Add("pthana_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.thana_id);
                        //    param.Add("pphone", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.phone);
                        //    param.Add("pmobile", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.mobile);
                        //    param.Add("pemail", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.email);
                        //    param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.make_by);
                        //    param.Add("paddr_id", OracleDbType.NVarchar2, ParameterDirection.Output, objCustAddress.addr_id);

                        //    string query = _appSettings.SpPrefix + "customer.customer_profile_u";
                        //    _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        //    objCustAddress.error_msg = param.Get<string>("perrormassage");
                        //    if (!string.IsNullOrEmpty(objCustAddress.error_msg) || objCustAddress.error_msg == "null")
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

        public string SaveCustomerAddressDetails(CUSTOMER_ADDRESS objCustAddress)
        {

            string vMsg = string.Empty;
            var actionStatus = string.Empty;
            var remarks = string.Empty;


            try
            {
                var param = new OracleDynamicParameters();
                if (objCustAddress.isAdd)
                {

                    param.Add("paddr_type_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.addr_type_id);
                    param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.customer_id);
                    param.Add("paddress", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.address);
                    param.Add("pcity", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.city);
                    param.Add("pzip_code", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.zip_code);
                    param.Add("pcountry_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.country_id);
                    param.Add("pdivision_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.division_id);
                    param.Add("pdistrict_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.district_id);
                    param.Add("pthana_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.thana_id);
                    param.Add("pphone", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.phone);
                    param.Add("pmobile", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.mobile);
                    param.Add("pemail", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.email);
                    param.Add("pmake_by", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.make_by);
                    param.Add("paddr_id", OracleDbType.NVarchar2, ParameterDirection.Output, size: 32000);

                    string query = "customer.customer_address_i";
                    _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                    objCustAddress.addr_id = param.Get<string>("paddr_id");

                    if (string.IsNullOrEmpty(objCustAddress.addr_id))
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
                else if (objCustAddress.isOld)
                {
                    if (objCustAddress.isDelete)
                    {
                        CUSTOMER_ADDRESS obj = GetCustomerAddressById(objCustAddress.addr_id);
                        if (string.IsNullOrEmpty(obj.error_msg))
                        {
                            //==delete sp
                            param = new OracleDynamicParameters();
                            param.Add("paddr_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.addr_id);
                            param.Add("perrormassage", OracleDbType.NVarchar2, ParameterDirection.Output);
                            param.Add("perrorcode", OracleDbType.NVarchar2, ParameterDirection.Output);

                            string query = _appSettings.SpPrefix + "customer.customer_address_d";
                            _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                            objCustAddress.error_msg = param.Get<string>("perrormassage");

                            if (string.IsNullOrEmpty(objCustAddress.error_msg))
                            {
                                actionStatus = "DEL";
                                remarks = "Country Id:" + objCustAddress.customer_id;
                                vMsg = "Deleted Successfully!";
                            }
                            else
                            {
                                vMsg = "Failed to Delete";
                            }

                        }
                        else
                        {

                            return obj.error_msg;
                        }
                    }
                    else if (objCustAddress.isOld && !objCustAddress.isDelete)
                    {
                        //edit sp with try catch  
                        param.Add("paddr_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.addr_id);
                        param.Add("paddr_type_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.addr_type_id);
                        param.Add("pcustomer_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.customer_id);
                        param.Add("paddress", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.address);
                        param.Add("pcity", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.city);
                        param.Add("pzip_code", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.zip_code);
                        param.Add("pcountry_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.country_id);
                        param.Add("pdivision_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.division_id);
                        param.Add("pdistrict_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.district_id);
                        param.Add("pthana_id", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.thana_id);
                        param.Add("pphone", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.phone);
                        param.Add("pmobile", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.mobile);
                        param.Add("pemail", OracleDbType.NVarchar2, ParameterDirection.Input, objCustAddress.email);

                        string query = _appSettings.SpPrefix + "customer.customer_address_u";
                        _dbConnection.Db.Query(query, param, commandType: CommandType.StoredProcedure);

                        objCustAddress.error_msg = param.Get<string>("perrormassage");
                        if (!string.IsNullOrEmpty(objCustAddress.error_msg) || objCustAddress.error_msg == "null")
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

