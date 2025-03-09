namespace Auto1040.Api.PostModels
{
    public class OutputFormPostModel
    {
        public int UserId { get; set; }
        public int Year { get; set; }
        public string FilePath { get; set; }
        public string S3Key { get; set; }
        public bool IsDeleted { get; set; }
    }
}
