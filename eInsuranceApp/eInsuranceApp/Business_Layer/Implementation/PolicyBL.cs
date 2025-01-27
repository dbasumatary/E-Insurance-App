using eInsuranceApp.Business_Layer.Interface;
using eInsuranceApp.Email;
using eInsuranceApp.Entities.Dtos;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;
using eInsuranceApp.RepositoryLayer.Implementation;
using eInsuranceApp.RepositoryLayer.Interface;
using eInsuranceApp.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace eInsuranceApp.Business_Layer.Implementation
{
    public class PolicyBL : IPolicyBL
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PolicyBL> _logger;
        private readonly IEmailProducer _emailProducer;

        public PolicyBL(IUnitOfWork unitOfWork, ILogger<PolicyBL> logger, IEmailProducer emailProducer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailProducer = emailProducer;
        }

        public async Task CreatePolicyAsync(Policy policyDTO)
        {
            try
            {
                if (policyDTO == null || policyDTO.CustomerID <= 0 || policyDTO.SchemeID <= 0)
                    throw new ArgumentException("Invalid policy data.");
                
                await _unitOfWork.PolicyRepo.ValidateAndCreatePolicyAsync(policyDTO);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Policy created successfully for CustomerID: {policyDTO.CustomerID}, SchemeID: {policyDTO.SchemeID}");

                // confirmation email
                var customer = await _unitOfWork.CustomerRepo.GetCustomerByIdAsync(policyDTO.CustomerID);
                var emailSubject = "Policy Created Successfully";
                var emailBody = $"Your policy has been created. Policy ID: {policyDTO.PolicyID}";
                await _emailProducer.SendEmailAsync(customer.Email, emailSubject, emailBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the policy");
                throw;
            }
        }


        // Get Policy by ID
        //public async Task<Policy> GetPolicyAsync(int policyId)
        //{
        //    try
        //    {
        //        //var policy = await _policyRepo.GetPolicyByIdAsync(policyId);
        //        var policy = await _unitOfWork.PolicyRepo.GetPolicyByIdAsync(policyId);

        //        if (policy == null)
        //        {
        //            _logger.LogWarning($"Policy with ID {policyId} not found.");
        //            throw new KeyNotFoundException($"Policy with ID {policyId} not found.");
        //        }

        //        var policyDTO = new Policy
        //        {
        //            PolicyID = policy.PolicyID,
        //            CustomerID = policy.CustomerID,
        //            SchemeID = policy.SchemeID,
        //            PolicyDetails = policy.PolicyDetails,
        //            //BasePremiumRate = policy.BasePremiumRate,
        //            //Premium = (decimal)policy.Premium,
        //            DateIssued = policy.DateIssued,
        //            MaturityPeriod = policy.MaturityPeriod,
        //            PolicyLapseDate = policy.PolicyLapseDate
        //        };

        //        _logger.LogInformation($"Fetched policy with ID {policyId} successfully.");
        //        return policyDTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while fetching the policy.");
        //        throw;
        //    }
        //}


        public async Task<PolicyViewDTO> GetPolicyDetailsByIdAsync(int policyId)
        {
            try
            {
                if (policyId <= 0)
                {
                    _logger?.LogWarning("Invalid PolicyID: {PolicyID}", policyId);
                    return null;
                }

                //var policy = await _policyRL.GetPolicyDetailsByIdAsync(policyId);
                var policy = await _unitOfWork.PolicyRepo.GetPolicyDetailsByIdAsync(policyId);

                if (policy == null)
                {
                    _logger?.LogWarning("Policy not found for PolicyID: {PolicyID}", policyId);
                }
                else
                {
                    _logger?.LogInformation("Policy found for PolicyID: {PolicyID}", policyId);
                }

                return policy;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PolicyID: {PolicyID}", policyId);
                throw;
            }
        }

        public async Task<IEnumerable<PolicyViewDTO>> GetPoliciesAsync()
        {
            return await _unitOfWork.PolicyRepo.GetPoliciesAsync();
        }


      


        /*public async Task<decimal> GetBasePayRateByPolicyIdAsync(int policyId)
        {
            try
            {
                var policy = await _unitOfWork.PolicyRepo.GetPolicyByIdAsync(policyId);

                if (policy == null)
                {
                    throw new KeyNotFoundException($"Policy with ID {policyId} not found.");
                }

                _logger.LogInformation($"Found BasePayRate for Policy ID: {policyId}, BasePayRate: {policy.BasePremiumRate}");
                return policy.BasePremiumRate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching BasePayRate for Policy ID: {policyId}");
                throw;
            }
        }*/



        public async Task<decimal?> GetBasePremiumRateAsync(int schemeId)
        {
            return await _unitOfWork.PolicyRepo.GetBasePremiumRateAsync(schemeId);
        }

        public async Task<PolicyViewDTO> SearchPolicyDetailsAsync(int? policyId, string customerName)
        {
            return await _unitOfWork.PolicyRepo.SearchPolicyDetailsAsync(policyId, customerName);
        }
    }
}

