using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Models
{
    public class LSBranchOperation
    {
        public string UserId { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string TransDate { get; set; }
        public string BranchConventionFlag { get; set; }
        public string ServiceTypeId { get; set; }
        public string IsNonFinanFlag { get; set; }
        public string IsFinanFlag { get; set; }
    }
}
