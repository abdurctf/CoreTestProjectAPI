using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Models
{
    public class LogInInfo
    {
        #region Properties Declared
        public string requestClientIp { get; set; }
        public string requestAppTerminalIp { get; set; }
        public int sessionTimeOut { get; set; }
        public int FunctionGroupId { get; set; }
        public string UserId { get; set; }
        public string PasswordText { get; set; }
        public LoginUserInfo vLoginUserInfo { get; set; }
        public int ApplicationId { get; set; }

        //private int _CurrentAllowedNumber;
        //private string _LicenseKey;
        //private int _LicenseType;
        public string ERRORMSG { get; set; }
        #endregion
        public LogInInfo()
        {
            vLoginUserInfo = new LoginUserInfo();
        }
    }
}
