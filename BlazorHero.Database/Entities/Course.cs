using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class Course
    {
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
    }
}
