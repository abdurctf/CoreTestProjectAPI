using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.AuthDBAccess.NFT.Contracts
{
    public interface ICoreAuthorizeSaveLogService
    {
        public string CreateNftAuthLogForList(IEnumerable<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy);
        public string CreateNftAuthLog(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy);
        public string CreateNftAuthLogForListUsingSP(IEnumerable<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM);
        public string CreateNftAuthLogUsingSP(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM);
        public void Save();
    }
}
