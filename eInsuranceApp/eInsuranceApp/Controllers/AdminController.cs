using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBL _adminBL;

        public AdminController(IAdminBL adminBL)
        {
            _adminBL = adminBL;
        }

        [HttpPost("register")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] AdminRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _adminBL.RegisterAdminAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
