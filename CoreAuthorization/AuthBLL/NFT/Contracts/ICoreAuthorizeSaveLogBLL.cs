using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.AuthBLL.NFT.Contracts
{
    public interface ICoreAuthorizeSaveLogBLL
    {
        public string CreateNftAuthLogForList(List<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy);
        public string CreateNftAuthLog(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy);
        public string CreateNftAuthLogForListUsingSP(List<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM = "0");
        public string CreateNftAuthLogUsingSP(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM = "0");
    }
}
