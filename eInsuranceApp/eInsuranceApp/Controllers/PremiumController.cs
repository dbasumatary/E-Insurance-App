using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremiumController : ControllerBase
    {
        private readonly IPremiumBL _premiumBL;
        public PremiumController(IPremiumBL premiumBL)
        {
            _premiumBL = premiumBL;
        }

        [HttpPost]
        public async Task<IActionResult> CalculatePremium([FromBody] PremiumEntity premiumDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //var policy = await _premiumBL.GetPolicyByIdAsync(premiumDetails.PolicyID);
                //if (policy == null)
                //    return NotFound(new { Message = $"Policy with ID {premiumDetails.PolicyID} not found." });


                var prem = new PremiumCalculationDTO
                {
                    CustomerID = premiumDetails.CustomerID,
                    SchemeID = premiumDetails.SchemeID,
                    PolicyID = premiumDetails.PolicyID,
                    BaseRate = premiumDetails.BaseRate,
                };
                var premium = await _premiumBL.CalculatePremiumAsync(prem);
                return Ok(new { CalculatedPremium = premium });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while calculating the premium.", Error = ex.Message });
            }
        }
    }

}
