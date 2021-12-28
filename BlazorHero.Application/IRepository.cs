public interface IRepository<T>
{
    IQueryable<T> QueryAll();
    Task<T> GetAsync(int id);
}
