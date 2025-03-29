namespace Auto1040.Core.DTOs
{
    public class PaySlipDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int TaxYear { get; set; }
        public string S3Key { get; set; }
    }
}
