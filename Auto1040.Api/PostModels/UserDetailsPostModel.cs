namespace Auto1040.Api.PostModels
{
    public class UserDetailsPostModel
    {
        public int UserId { get; set; }

        public string Ssn { get; set; }

        public string? SpouseFirstName { get; set; }

        public string? SpouseLastName { get; set; }

        public string? SpouseSsn { get; set; }

        public string? HomeAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? ForeignCountry { get; set; }

        public string? ForeignState { get; set; }

        public string? ForeignPostalCode { get; set; }

        public bool PresidentialCampaign { get; set; }

        public string? FilingStatus { get; set; }

        public string? Dependents { get; set; }
    }
}
