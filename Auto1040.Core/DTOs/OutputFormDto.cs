namespace Auto1040.Core.DTOs
{
    public class OutputFormDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Year { get; set; }
        public string S3Url { get; set; }
        public string S3Key { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
