using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.AuthDBAccess.NFT.Contracts
{
    public interface IAuthManager
    {
        ICoreAuthorizeSaveLogService CoreAuthorizeSaveLogService { get; }
        ICoreAuthorizeGenerateLogService CoreAuthorizeGenerateLogService { get; }
    }
}
