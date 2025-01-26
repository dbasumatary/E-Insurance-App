using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.EmployeeDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeBL _employeeBL;

        public EmployeeController(IEmployeeBL employeeBL)
        {
            _employeeBL = employeeBL;
        }


        [HttpPost("register")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegistrationRequest employeeDTO)
        {
            try
            {
                var result = await _employeeBL.RegisterEmployeeAsync(employeeDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

