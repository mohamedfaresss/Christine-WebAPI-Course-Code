using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIDotNet.DTO;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> UserManager, IConfiguration config)
        {
            userManager = UserManager;
            this.config = config;
        }

        // Register new user
        [HttpPost("Register")] // api/Account/Register
        public async Task<IActionResult> Register(RegisterDto userFromRequest)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = userFromRequest.UserName;
                user.Email = userFromRequest.Email;

                IdentityResult result =
                    await userManager.CreateAsync(user, userFromRequest.Password);

                if (result.Succeeded)
                {
                    return Ok("Created");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }

            return BadRequest(ModelState);
        }

        // Login user and generate JWT token
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto userFromRequest)
        {
            if (ModelState.IsValid)
            {
                // check 
                ApplicationUser userFromDB =
                      await userManager.FindByNameAsync(userFromRequest.UserName);
                if (userFromDB != null)
                {
                    bool found =
                        await userManager.CheckPasswordAsync(userFromDB, userFromRequest.Password);
                    if (found == true)
                    {
                        List<Claim> userClaims = new List<Claim>();
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDB.Id));
                        userClaims.Add(new Claim(ClaimTypes.Name, userFromDB.UserName));

                        // add roles to claims
                        var userRoles = await userManager.GetRolesAsync(userFromDB);
                        foreach (var roleName in userRoles)
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, roleName));
                        }

                        var signInKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecurityKey"]));

                        SigningCredentials signingCred =
                            new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

                        // design token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            audience: config["JWT:AudienceIP"],
                            issuer: config["JWT:IssuerIP"],
                            expires: DateTime.Now.AddHours(1),
                            claims: userClaims,
                            signingCredentials: signingCred
                        );

                        // generate token response
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = DateTime.Now.AddHours(1) // mytoken.ValidTo
                        });
                    }
                    ModelState.AddModelError("UserName", "UserName OR Password Invalid");
                }
            }
            return BadRequest();
        }
    }
}
