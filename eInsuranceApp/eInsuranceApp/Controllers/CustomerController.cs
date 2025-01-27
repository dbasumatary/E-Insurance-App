using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.RepositoryLayer.Implementation;
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
        private readonly ILogger<CustomerRL> _logger;

        public CustomerController(ICustomerBL customerBL, ILogger<CustomerRL> logger)
        {
            _customerBL = customerBL;
            _logger = logger;
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


        [HttpGet("{customerId}/policies")]
        public async Task<IActionResult> GetPolicies(int customerId)
        {
            try
            {

                var policies = await _customerBL.GetPoliciesForCustomerAsync(customerId);

                if (policies == null)
                {
                    _logger.LogWarning("No policies found for CustomerID: {CustomerID}", customerId);
                    return NotFound(new { message = "No policies found for the given customer." });
                }

                return Ok(policies);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid request for CustomerID: {CustomerID}", customerId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching policies for CustomerID: {CustomerID}", customerId);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
    }
}
