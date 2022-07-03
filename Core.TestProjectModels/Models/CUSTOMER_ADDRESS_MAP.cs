using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;

namespace Core.TestProjectModels.Models

{
    [Serializable]
    public class CUSTOMER_ADDRESS_MAP : ModelBase<CUSTOMER_ADDRESS_MAP>
    {
        public string addr_type_id { get; set; }
        public string addr_type { get; set; }
        public string customer_id { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string country_id { get; set; }
        public string country_nm { get; set; }
        public string division_id { get; set; }
        public string division_nm { get; set; }
        public string district_id { get; set; }
        public string district_nm { get; set; }
        public string thana_id { get; set; }
        public string thana_nm { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string auth_status_id { get; set; }
        public string make_by { get; set; }
        public string make_dt { get; set; }
        public string auth_1st_by { get; set; }
        public string auth_1st_dt { get; set; }
        public string auth_2nd_by { get; set; }
        public string auth_2nd_dt { get; set; }
        public string last_action { get; set; }
        public string addr_id { get; set; }
        public string error_msg { get; set; }
        public string error_code { get; set; }
    }
}
