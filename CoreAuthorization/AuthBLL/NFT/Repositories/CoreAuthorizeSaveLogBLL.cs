using AutoMapper;
using Authorization.AuthBLL.NFT.Contracts;
using Authorization.AuthDBAccess.NFT.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.AuthBLL.NFT.Repositories
{
    public class CoreAuthorizeSaveLogBLL : ICoreAuthorizeSaveLogBLL
    {
        #region Properties
        private readonly IMapper _mapper;
        private readonly ICoreAuthorizeSaveLogService _coreAuthorizeSaveLogService;
        #endregion
        public CoreAuthorizeSaveLogBLL(ICoreAuthorizeSaveLogService coreAuthorizeSaveLogService, IMapper mapper)
        {
            _mapper = mapper;
            _coreAuthorizeSaveLogService = coreAuthorizeSaveLogService;
        }

        public string CreateNftAuthLogForList(List<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy)
        {
            return _coreAuthorizeSaveLogService.CreateNftAuthLogForList(NewObject, pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy);
        }
        public string CreateNftAuthLog(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy)
        {
            return _coreAuthorizeSaveLogService.CreateNftAuthLog(NewObject, pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy);
        }
        public string CreateNftAuthLogForListUsingSP(List<object> NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM = "0")
        {
            return _coreAuthorizeSaveLogService.CreateNftAuthLogForListUsingSP(NewObject, pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy, ORM);
        }
        public string CreateNftAuthLogUsingSP(object NewObject, string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy, string ORM = "0")
        {
            return _coreAuthorizeSaveLogService.CreateNftAuthLogUsingSP(NewObject, pBranchID, pFunctionID, pRemarks, pActionStatus, pMakeBy, ORM);
        }
    }
}
