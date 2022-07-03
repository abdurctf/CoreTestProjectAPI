using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;

namespace Core.TestProjectModels.Models
{
    [Serializable]
    public class CUSTOMER_PROFILE_MAP : ModelBase<CUSTOMER_PROFILE_MAP>
    {
        public string customer_id { get; set; }
        public string customer_category_id { get; set; }
        public string customer_name { get; set; }
        public string father_name { get; set; }
        public string mother_name { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string marital_status { get; set; }
        public string spouse_name { get; set; }
        public string nid { get; set; }
        public string auth_status_id { get; set; }
        public string make_by { get; set; }
        public string make_dt { get; set; }
        public string auth_1st_by { get; set; }
        public string auth_1st_dt { get; set; }
        public string auth_2nd_by { get; set; }
        public string auth_2nd_dt { get; set; }
        public string last_action { get; set; }       
        public CUSTOMER_INTRODUCER_MAP Obj_Customer_Introducer { get; set; }
        public List<CUSTOMER_ADDRESS_MAP> List_Customer_Address { get; set; }
    }                                          
}
