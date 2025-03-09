namespace Auto1040.Core.DTOs
{
    public class PaySlipDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TipIncome { get; set; }
        public decimal HouseholdWages { get; set; }
        public decimal TaxableDependentBenefits { get; set; }
        public decimal EmployerAdoptionBenefits { get; set; }
        public decimal OtherEarnedIncome { get; set; }
        public decimal TotalTaxableIncome { get; set; }
        public decimal TotalExemptIncome { get; set; }
        public decimal TotalDividends { get; set; }
        public decimal CapitalGains { get; set; }
    }
}
