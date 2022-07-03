using Core.TestProjectModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Models;

namespace Core.TestProjectBLL.BLLContracts
{
    public interface ICustomer_ProfileBLL
    {
        CUSTOMER_PROFILE_MAP GetCustomerProfileById(string Customer_Id);
        CUSTOMER_PROFILE_MAP SaveCustomerProfile(CUSTOMER_PROFILE_MAP objcustomerDTO, AuthParam authParam);
        string SaveCustomerProfileNFT(CUSTOMER_PROFILE_MAP objcustomerDTO, AuthParam authParam);
        CUSTOMER_PROFILE_MAP GetCustomerProfileDetailsById(string Customer_Id);
        string SaveCustomerProfileWithDetails(CUSTOMER_PROFILE_MAP objcustomerDTO); 
    }
}
