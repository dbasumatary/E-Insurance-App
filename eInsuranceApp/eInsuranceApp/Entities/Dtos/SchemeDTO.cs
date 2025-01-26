namespace eInsuranceApp.Entities.Dtos
{
    public class SchemeDTO
    {
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public string SchemeDetails { get; set; }
        public PlanDTO Plan { get; set; }
    }
}
