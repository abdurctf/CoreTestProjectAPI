using Authorization.AuthDBAccess.NFT.Contracts;
using DBContext;
using Microsoft.Extensions.Configuration;

namespace Authorization.AuthDBAccess.NFT.Repositories
{
    public class AuthManager : IAuthManager
    {
        private IConfiguration _config;
        private DatabaseContext _repoContext;
        private DatabaseContextReadOnly _dbConnection;
        private ICoreAuthorizeSaveLogService _coreAuthorizeSaveLogService;
        private ICoreAuthorizeGenerateLogService _coreAuthorizeGenerateLogService;
        public AuthManager(DatabaseContext repositoryContext, DatabaseContextReadOnly dbConnection, IConfiguration config)
        {
            _repoContext = repositoryContext;
            _dbConnection = dbConnection;
            _config = config;
        }
        public ICoreAuthorizeGenerateLogService CoreAuthorizeGenerateLogService
        {
            get
            {
                if (_coreAuthorizeGenerateLogService == null)
                {
                    _coreAuthorizeGenerateLogService = new CoreAuthorizeGenerateLogService(_repoContext, _dbConnection, _config, null);
                }
                return _coreAuthorizeGenerateLogService;
            }
        }
        public ICoreAuthorizeSaveLogService CoreAuthorizeSaveLogService
        {
            get
            {
                if (_coreAuthorizeSaveLogService == null)
                {
                    //_coreAuthorizeSaveLogService = new CoreAuthorizeSaveLogService(_repoContext, _dbConnection, _config, _coreAuthorizeGenerateLogService);
                    _coreAuthorizeSaveLogService = new CoreAuthorizeSaveLogService(_repoContext, _dbConnection, _config, _coreAuthorizeGenerateLogService, null);
                }
                return _coreAuthorizeSaveLogService;
            }
        }
    }
}
