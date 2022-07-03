using Authorization.AuthModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.AuthBLL.NFT.Contracts
{
    public interface ICoreAuthorizeGenerateLogBLL
    {
        public List<CorNftAuthDtlsMainMap> GenerateNftAuthDtlsMainLog(object NewObject, string pLogId, out List<CorNftAuthDtlsMap> listObjCorNftAuthDtlsMap);
        public List<CorNftAuthDtlsMap> GenerateNftAuthDtlsLog(object NewObject, string pLogId, string pLogDtlsMainId, out List<CorNftAuthDtlsMainMap> listCorNftAuthDtlsMainMap);
    }
}
