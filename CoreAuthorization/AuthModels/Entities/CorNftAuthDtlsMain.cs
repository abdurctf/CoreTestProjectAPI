using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities.CommonState;

namespace Authorization.AuthModels.Entities
{
    [Serializable]
    [Table("COR_NFT_AUTH_DTLS_MAIN")]
    [TableBlock("Cor Nft Authorization Main Details")]
    public class CorNftAuthDtlsMain : ModelBase<CorNftAuthDtlsMain>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column("LOG_DTLS_MAIN_ID")]
        [Display(Name = "Log Details Main Id")]
        public string LogDtlsMainId { get; set; }

        [Column("LOG_ID")]
        [Display(Name = "Log Id")]
        public string LogId { get; set; }

        [Required]
        [Column("PK_COLUMN_NAME")]
        [Display(Name = "Primary Column Name")]
        public string PkColumnName { get; set; }

        [Required]
        [Column("PK_DISPLAY_NAME")]
        [Display(Name = "Primary Display Name")]
        public string PkDisplayName { get; set; }

        [Required]
        [Column("PK_COLUMN_VALUE")]
        [Display(Name = "Primary Column Value")]
        public string PkColumnValue { get; set; }

        [Required]
        [Column("TABLE_NAME")]
        [Display(Name = "Table Name")]
        public string TableName { get; set; }

        [Required]
        [Column("TABLE_DISPLAY_NAME")]
        [Display(Name = "Table Display Name")]
        public string TableDisplayName { get; set; }
        
        [Required]
        [Column("ACTION_STATUS")]
        [Display(Name = "Action Status")]
        public string ActionStatus { get; set; }
    }
}
