using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.CommonState;

namespace Authorization.AuthModels.Entities
{
    [Serializable]
    [Table("COR_NFT_AUTH_DTLS")]
    [TableBlock("Cor Nft Authorization Details")]
    public class CorNftAuthDtls : ModelBase<CorNftAuthDtls>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column("LOG_DTLS_ID")]
        [Display(Name = "Log Details Id")]
        public string LogDtlsId { get; set; }

        [Column("LOG_ID")]
        [Display(Name = "Log Id")]
        public string LogId { get; set; }

        [Required]
        [Column("COLUMN_NAME")]
        [Display(Name = "Column Name")]
        public string ColumnName { get; set; }

        [Required]
        [Column("COL_DISPLAY_NAME")]
        [Display(Name = "Column Display Name")]
        public string ColDisplayName { get; set; }

        [Required]
        [Column("OLD_VALUE")]
        [Display(Name = "Old Value")]
        public string OldValue { get; set; }

        [Required]
        [Column("NEW_VALUE")]
        [Display(Name = "New Value")]
        public string NewValue { get; set; }

        [Required]
        [Column("OLD_VALUE_DIS_NAME")]
        [Display(Name = "Old Value Display Name")]
        public string OldValueDisName { get; set; }

        [Required]
        [Column("NEW_VALUE_DIS_NAME")]
        [Display(Name = "New Value Display Name")]
        public string NewValueDisName { get; set; }

        [Required]
        [Column("LOG_DTLS_MAIN_ID")]
        [Display(Name = "Log Details Main Id")]
        public string LogDtlsMainId { get; set; } 
    }
}
