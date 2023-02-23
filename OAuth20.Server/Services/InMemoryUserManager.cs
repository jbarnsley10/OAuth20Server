using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using OAuth20.Server.Models.Entities;
using OAuth20.Server.OauthRequest;
using OAuth20.Server.OauthResponse;

namespace OAuth20.Server.Services
{
    public class InMemoryUserManager : IInMemoryUserManager
    {
        private List<AppUser> userList = new List<AppUser>();

        public InMemoryUserManager()
        {
            this.userList = this.ReadUsersFromFile("users.json");
        }

        public Task<AppUser> FindByIdAsync(string userId)
        {
            return Task.FromResult(userList.FirstOrDefault(x => x.Id.Equals(userId)));
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(userList.FirstOrDefault(x => x.UserName.Equals(userName)));
        }

        public Task<AppUser> FindByEmailAsync(string email)
        {
            return Task.FromResult(userList.FirstOrDefault(x => x.Email.Equals(email)));
        }

        public async Task<OpenIdConnectLoginResponse> LoginUserByOpenIdAsync(OpenIdConnectLoginRequest request)
        {
            bool validationResult = validateOpenIdLoginRequest(request);
            if (!validationResult)
            {
                return new OpenIdConnectLoginResponse { Error = "The login process is failed" };
            }
            AppUser user = null;
            user = await this.FindByNameAsync(request.UserName);
            if (user == null && request.UserName.Contains("@"))
                user = await this.FindByEmailAsync(request.UserName);
            if (user == null)
            {
                return new OpenIdConnectLoginResponse { Error = "No user has this credential" };
            }
            // TODO - implement password check

            //await _signInManager.SignOutAsync();

            //Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager
            //    .PasswordSignInAsync(user, request.Password, false, false);
            if (user.PasswordHash == request.Password)
            {
                return new OpenIdConnectLoginResponse { Succeeded = true };
            }
            return new OpenIdConnectLoginResponse { Succeeded = false, Error = "Login failed" };
        }

        private bool validateOpenIdLoginRequest(OpenIdConnectLoginRequest request)
        {
            if (request.Code == null || request.UserName == null || request.Password == null)
                return false;
            return true;
        }

        private List<AppUser> ReadUsersFromFile(string filename)
        {
            var json = File.ReadAllText(filename);
            var users = JsonConvert.DeserializeObject<List<AppUser>>(json);

            // Copy password into correct property
            foreach (var user in users)
            {
                user.PasswordHash = user.Password;
            }

            return users;
        }
    }
}

