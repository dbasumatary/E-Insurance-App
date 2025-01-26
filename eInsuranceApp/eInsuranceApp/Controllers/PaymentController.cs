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


        /*[HttpPost]
        [Route("InsertPayment")]
        public async Task<IActionResult> InsertPayment([FromBody] PaymentDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _paymentBL.InsertPaymentsAsync(request);

                if (result)
                {
                    return Ok(new { Message = "Payment processed successfully." });
                }

                return BadRequest(new { Message = "Failed to process payment." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the payment.", Details = ex.Message });
            }
        }*/



        /*[HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var customer = await _customerRL.GetCustomerByIdAsync(paymentdto.CustomerID);
                if (customer == null)
                    return NotFound(new { message = "Customer not found." });

                var customerAge = DateTime.Now.Year - customer.DateOfBirth.Year;

                //var premium = await _premiumBL.GetCalculatedPremiumAsync(paymentdto.CustomerID, paymentdto.PolicyID, paymentdto.SchemeID, paymentdto.BaseRate, customerAge);
                var premiumDetails = new PremiumCalculationDTO
                {
                    CustomerID = paymentdto.CustomerID,
                    SchemeID = paymentdto.SchemeID,
                    Age = customerAge,
                    BaseRate = paymentdto.BaseRate
                };
                var calculatedPremium = await _premiumBL.CalculatePremiumAsync(premiumDetails);


                var payment = new PaymentEntity
                {
                    CustomerID = paymentdto.CustomerID,
                    PolicyID = paymentdto.PolicyID,
                    PaymentType = paymentdto.PaymentType,
                    Amount = calculatedPremium,
                    PaymentDate = paymentdto.PaymentDate,
                    Status = paymentdto.Status,
                    CreatedAt = paymentdto.CreatedAt,
                };
                await _paymentBL.InsertPaymentsAsync(payment);
                return Ok(new { message = "Success Payment" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error during payment ", ex.Message });
            }
        }*/




        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPaymentDetail(int id)
        {
            var payment = await _paymentBL.GetPaymentsByIdAsync(id);
            if (payment == null)
            {
                return NotFound(new {message = "No payment found"});
            }
            return Ok(payment);
        }
            
    }
}
