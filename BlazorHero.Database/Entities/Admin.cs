using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class Admin
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Regions { get; set; }
        public string? Department { get; set; }
        public DateTime? LastAccessAt { get; set; }
        public string? Photo { get; set; }
        public string? Title { get; set; }
    }
}
