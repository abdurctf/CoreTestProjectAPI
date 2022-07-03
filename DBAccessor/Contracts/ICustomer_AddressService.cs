using Core.TestProjectModels.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;
using Utilities.Models;

namespace DBAccessor.Contracts
{
    public interface ICustomer_AddressService
    {
        CUSTOMER_ADDRESS GetCustomerAddressById(string address_Id);
        List<LSListItem> GetCountryDDL();
        List<LSListItem> GetDivisionDDL(string countryId);
        List<LSListItem> GetDistrictDDL(string divisionId);
        List<LSListItem> GetThanaDDL(string districtId);
        List<CUSTOMER_ADDRESS> GetCustomerAddressByCustomerId(string pCustomerId, Int16 pName_Value_List);
        string SaveCustomerAddress(CUSTOMER_ADDRESS objcustomer);
        string SaveCustomerAddressDetails(CUSTOMER_ADDRESS objcustomer);        
    }
}
