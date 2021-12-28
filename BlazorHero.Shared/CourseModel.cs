public record CourseModel
{
    public int CourseNo { get; init; }
    public string? Title { get; init; }
    public string? Status { get; init; }
    public string? Thumbnail { get; init; }
    public string? Profession { get; init; }
    public string? Description { get; init; }
    public string? Content { get; init; }
    public string? Introduction { get; init; }
    public DateTime? CreatedAt { get; set; }
}