using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Models
{
    public class LoginUserInfo
    {
        public LoginUserInfo()
        {
            UserFunctionAccess = new List<UserFunctionAccessManagement>();
            AnyBranchOperations = new List<LSBranchOperation>();
        }
        public string UserId { get; set; }
        public string LoginId { get; set; }
        public string UserNm { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string UserDescrip { get; set; }
        public string HomeBranchId { get; set; }
        public string HomeBranchName { get; set; }
        public string BranchConventionFlag { get; set; }
        public int SmsAdminRoleFlag { get; set; }
        public int SysAdminRoleFlag { get; set; }
        public int ForcePasswordChangedFlag { get; set; }
        public string PasswordExpiryAlertMsg { get; set; }
        public int BuiltInUserFlag { get; set; }
        public int InstituteStaffFlag { get; set; }
        public string EmployeeId { get; set; }
        public int AnyBrReportAccessFlag { get; set; }
        public string AnyBrOperationFlag { get; set; }
        public string SessionId { get; set; }
        public string SessionTerminalIp { get; set; }
        public string SessionCreated { get; set; }
        public string TerminalIp { get; set; }
        public string InstituteId { get; set; }
        public string InstituteName { get; set; }

        public string BranchAppTerminalIP { get; set; }
        public string BranchUserTerminalIP { get; set; }

        public DateTime? TransDate { get; set; }
        public string EmployeeNm { get; set; }
        public string ResPhone { get; set; }
        public string ResMobile { get; set; }
        public List<LSBranchOperation> AnyBranchOperations { get; set; }
        public List<UserFunctionAccessManagement> UserFunctionAccess { get; set; }
        public List<UserFavoriteMenu> UserFavoriteMenu { get; set; }

        public string Browser { get; set; }

    }
}
