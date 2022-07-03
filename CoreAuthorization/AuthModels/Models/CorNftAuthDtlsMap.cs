using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Utilities.CommonState;

namespace Authorization.AuthModels.Models
{
    [Serializable]
    public class CorNftAuthDtlsMap : ModelBase<CorNftAuthDtlsMap>
    {
        [Display(Name = "Log Details Id")]
        public string LogDtlsId { get; set; }

        [Display(Name = "Log Id")]
        public string LogId { get; set; }

        [Required]
        [Display(Name = "Column Name")]
        public string ColumnName { get; set; }

        [Required]
        [Display(Name = "Column Display Name")]
        public string ColDisplayName { get; set; }

        [Required]
        [Display(Name = "Old Value")]
        public string OldValue { get; set; }

        [Required]
        [Display(Name = "New Value")]
        public string NewValue { get; set; }

        [Required]
        [Display(Name = "Old Value Display Name")]
        public string OldValueDisName { get; set; }

        [Required]
        [Display(Name = "New Value Display Name")]
        public int NewValueDisName { get; set; }

        [Required]
        [Display(Name = "Log Details Main Id")]
        public string LogDtlsMainId { get; set; }
    }
}
