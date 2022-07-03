using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities.CommonState;

namespace Authorization.AuthModels.Entities
{
    [Serializable]
    [Table("COR_NFT_AUTH_APPRV_LOG")]
    [TableBlock("Cor Nft Authorization Approval Log")]
    public class CorNftAuthApprvLog : ModelBase<CorNftAuthApprvLog>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column("LOG_APPRV_ID")]
        [Display(Name = "Log Approve Id")]
        public string LogApprvId { get; set; }

        [Column("LOG_ID")]
        [Display(Name = "Log Id")]
        public string LogId { get; set; }

        [Required]
        [Column("AUTH_OR_DEC_BY")]
        [Display(Name = "Authorized Or Decline By")]
        public string AuthOrDecBy { get; set; }

        [Required]
        [Column("AUTH_OR_DEC_DATE")]
        [Display(Name = "Authorized Or Decline Date")]
        public DateTime AuthOrDecDate { get; set; }

        [Required]
        [Column("AUTH_LEVEL")]
        [Display(Name = "Authorization Level")]
        public int AuthLevel { get; set; }

        [Required]
        [Column("REMARKS")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}
