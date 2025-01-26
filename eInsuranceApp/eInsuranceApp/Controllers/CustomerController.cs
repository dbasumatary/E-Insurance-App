using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.CustomerDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBL _customerBL;

        public CustomerController(ICustomerBL customerBL)
        {
            _customerBL = customerBL;
        }

        [HttpPost("register")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationRequest customerDTO)
        {
            try
            {
                var result = await _customerBL.RegisterCustomerAsync(customerDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
