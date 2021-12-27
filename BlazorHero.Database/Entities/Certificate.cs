using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class Certificate
    {
        public Certificate()
        {
            Courses = new HashSet<Course>();
        }

        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Name { get; set; }
        public bool IsCountryDefault { get; set; }
        public string? CountryCode { get; set; }
        public string? TemplateUrl { get; set; }
        public string? SnapshotUrl { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
