using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.RepositoryLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eInsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentBL _paymentBL;
        private readonly ICustomerRL _customerRL;
        private readonly IPremiumBL _premiumBL;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentBL paymentBL, ICustomerRL customerRL, IPremiumBL premiumBL, ILogger<PaymentController> logger)
        {
            _paymentBL = paymentBL;
            _customerRL = customerRL;
            _premiumBL = premiumBL;
            _logger = logger;
        }

        [HttpPost]
        [Route("ProcessPayment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentDTO paymentDto)
        {
            if (paymentDto == null)
            {
                _logger.LogWarning("Received null payment request.");
                return BadRequest("Payment details cannot be null.");
            }

            try
            {
                var payment = new PaymentEntity
                {
                    CustomerID = paymentDto.CustomerID,
                    PolicyID = paymentDto.PolicyID,
                    PaymentDate = paymentDto.PaymentDate,
                    PaymentType = paymentDto.PaymentType,
                    CreatedAt = paymentDto.CreatedAt,
                    PremiumID = paymentDto.PremiumID
                    
                };
                await _paymentBL.InsertPaymentsAsync(payment);
                _logger.LogInformation($"Payment added successfully for CustomerID: {paymentDto.CustomerID}, PolicyID: {paymentDto.PolicyID}");
                return Ok("Payment added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding payment.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentDetails(int paymentId)
        {
            var paymentDetails = await _paymentBL.GetPaymentDetailsByIdAndLog(paymentId);

            if (paymentDetails == null)
            {
                return NotFound(new { Message = "Payment details not found." });
            }

            return Ok(paymentDetails);
        }






        //[HttpGet]
        //[Route("{id}")]
        //public async Task<IActionResult> GetPaymentDetail(int id)
        //{
        //    var payment = await _paymentBL.GetPaymentsByIdAsync(id);
        //    if (payment == null)
        //    {
        //        return NotFound(new {message = "No payment found"});
        //    }
        //    return Ok(payment);
        //}
            
    }
}
