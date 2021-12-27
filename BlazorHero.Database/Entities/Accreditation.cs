﻿using System;
using System.Collections.Generic;

namespace BlazorHero.Database
{
    public partial class Accreditation
    {
        public Guid Id { get; set; }
        public string? CountryCode { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}