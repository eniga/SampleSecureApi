using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleSecureApi.Areas.Identity.Data;
using SampleSecureApi.Data;
using SampleSecureApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SampleSecureApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly AppSecurity appSecurity;
        private readonly SampleSecureApiContext _dbContext;
        private readonly UserManager<SampleSecureApiUser> _userManager;
        private readonly SignInManager<SampleSecureApiUser> _signInManager;

        public AuthController(IOptions<AppSecurity> options, SampleSecureApiContext dbContext, UserManager<SampleSecureApiUser> userManager, SignInManager<SampleSecureApiUser> signInManager)
        {
            appSecurity = options.Value;
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("getToken")]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken([FromBody] MyLoginModelType myLoginModel)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == myLoginModel.Email);
            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, myLoginModel.Password, false);

                if (signInResult.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSecurity.SecurityKey);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new(ClaimTypes.Name, myLoginModel.Email)
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return Ok("Failed, Try again");
                }
            }
            return Ok("Failed, Try again");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] MyLoginModelType myLoginModel)
        {
            SampleSecureApiUser sampleSecureApiUser = new SampleSecureApiUser()
            {
                Email = myLoginModel.Email,
                UserName = myLoginModel.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(sampleSecureApiUser, myLoginModel.Password);

            if (result.Succeeded)
            {
                return Ok(new { Result = "Regiser Success" });
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach(var error in result.Errors)
                {
                    stringBuilder.Append(error.Description);
                    stringBuilder.Append("\r\n");
                }
                return Ok(new { Result = $"Register Failed: {stringBuilder.ToString()}" });
            }
        }
    }
}
