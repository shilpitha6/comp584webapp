using comp584webapp.DTO;
using datamodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace comp584webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin(UserManager<AppUser> userManager, JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginrequest)
        {
            AppUser? user = await userManager.FindByNameAsync(loginrequest.UserName);
            if (user == null)
            {
                return Unauthorized("Bad UserName");
            }
            bool success = await userManager.CheckPasswordAsync(user, loginrequest.Password);

            if (!success)
            {
                return Unauthorized("Bad Password");
            }

            JwtSecurityToken token = await jwtHandler.GetSecurityTokenAsync(user);

            string jwtstring = new JwtSecurityTokenHandler().WriteToken(token);

            LoginRespond respond = new()
            {
                Success = true,
                Message = "finally",
                Token = jwtstring,

            };
            return Ok(respond);
        }
    }
}
