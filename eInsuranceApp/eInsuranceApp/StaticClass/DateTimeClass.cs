namespace eInsuranceApp.StaticClass
{
    public static class DateTimeClass
    {
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            return age;
        }
    }
}
