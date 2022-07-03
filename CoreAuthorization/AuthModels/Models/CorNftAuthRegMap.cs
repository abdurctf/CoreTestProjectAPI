using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Utilities.CommonState;

namespace Authorization.AuthModels.Models
{
    [Serializable]
    public class CorNftAuthRegMap : ModelBase<CorNftAuthRegMap>
    {
        [Display(Name = "Log Id")]
        public string LogId { get; set; }

        [Required]
        [Display(Name = "Function Id")]
        public string FunctionId { get; set; }

        [Required]
        [Display(Name = "Branch Id")]
        public string BranchId { get; set; }

        [Required]
        [Display(Name = "Action Status")]
        public string ActionStatus { get; set; }

        [Required]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Authorization Status Id")]
        public string AuthStatusId { get; set; }

        [Required]
        [Display(Name = "Authorization Level Maximum")]
        public int AuthLevelMax { get; set; }

        [Required]
        [Display(Name = "Authorization Level Pending")]
        public int AuthLevelPending { get; set; }

        [Required]
        [Display(Name = "Reason Decline")]
        public string ReasonDecline { get; set; }

        [Required]
        [Display(Name = "Make By")]
        public string MakeBy { get; set; }

        [Required]
        [Display(Name = "Make Date")]
        public DateTime MakeDate { get; set; }
    }
}
