public class DTO
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}
