using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;

namespace Core.TestProjectModels.Models
{
    [Serializable]
    public class CUSTOMER_INTRODUCER_MAP: ModelBase<CUSTOMER_INTRODUCER_MAP>
    {
        public string introducer_id { get; set; }
        public string introducer_type       {get;set;}
        public string introducer_account_br {get;set;}
        public string introducer_account_no {get;set;}
        public string employee_id           {get;set;}
        public string employee_pa_no        {get;set;}
        public string introducer_details    {get;set;}
        public string customer_id           {get;set;}
        public string auth_status_id        {get;set;}
        public string make_by               {get;set;}
        public string make_dt               {get;set;}
        public string auth_1st_by           {get;set;}
        public string auth_1st_dt           {get;set;}
        public string auth_2nd_by           {get;set;}
        public string auth_2nd_dt           {get;set;}
        public string last_action           {get;set;}
        public string error_msg { get; set; }
        public string error_code { get; set; }
    }
}
