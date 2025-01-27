namespace eInsuranceApp.Entities.Payment
{
    public class PaymentViewDTO
    {
        public int PaymentID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string PolicyDetails { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
