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

        public AccountController(UserManager<ApplicationUser> UserManager , IConfiguration config)
        {
            userManager = UserManager;
            this.config = config;
        }

        [HttpPost("Register")]//api/Account/Register
        public async Task<IActionResult> Register(RegisterDto UserFromReguest)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser user = new ApplicationUser();
                user.UserName = UserFromReguest.UserName;
                user.Email = UserFromReguest.Email;
                IdentityResult result =
                    await userManager.CreateAsync(user, UserFromReguest.Password);
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto UserFromReguest)
        {
            if (ModelState.IsValid)
            {
                //check 
                ApplicationUser userFromDB =
                      await userManager.FindByNameAsync(UserFromReguest.UserName);
                if (userFromDB != null)
                {
                    bool found =
                        await userManager.CheckPasswordAsync(userFromDB, UserFromReguest.Password);
                    if (found == true)
                    {   

                        List<Claim> UserClaims = new List<Claim>();
                        UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDB.Id));
                        UserClaims.Add(new Claim(ClaimTypes.Name, userFromDB.UserName));

                        // مش فاهمها 
                        var UserRoles = await userManager.GetRolesAsync(userFromDB);
                        foreach (var roleNAme in UserRoles)
                        {

                            UserClaims.Add(new Claim(ClaimTypes.Role, roleNAme));
                        }
                        //   

                        var SignInKey = 
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                            (config["JWT:SecrityKey"]));

                        SigningCredentials signingCred = 
                            new SigningCredentials
                            (SignInKey, SecurityAlgorithms.HmacSha256);
                        // design token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            audience: config["JWT:AudianceIP"],
                            issuer:   config["JWT:IssuerIP"],
                            expires: DateTime.Now.AddHours(1),
                            claims: UserClaims,
                            signingCredentials :signingCred
                            );

                        // generate token response
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = DateTime.Now.AddHours(1) //mytoken.ValidTo
                        });

                        // generate token
                    }
                    ModelState.AddModelError("UserName", "UserName OR Password InVaild");

                }
            }
            return BadRequest();

            


        }
    }

        }
    
