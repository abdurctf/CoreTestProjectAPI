using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.TestProjectModels.Entities
{
    [Serializable]
    [Table("CUSTOMER_INTRODUCER")]
    [TableBlock("Customer Introducer")]
    public class CUSTOMER_INTRODUCER : ModelBase<CUSTOMER_INTRODUCER>
    {

        [Key]
        [Column("introducer_id")]
        [Display(Name = "introducer_id")]
        public string introducer_id { get; set; }

        [Column("introducer_type")]
        [Display(Name = "introducer_type")]
        public string introducer_type { get; set; }

        [Column("introducer_account_br")]
        [Display(Name = "introducer_account_br")]
        public string introducer_account_br { get; set; }

        [Column("introducer_account_no")]
        [Display(Name = "introducer_account_no")]
        public string introducer_account_no { get; set; }

        [Column("employee_id")]
        [Display(Name = "employee_id")]
        public string employee_id { get; set; }

        [Column("employee_pa_no")]
        [Display(Name = "employee_pa_no")]
        public string employee_pa_no { get; set; }

        [Column("introducer_details")]
        [Display(Name = "introducer_details")]
        public string introducer_details { get; set; }

        [Column("customer_id")]
        [Display(Name = "customer_id")]
        public string customer_id { get; set; }

        [Column("auth_status_id")]
        [Display(Name = "auth_status_id")]
        public string auth_status_id { get; set; }

        [Column("make_by")]
        [Display(Name = "make_by")]
        public string make_by { get; set; }

        [Column("make_dt")]
        [Display(Name = "make_dt")]
        public string make_dt { get; set; }

        [Column("auth_1st_by")]
        [Display(Name = "auth_1st_by")]
        public string auth_1st_by { get; set; }

        [Column("auth_1st_dt")]
        [Display(Name = "auth_1st_dt")]
        public string auth_1st_dt { get; set; }

        [Column("auth_2nd_by")]
        [Display(Name = "auth_2nd_by")]
        public string auth_2nd_by { get; set; }

        [Column("auth_2nd_dt")]
        [Display(Name = "auth_2nd_dt")]
        public string auth_2nd_dt { get; set; }

        [Column("last_action")]
        [Display(Name = "last_action")]
        public string last_action { get; set; }

       [NotMapped]
        public string error_msg { get; set; }

        [NotMapped]
        public string error_code { get; set; }

    }
}
