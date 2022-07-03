using Core.TestProjectModels.Entities;
using Core.TestProjectModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;
using Utilities.Models;

namespace Core.TestProjectBLL.BLLContracts
{ 
    public interface ICustomer_AddressBLL
    {
        CUSTOMER_ADDRESS_MAP GetCustomerAddressById(string address_Id);
        List<LSListItem> GetCountryDDL();
        List<LSListItem> GetDivisionDDL(string countryId);
        List<LSListItem> GetDistrictDDL(string divisionId);
        List<LSListItem> GetThanaDDL(string districtId);
        string SaveCustomerAddress(CUSTOMER_ADDRESS_MAP objcustomerAddressDTO);
    }
}
