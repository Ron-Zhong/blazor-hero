using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class PreviewAccount
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Remarks { get; set; }
        public DateTime? EndAt { get; set; }
    }
}
