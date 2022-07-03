using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Utilities.CommonState;

namespace Authorization.AuthModels.Models
{
    [Serializable]
    public class CorNftAuthDtlsMainMap : ModelBase<CorNftAuthDtlsMainMap>
    {
        [Display(Name = "Log Details Main Id")]
        public string LogDtlsMainId { get; set; }

        [Display(Name = "Log Id")]
        public string LogId { get; set; }

        [Required]
        [Display(Name = "Primary Column Name")]
        public string PkColumnName { get; set; }

        [Required]
        [Display(Name = "Primary Display Name")]
        public string PkDisplayName { get; set; }

        [Required]
        [Display(Name = "Primary Column Value")]
        public string PkColumnValue { get; set; }

        [Required]
        [Display(Name = "Table Name")]
        public string TableName { get; set; }

        [Required]
        [Display(Name = "Table Display Name")]
        public string TableDisplayName { get; set; }

        [Required]
        [Display(Name = "Action Status")]
        public string ActionStatus { get; set; }
    }
}
