using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Models
{
    public class AuthParam
    {
        public string UserId { get; set; }
        public string BranchId { get; set; }
        public string FunctionId { get; set; }
        public string Remarks { get; set; }
        public string ErrorMsg { get; set; }
        public string SuccessMsg { get; set; }
    }
}
