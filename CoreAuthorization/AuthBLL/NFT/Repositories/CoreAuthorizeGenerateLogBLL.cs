using AutoMapper;
using Authorization.AuthBLL.NFT.Contracts;
using Authorization.AuthDBAccess.NFT.Contracts;
using Authorization.AuthModels.Entities;
using Authorization.AuthModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.AuthBLL.NFT.Repositories
{
    public class CoreAuthorizeGenerateLogBLL : ICoreAuthorizeGenerateLogBLL
    {
        #region Class level Variables
        private readonly IMapper _mapper;
        private readonly ICoreAuthorizeGenerateLogService _coreAuthorizeGenerateLogService;
        #endregion
        public CoreAuthorizeGenerateLogBLL(ICoreAuthorizeGenerateLogService coreAuthorizeGenerateLogService, IMapper mapper)
        {
            _mapper = mapper;
            _coreAuthorizeGenerateLogService = coreAuthorizeGenerateLogService;
        }

        public List<CorNftAuthDtlsMainMap> GenerateNftAuthDtlsMainLog(object NewObject, string pLogId, out List<CorNftAuthDtlsMap> listObjCorNftAuthDtlsMap)
        {
            List<CorNftAuthDtlsMainMap> listCorNftAuthDtlsMainMap = new List<CorNftAuthDtlsMainMap>();
            listObjCorNftAuthDtlsMap = new List<CorNftAuthDtlsMap>();
            List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
            List<CorNftAuthDtls> listObjCorNftAuthDtls = new List<CorNftAuthDtls>();
            _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsMainLog(NewObject, pLogId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
            listCorNftAuthDtlsMainMap = _mapper.Map<List<CorNftAuthDtlsMainMap>>(listCorNftAuthDtlsMain);
            listObjCorNftAuthDtlsMap = _mapper.Map<List<CorNftAuthDtlsMap>>(listObjCorNftAuthDtls);
            return listCorNftAuthDtlsMainMap;
        }
        public List<CorNftAuthDtlsMap> GenerateNftAuthDtlsLog(object NewObject, string pLogId, string pLogDtlsMainId, out List<CorNftAuthDtlsMainMap> listCorNftAuthDtlsMainMap)
        {
            listCorNftAuthDtlsMainMap = new List<CorNftAuthDtlsMainMap>();
            List<CorNftAuthDtls> listObjCorNftAuthDtls = new List<CorNftAuthDtls>();
            List<CorNftAuthDtlsMain> listCorNftAuthDtlsMain = new List<CorNftAuthDtlsMain>();
            _coreAuthorizeGenerateLogService.GenerateNftAuthDtlsLog(NewObject, pLogId, pLogDtlsMainId, ref listCorNftAuthDtlsMain, ref listObjCorNftAuthDtls);
            listCorNftAuthDtlsMainMap = _mapper.Map<List<CorNftAuthDtlsMainMap>>(listCorNftAuthDtlsMain);
            List<CorNftAuthDtlsMap> listObjCorNftAuthDtlsMap = _mapper.Map<List<CorNftAuthDtlsMap>>(listObjCorNftAuthDtls);
            return listObjCorNftAuthDtlsMap;
        }
    }
}
