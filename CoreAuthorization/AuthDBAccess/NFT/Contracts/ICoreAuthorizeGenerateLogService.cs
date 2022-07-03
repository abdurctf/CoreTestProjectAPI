using Authorization.AuthModels.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.AuthDBAccess.NFT.Contracts
{
    public interface ICoreAuthorizeGenerateLogService
    {
        public void GenerateNftAuthDtlsMainLog(object NewObject, string pLogId, ref List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain, ref List<CorNftAuthDtls> listObjCorNftAuthDtls);
        public void GenerateNftAuthDtlsLog(object NewObject, string pLogId, string pLogDtlsMainId, ref List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain, ref List<CorNftAuthDtls> listObjCorNftAuthDtls);
        public CorNftAuthReg CreateNftAuthObj(string pBranchID, string pFunctionID, string pRemarks, string pActionStatus, string pMakeBy);
        public CorNftAuthDtlsMain CreateNftAuthDtlsMainObj(object pOldObject, string pLogId);
        public CorNftAuthDtls CreateNftAuthDtlsObj(string pLogId, string pLogDtlsMainId, string pColumnName, string pColDisplayName, string pOldValueDisName, string pOldValue, string pNewValue, string pNewValueDisName);
    }
}
