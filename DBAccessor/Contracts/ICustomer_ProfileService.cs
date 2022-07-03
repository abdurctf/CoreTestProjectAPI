using Core.TestProjectModels.Entities;
using Core.TestProjectModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Models;

namespace DBAccessor.Contracts
{
    public interface ICustomer_ProfileService
    {
        CUSTOMER_PROFILE GetCustomerProfileById(string Customer_Id);
        CUSTOMER_PROFILE GetCustomerProfileDetailsById(string Customer_Id);
        CUSTOMER_PROFILE SaveCustomerProfile(CUSTOMER_PROFILE objcustomer, AuthParam authParam);
        string SaveCustomerProfileNFT(CUSTOMER_PROFILE objcustomer, AuthParam authParam);
        string SaveCustomerProfileWithDetails(CUSTOMER_PROFILE objcustomer); 
    }
}
