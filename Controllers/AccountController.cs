using GPAttendSystemAPI.Data.DTOs;
using GPAttendSystemAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GPAttendSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration configuration;

      
        //[HttpPost("registerprod")]
        //public async Task<IActionResult> RegisterNewUser(dtoNewUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var appUser = new AppUser
        //        {
        //            UserName = user.userName,
        //            Email = user.email,

        //        };

        //        var result = await _userManager.CreateAsync(appUser, user.password);
        //        if (result.Succeeded)
        //        {
        //            return Ok("Success");
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //        }
        //    }
        //    return BadRequest(ModelState);
        //}


        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(dtoLogin login)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByNameAsync(login.userName);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, login.password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id)); 
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        

                        // Add role-based claims
                        if (user.UserName == "DrEman")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "DataMining"));
                        }
                        else if (user.UserName == "DrMohamed")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "ExpertSystem"));
                        }
                        


                        // Create and sign the JWT token
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: sc
                        );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Name is invalid");
                }
            }
            return BadRequest(ModelState);
        }




        [HttpGet("Schedule"), Authorize]
        public ActionResult<object> GetMe()
        {

            var username = User.FindFirstValue(ClaimTypes.Name);
    
            var Subjects = User.FindFirstValue(ClaimTypes.Role);

            var Hall = "406";


            return Ok(new { username,Subjects, Hall });
        }
    }


}

