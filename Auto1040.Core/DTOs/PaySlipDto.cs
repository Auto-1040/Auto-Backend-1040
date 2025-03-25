namespace Auto1040.Core.DTOs
{
    public class PaySlipDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal? F158_172 { get; set; }//Salary
        public decimal? F218_219 { get; set; }//Continuing education fund
        public decimal? F248_249 { get; set; }//Pension fund
        public decimal? F36 { get; set; }//Tax-exempt salary
    }
}
