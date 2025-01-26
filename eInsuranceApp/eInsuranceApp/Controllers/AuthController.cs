using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginBL _loginBL;
        public AuthController(ILoginBL loginBL)
        {
            _loginBL = loginBL;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginEntity loginEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(loginEntity.EmailOrUsername) || string.IsNullOrEmpty(loginEntity.Password))
                {
                    return BadRequest("Email/Username and Password are required.");
                }

                var token = await _loginBL.Login(loginEntity.EmailOrUsername, loginEntity.Password);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}

