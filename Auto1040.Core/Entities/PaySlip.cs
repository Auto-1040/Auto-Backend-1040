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
        public decimal TotalIncomeILS { get; set; }  
        public decimal TotalIncomeUSD { get; set; }  
        public decimal? F158_172 { get; set; }//Salary
        public decimal? F218_219 { get; set; }//Continuing education fund
        public decimal? F248_249 { get; set; }//Pension fund
        public decimal? F36 { get; set; }//Tax-exempt salary
        public decimal ExchangeRate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
