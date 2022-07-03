using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.TestProjectModels.Entities
{
    [Serializable]
    [Table("CUSTOMER_PROFILE")]
    [TableBlock("Customer Profile")]
    public class CUSTOMER_PROFILE : ModelBase<CUSTOMER_PROFILE>
    {

        private CUSTOMER_INTRODUCER _objCUSTOMER_INTRODUCER = new CUSTOMER_INTRODUCER();

        [Key]
        [Column("customer_id")]
        [Display(Name = "customer_id")]
        public string customer_id { get; set; }

        [Column("customer_category_id")]
        [Display(Name = "customer_category_id")]
        public string customer_category_id { get; set; }

        [Column("customer_name")]
        [Display(Name = "customer_name")]
        public string customer_name { get; set; }

        [Column("father_name")]
        [Display(Name = "father_name")]
        public string father_name { get; set; }

        [Column("mother_name")]
        [Display(Name = "mother_name")]
        public string mother_name { get; set; }

        [Column("gender")]
        [Display(Name = "gender")]
        public string gender { get; set; }

        [Column("date_of_birth")]
        [Display(Name = "date_of_birth")]
        public string date_of_birth { get; set; }

        [Column("marital_status")]
        [Display(Name = "marital_status")]
        public string marital_status { get; set; }

        [Column("spouse_name")]
        [Display(Name = "spouse_name")]
        public string spouse_name { get; set; }

        [Column("nid")]
        [Display(Name = "nid")]
        public string nid { get; set; }

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
        public CUSTOMER_INTRODUCER Obj_Customer_Introducer
        {
            get
            {
                return _objCUSTOMER_INTRODUCER;
            }
            set
            {
                if (_objCUSTOMER_INTRODUCER != value)
                    _objCUSTOMER_INTRODUCER = value;

            }
        }

        [NotMapped]
        public List<CUSTOMER_ADDRESS> List_Customer_Address { get; set; } = new List<CUSTOMER_ADDRESS>();
    }
}
