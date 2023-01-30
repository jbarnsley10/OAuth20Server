using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuth20.Server.OauthRequest;
using OAuth20.Server.Services;
using OAuth20.Server.Services.CodeServce;
using OAuth20.Server.Services.Users;

namespace OAuth20.Server.Controllers
{
    public class TfpController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthorizeResultService authorizeResultService;
        private readonly ICodeStoreService codeStoreService;
        private readonly IUserManagerService userManagerService;

        public TfpController(IHttpContextAccessor httpContextAccessor, IAuthorizeResultService authorizeResultService,
            ICodeStoreService codeStoreService, IUserManagerService userManagerService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authorizeResultService = authorizeResultService;
            this.codeStoreService = codeStoreService;
            this.userManagerService = userManagerService;
        }

        [Route("/tfp/localtestdomain/b2c_1a_signupsignin/oauth2/v2.0/token")]
        public async Task<IActionResult> Index(TokenRequest tokenRequest)
        {
            var homeController = new HomeController(this.httpContextAccessor, this.authorizeResultService, this.codeStoreService, this.userManagerService);
            return await homeController.Token(tokenRequest);
        }
    }
}
