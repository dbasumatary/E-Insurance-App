using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Dtos;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyBL _policyBL;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PolicyController> _logger;


        public PolicyController(IPolicyBL policyBL, IUnitOfWork unitOfWork, ILogger<PolicyController> logger)
        {
            _policyBL = policyBL;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyCreateDTO policyCreateDTO)
        {
            if (policyCreateDTO == null)
            {
                return BadRequest("Policy data is null.");
            }

            try
            {
                
                var policy = new Policy
                {
                    CustomerID = policyCreateDTO.CustomerID,
                    SchemeID = policyCreateDTO.SchemeID,
                    PolicyDetails = policyCreateDTO.PolicyDetails,
                    //BasePremiumRate = policyCreateDTO.BasePremiumRate,
                    //Premium = premiumRate.CalculatedPremium, 
                    DateIssued = policyCreateDTO.DateIssued,
                    MaturityPeriod = policyCreateDTO.MaturityPeriod,
                    PolicyLapseDate = policyCreateDTO.PolicyLapseDate,
                    CreatedAt = DateTime.UtcNow
                };

                await _policyBL.CreatePolicyAsync(policy);
                return Ok(new { Message = "Policy created successfully", Policy = policy });
                //return CreatedAtAction(nameof(GetPolicy), new { policyId = policy.PolicyID }, policy);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the policy.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicyDetailsById(int policyId)
        {
            try
            {
                if (policyId <= 0)
                {
                    _logger?.LogWarning("Invalid PolicyID: {PolicyID}", policyId);
                    return BadRequest(new { Message = "Invalid Policy ID." });
                }

                var policy = await _policyBL.GetPolicyDetailsByIdAsync(policyId);

                if (policy == null)
                {
                    _logger?.LogWarning("Policy not found for PolicyID: {PolicyID}", policyId);
                    return NotFound(new { Message = "Policy not found." });
                }

                _logger?.LogInformation("Policy details returned for PolicyID: {PolicyID}", policyId);
                return Ok(policy);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error for PolicyID: {PolicyID}", policyId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error" });
            }
        }


        [HttpGet("GetAllPolicies")]
        public async Task<IActionResult> GetPolicies()
        {
            _logger.LogInformation("Fetching all policies.");
            try
            {
                var policies = await _policyBL.GetPoliciesAsync();
                return Ok(policies);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching policies: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchPolicyDetails([FromQuery] int? policyId, [FromQuery] string customerName)
        {
            try
            {
                var policy = await _policyBL.SearchPolicyDetailsAsync(policyId, customerName);
                if (policy == null)
                {
                    return NotFound();
                }
                return Ok(policy);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while searching for policy details: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        /*[HttpPost("CreatePolicy")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyCreateDTO policyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var policy = new Policy
                {
                    CustomerID = policyDto.CustomerID,
                    SchemeID = policyDto.SchemeID,
                    PolicyDetails = policyDto.PolicyDetails,
                    Premium = policyDto.Premium,
                    DateIssued = policyDto.DateIssued,
                    MaturityPeriod = policyDto.MaturityPeriod,
                    PolicyLapseDate = policyDto.PolicyLapseDate
                };

                await _policyBL.CreatePolicyAsync(policy);

                return Ok(new { Message = "Policy created successfully." });
            }
            catch (Exception ex) {
                return StatusCode(500, new { Message = "An error occurred", Detail = ex.Message });

            }

        }*/


        //[Authorize(Roles = "Admin")]
        //[HttpGet("{policyId}")]
        //public async Task<IActionResult> GetPolicy(int policyId)
        //{
        //    try
        //    {
        //        var policyDTO = await _policyBL.GetPolicyAsync(policyId);
        //        return Ok(policyDTO);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        //    }
        //}
    }
}
