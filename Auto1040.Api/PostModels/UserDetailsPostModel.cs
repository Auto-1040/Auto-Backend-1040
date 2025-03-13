using Auto1040.Core.Entities;
using System.Text.Json.Serialization;

namespace Auto1040.Api.PostModels
{
    public class UserDetailsPostModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Ssn { get; set; }

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

        public bool? PresidentialCampaign { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FilingStatus? FilingStatus { get; set; }

        public string? Dependents { get; set; }
    }
}
