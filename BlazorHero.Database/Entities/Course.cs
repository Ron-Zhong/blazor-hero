using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CourseNo { get; set; }
        public string CourseHash { get; set; } = null!;
        public string? ModuleCode { get; set; }
        public string? CourseCode { get; set; }
        public string? CountryCode { get; set; }
        public string? Profession { get; set; }
        public string? Specilties { get; set; }
        public bool? PublicAccess { get; set; }
        public bool? PrivateAccess { get; set; }
        public string? PublicUrl { get; set; }
        public string? PrivateUrl { get; set; }
        public int SeatsQuota { get; set; }
        public DateTime? FullAt { get; set; }
        public string? Status { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? StoppedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? Title { get; set; }
        public string? Thumbnail { get; set; }
        public string? Introduction { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Length { get; set; }
        public string? Sponsor { get; set; }
        public string? ProgramCode { get; set; }
        public string? ProgramProvider { get; set; }
        public string? Cpdpoints { get; set; }
        public bool HavingQuiz { get; set; }
        public string? QuizIntro { get; set; }
        public string? QuizContext { get; set; }
        public double PassingScore { get; set; }
        public string? LegacyUrl { get; set; }
        public Guid? CertificateId { get; set; }
        public bool? IsLimitedSeats { get; set; }
        public int PrivateEnrolled { get; set; }
        public int PublicEnrolled { get; set; }
        public string? Remarks { get; set; }
        public string? LegacyId { get; set; }
        public bool? HavingCertificate { get; set; }
        public string? QuizFooter { get; set; }
        public int MaxAttempts { get; set; }
        public string? AttributionMode { get; set; }
        public DateTime? PausedAt { get; set; }
        public DateTime? InterstitialAdsEndAt { get; set; }
        public string? InterstitialAdsImgUrl { get; set; }
        public DateTime? InterstitialAdsStartAt { get; set; }
        public bool? HavingInterstitialAds { get; set; }
        public bool IsMimsBrand { get; set; }

        public virtual Certificate? Certificate { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
