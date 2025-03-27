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
        public int? UserId { get; set; }
        public User User { get; set; }
        public decimal TotalIncomeILS { get; set; }  
        public decimal TotalIncomeUSD { get; set; }  
        public decimal? Field158_172 { get; set; }//Salary
        public decimal? Field218_219 { get; set; }//Continuing education fund
        public decimal? Field248_249 { get; set; }//Pension fund
        public decimal? Field36 { get; set; }//Tax-exempt salary
        public int TaxYear { get; set; }
        public decimal ExchangeRate { get; set; }
        public string S3Key { get; set; }
        public string S3Url { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
