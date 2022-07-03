using Core.TestProjectModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.TestProjectBLL.BLLContracts
{
    public interface ICustomer_IntroducerBLL
    {
        string SaveCustomerIntroducer(CUSTOMER_INTRODUCER_MAP objcustomer_introducer);
        CUSTOMER_INTRODUCER_MAP GetCustomerIntroducerById(string introducer_Id);
    }
}
