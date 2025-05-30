﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto1040.Core.Entities
{
    public enum FilingStatus
    {
        Single,
        MarriedFilingJointly, // Married filing jointly (even if only one had income)
        MarriedFilingSeparately, // MFS
        HeadOfHousehold, // HOH
        QualifyingSurvivingSpouse // QSS
    }

    [Table("UserDetails")]
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }


        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(20)]
        public string? Ssn { get; set; }

        [MaxLength(50)]
        public string? SpouseFirstName { get; set; }

        [MaxLength(50)]
        public string? SpouseLastName { get; set; }

        [MaxLength(20)]
        public string? SpouseSsn { get; set; }

        [MaxLength(255)]
        public string? HomeAddress { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? ZipCode { get; set; }

        [MaxLength(100)]
        public string? ForeignCountry { get; set; }

        [MaxLength(100)]
        public string? ForeignState { get; set; }

        [MaxLength(20)]
        public string? ForeignPostalCode { get; set; }

        public bool? PresidentialCampaign { get; set; }

        [MaxLength(50)]
        public FilingStatus? FilingStatus { get; set; }

        public string? Dependents { get; set; }
    }
}

