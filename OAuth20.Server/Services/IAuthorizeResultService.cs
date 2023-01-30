﻿/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OAuth20.Server.OauthRequest;
using OAuth20.Server.OauthResponse;

namespace OAuth20.Server.Services
{
    public interface IAuthorizeResultService
    {
        AuthorizeResponse AuthorizeRequest(IHttpContextAccessor httpContextAccessor, AuthorizationRequest authorizationRequest);
        Task<TokenResponse> GenerateTokenAsync(TokenRequest tokenRequest);
    }
}
