using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.TestProjectModels.Entities
{
    [Serializable]
    [Table("CUSTOMER_ADDRESS")]
    [TableBlock("Customer Address")]
    public class CUSTOMER_ADDRESS : ModelBase<CUSTOMER_ADDRESS>
    {
        [Key]
        [Column("addr_id")]
        [Display(Name = "addr_id")]
        public string addr_id { get; set; }

        [Required]
        [Column("addr_type_id")]
        [Display(Name = "addr type id")]
        public string addr_type_id { get; set; }
        [NotMapped]
        public string addr_type { get; set; }
        [Column("customer_id")]
        [Display(Name = "customer id")]
        public string customer_id { get; set; }

        [Column("address")]
        [Display(Name = "address")]
        public string address { get; set; }

        [Column("city")]
        [Display(Name = "city")]
        public string city { get; set; }

        [Column("zip_code")]
        [Display(Name = "zip_code")]
        public string zip_code { get; set; }

        [Column("country_id")]
        [Display(Name = "country_id")]
        public string country_id { get; set; }

        [NotMapped]
        public string country_nm { get; set; }
       

        [Column("division_id")]
        [Display(Name = "division_id")]
        public string division_id { get; set; }
        [NotMapped]
        public string division_nm { get; set; }
       

        [Column("district_id")]
        [Display(Name = "district_id")]
        public string district_id { get; set; }
        [NotMapped]
        public string district_nm { get; set; }
       
        [Column("thana_id")]
        [Display(Name = "thana_id")]
        public string thana_id { get; set; }
        [NotMapped]
        public string thana_nm { get; set; }
        [Column("phone")]
        [Display(Name = "phone")]

        public string phone { get; set; }

        [Column("mobile")]
        [Display(Name = "mobile")]
        public string mobile { get; set; }

        [Column("email")]
        [Display(Name = "email")]
        public string email { get; set; }

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
