using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OAuth20.Server.Models.Entities;
using OAuth20.Server.OauthRequest;
using OAuth20.Server.OauthResponse;

namespace OAuth20.Server.Services
{
    public interface IInMemoryUserManager
    {
        Task<AppUser> FindByIdAsync(string userId);

        Task<AppUser> FindByNameAsync(string userName);

        Task<AppUser> FindByEmailAsync(string email);

        Task<OpenIdConnectLoginResponse> LoginUserByOpenIdAsync(OpenIdConnectLoginRequest request);
    }
}

