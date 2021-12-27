using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class User
    {
        public User()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int SsoId { get; set; }
        public string Email { get; set; } = null!;
        public string? CountryCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Title { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Profession { get; set; }
        public string? Specialty { get; set; }
        public string? SubSpecialty { get; set; }
        public string? CusSubSpecialty { get; set; }
        public string? LicenceNumber { get; set; }
        public string? VerificationStatus { get; set; }
        public DateTime? LastAccessAt { get; set; }
        public int LegacyId { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public string? Role { get; set; }
        public string? WorkplaceType { get; set; }
        public string? PracticeType { get; set; }
        public string? IdentityCardNo { get; set; }
        public string? PracticeName { get; set; }
        public string? Membership { get; set; }
        public string? WorkplaceState { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
