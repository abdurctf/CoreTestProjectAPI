using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.CommonState;

namespace Authorization.AuthModels.Entities
{
    [Serializable]
    [Table("COR_NFT_AUTH_REG")]
    [TableBlock("Cor Nft Authorization Registration")]
    public class CorNftAuthReg : ModelBase<CorNftAuthDtls>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column("LOG_ID")]
        [Display(Name ="Log Id")]
        public string LogId { get; set; }

        [Required]
        [Column("FUNCTION_ID")]
        [Display(Name = "Function Id")]
        public string FunctionId { get; set; }

        [Required]
        [Column("BRANCH_ID")]
        [Display(Name = "Branch Id")]
        public string BranchId { get; set; }

        [Required]
        [Column("ACTION_STATUS")]
        [Display(Name = "Action Status")]
        public string ActionStatus { get; set; }

        [Required]
        [Column("REMARKS")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Required]
        [Column("AUTH_STATUS_ID")]
        [Display(Name = "Authorization Status Id")]
        public string AuthStatusId { get; set; }

        [Required]
        [Column("AUTH_LEVEL_MAX")]
        [Display(Name = "Authorization Level Maximum")]
        public int AuthLevelMax { get; set; }

        [Required]
        [Column("AUTH_LEVEL_PENDING")]
        [Display(Name = "Authorization Level Pending")]
        public int AuthLevelPending { get; set; }

        [Required]
        [Column("REASON_DECLINE")]
        [Display(Name = "Reason Decline")]
        public string ReasonDecline { get; set; }

        [Required]
        [Column("MAKE_BY")]
        [Display(Name = "Make By")]
        public string MakeBy { get; set; }

        [Required]
        [Column("MAKE_DATE")]
        [Display(Name = "Make Date")]
        public DateTime MakeDate { get; set; }
    }
}
