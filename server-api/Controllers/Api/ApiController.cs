using System.Data.Common;
using System;
using Microsoft.AspNetCore.Identity;
using server_api.Data.Models;
using System.Threading.Tasks;
using server_api.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using server_api.Data.Models.Repositories;
using server_api.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace server_api.Controllers
{

    [ApiController]
    [Route(Constants.Strings.General.ApiDerictory + "/login")]
    public class ApiController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        //private readonly IJwtFactory _jwtFactory;
        //private readonly JwtIssuerOptions _jwtOptions;

        public ApiController(UserManager<User> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.userManager = userManager;
            //this._jwtFactory = jwtFactory;
            //this._jwtOptions = jwtOptions.Value;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JwtCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return Forbid();
            }
            var identity = await Repository.GetClaimsIdentity(credentials.UserName, credentials.Password, userManager);
            if (identity == null)
            {
                return Forbid();
            }
            //var jwt = await _jwtFactory.GenerateJwt(identity, credentials.UserName);
            return new OkObjectResult(null/*jwt*/);
        }
    }
}
