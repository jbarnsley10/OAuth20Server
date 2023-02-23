using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuth20.Server.OauthRequest;
using OAuth20.Server.Services;
using OAuth20.Server.Services.CodeServce;

namespace OAuth20.Server.Controllers
{
    public class TfpController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthorizeResultService authorizeResultService;
        private readonly ICodeStoreService codeStoreService;
        private readonly IInMemoryUserManager userManager;

        public TfpController(IHttpContextAccessor httpContextAccessor, IAuthorizeResultService authorizeResultService,
            ICodeStoreService codeStoreService, IInMemoryUserManager userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authorizeResultService = authorizeResultService;
            this.codeStoreService = codeStoreService;
            this.userManager = userManager;
        }

        [Route("/tfp/localtestdomain/b2c_1a_signupsignin/oauth2/v2.0/token")]
        public async Task<IActionResult> Index(TokenRequest tokenRequest)
        {
            var homeController = new HomeController(this.httpContextAccessor, this.authorizeResultService, this.codeStoreService, this.userManager);
            return await Task.FromResult(homeController.Token(tokenRequest));
        }
    }
}
