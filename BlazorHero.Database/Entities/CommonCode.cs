using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class CommonCode
    {
        public long Id { get; set; }
        public string? CodeType { get; set; }
        public string CodeKey { get; set; } = null!;
        public string CodeValue { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? DisplayOrder { get; set; }
        public long? ParentCommonCodeId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
