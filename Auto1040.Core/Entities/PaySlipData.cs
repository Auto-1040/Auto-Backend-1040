using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto1040.Core.Entities
{
    [Table("PaySlipData")]
    public class PaySlip
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

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
