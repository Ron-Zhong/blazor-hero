public record CourseModel
{
    public int CourseNo { get; init; }
    public string CourseHash { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string CountryCode { get; init; } = string.Empty;
    public string CourseCode { get; init; } = string.Empty;
    public string Profession { get; init; } = string.Empty;
    public string Thumbnail { get; init; } = string.Empty;
    public string Introduction { get; init; } = string.Empty;
}