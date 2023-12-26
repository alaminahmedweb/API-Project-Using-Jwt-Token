using API_Project_With_JWT_Token.ViewModel;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Project_With_JWT_Token.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AccountController : ControllerBase
    {
        private readonly ITokenClaimService _tokenClaimService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public AccountController(ITokenClaimService tokenClaimService,
            UserManager<ApplicationUser> userManager,
            IAuthService authService)
        {
            this._tokenClaimService = tokenClaimService;
            this._userManager = userManager;
            this._authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin login)
        {
            if(!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }

            if(await _authService.IsAuthenticated(login.UserName, login.Password))
            {
                var tokenString = await _tokenClaimService.GetTokenAsync(userName: login.UserName);
                return Ok(tokenString);
            }
            return BadRequest("Invalid User Name or Password..");
        }
    }
}
