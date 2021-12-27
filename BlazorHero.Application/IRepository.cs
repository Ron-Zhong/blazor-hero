using Microsoft.AspNetCore.Http;

public interface IRepository<T>
{
    Task<T> GetAsync(string index);
    Task<bool> IsExisted(string index);

    IQueryable<T> QueryAll();
    Task<RepoResult> CreateAsync(T dto);
    Task<RepoResult> SaveAsync(T dto);
    Task<RepoResult> RemoveAsync(T dto);
}

public record RepoResult
{
    public int Affected { get; set; }
    public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
    public string Message { get; set; } = string.Empty;
    public object? Item { get; set; }

    public RepoResult()
    {
    }

    public RepoResult(int statusCode, int affected = 0, string message = "")
    {
        StatusCode = statusCode;
        Message = message;
        Affected = affected;
    }
}
