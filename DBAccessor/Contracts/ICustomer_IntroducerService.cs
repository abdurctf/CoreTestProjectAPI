using Core.TestProjectModels.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBAccessor.Contracts
{
    public interface ICustomer_IntroducerService

    {
        CUSTOMER_INTRODUCER GetCustomerIntroducerById(string introducer_Id);
        CUSTOMER_INTRODUCER GetCustomerIntroducerByCustomerId(string pCustomer_Id, Int16 pName_Value_List);
        string SaveCustomerIntroducer(CUSTOMER_INTRODUCER objcustomer);
        string SaveCustomerIntroducerDetails(CUSTOMER_INTRODUCER objcustomer);        
    }
}
