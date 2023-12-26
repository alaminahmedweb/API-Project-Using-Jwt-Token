using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class TokenClaimService : ITokenClaimService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenClaimService(UserManager<ApplicationUser> userManager, 
            IConfiguration configuration) {
            this._userManager = userManager;
            this._configuration = configuration;
        }
        public async Task<string> GetTokenAsync(string userName)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key=Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value);
                var user=await _userManager.FindByNameAsync(userName);
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims.ToArray()),
                    Expires = DateTime.Now.AddMinutes(60),
                    Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                    Audience = _configuration.GetSection("Jwt:Audience").Value,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token=tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
