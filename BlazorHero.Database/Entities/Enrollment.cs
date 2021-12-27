using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class Enrollment
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int SsoId { get; set; }
        public int CourseNo { get; set; }
        public string? Mode { get; set; }
        public string? Status { get; set; }
        public int Views { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Attempts { get; set; }
        public double Score { get; set; }
        public DateTime? PassedAt { get; set; }
        public string? CertificateUrl { get; set; }
        public string? CertificateSnapshotUrl { get; set; }
        public DateTime? CertificateIssuedAt { get; set; }
        public string? PointsIssued { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public int LegacyUserId { get; set; }
        public string? LegacyCourseId { get; set; }
        public DateTime? Attempted { get; set; }
        public DateTime? EnrolledAt { get; set; }
        public bool? IsSubmitted { get; set; }
        public DateTime? AttemptedAt { get; set; }
        public string? Remarks { get; set; }
        public string? Accreditation { get; set; }

        public virtual Course CourseNoNavigation { get; set; } = null!;
        public virtual User Sso { get; set; } = null!;
    }
}
