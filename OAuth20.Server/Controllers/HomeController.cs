/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuth20.Server.OauthRequest;
using OAuth20.Server.Services;
using OAuth20.Server.Services.CodeServce;
using System.Threading.Tasks;

namespace OAuth20.Server.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizeResultService _authorizeResultService;
        private readonly ICodeStoreService _codeStoreService;
        private readonly IInMemoryUserManager _userManager;

        public HomeController(IHttpContextAccessor httpContextAccessor, IAuthorizeResultService authorizeResultService,
            ICodeStoreService codeStoreService, IInMemoryUserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizeResultService = authorizeResultService;
            _codeStoreService = codeStoreService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authorize(AuthorizationRequest authorizationRequest)
        {
            var result = _authorizeResultService.AuthorizeRequest(_httpContextAccessor, authorizationRequest);

            if (result.HasError)
                return RedirectToAction("Error", new { error = result.Error });

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var updateCodeResult = _codeStoreService.UpdatedClientDataByCode(result.Code, result.RequestedScopes, string.Empty);
                if (updateCodeResult != null)
                {
                    if (authorizationRequest.response_type == "code")
                    {
                        result.RedirectUri = result.RedirectUri.Replace("signin-oidc", "") + "/token&code=" + result.Code + "&redirectUri=" + result.RedirectUri;
                    }
                    else
                    {
                        result.RedirectUri = result.RedirectUri + "&code=" + result.Code;
                    }
                    return Redirect(result.RedirectUri);
                }
                else
                {
                    return RedirectToAction("Error", new { error = "invalid_request" });
                }
            }

            var loginModel = new OpenIdConnectLoginRequest
            {
                RedirectUri = result.RedirectUri,
                Code = result.Code,
                RequestedScopes = result.RequestedScopes,
            };
            return View("Login", loginModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(OpenIdConnectLoginRequest loginRequest)
        {
            // here I have to check if the username and passowrd is correct
            // and I will show you how to integrate the ASP.NET Core Identity
            // With our framework
            if (!loginRequest.IsValid())
                return RedirectToAction("Error", new { error = "invalid_request" });
            var userLoginResult = await _userManager.LoginUserByOpenIdAsync(loginRequest);
            if (userLoginResult.Succeeded)
            {
                var result = _codeStoreService.UpdatedClientDataByCode(loginRequest.Code, loginRequest.RequestedScopes, loginRequest.UserName);
                if (result != null)
                {
                    loginRequest.RedirectUri = loginRequest.RedirectUri + "&code=" + loginRequest.Code;
                    return Redirect(loginRequest.RedirectUri);
                }
            }
            return RedirectToAction("Error", new { error = "invalid_request" });
        }

        [HttpPost]
        public async Task<JsonResult> Token(TokenRequest tokenRequest)
        {
            var result = await _authorizeResultService.GenerateTokenAsync(tokenRequest);

            if (result.HasError)
                return Json(new
                {
                    error = result.Error,
                    error_description = result.ErrorDescription
                });

            return Json(result);
        }

        [HttpGet]
        public IActionResult Logout(string post_logout_redirect_uri)
        {
            this.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return Redirect(post_logout_redirect_uri.Replace("signout-callback-oidc", ""));
        }

        public IActionResult Error(string error)
        {
            return View(error);
        }
    }
}
