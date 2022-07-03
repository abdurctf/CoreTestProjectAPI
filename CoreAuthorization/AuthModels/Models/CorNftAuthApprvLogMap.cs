using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Utilities.CommonState;

namespace Authorization.AuthModels.Models
{
    [Serializable]
    public class CorNftAuthApprvLogMap : ModelBase<CorNftAuthApprvLogMap>
    {
        [Display(Name = "Log Approve Id")]
        public string LogApprvId { get; set; }

        [Display(Name = "Log Id")]
        public string LogId { get; set; }

        [Required]
        [Display(Name = "Authorized Or Decline By")]
        public string AuthOrDecBy { get; set; }

        [Required]
        [Display(Name = "Authorized Or Decline Date")]
        public DateTime AuthOrDecDate { get; set; }

        [Required]
        [Display(Name = "Authorization Level")]
        public int AuthLevel { get; set; }

        [Required]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}
