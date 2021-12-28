    using BlazorHero.Database;
    using Microsoft.EntityFrameworkCore;

    public class CourseRepo : IRepository<CourseModel>
    {
        private readonly DBContext _context;

        public CourseRepo(DBContext context)
        {
            _context = context;
        }

        public IQueryable<CourseModel> QueryAll()
        {
            return _context.Courses
                .Select(x => new CourseModel()
                {
                    Title = x.Title,
                    Status = x.Status,
                    CourseNo = x.CourseNo,
                    CreatedAt = x.CreatedAt,
                    Thumbnail = x.Thumbnail,
                    Profession = x.Profession,
                    Introduction = x.Introduction,
                })
                .AsNoTracking()
                .AsQueryable();
        }

        public async Task<CourseModel?> GetAsync(int id)
        {
            return await _context.Courses
                .Select(x => new CourseModel()
                {
                    Title = x.Title,
                    Status = x.Status,
                    Content = x.Content,
                    CourseNo = x.CourseNo,
                    CreatedAt = x.CreatedAt,
                    Thumbnail = x.Thumbnail,
                    Profession = x.Profession,
                    Description = x.Description,
                    Introduction = x.Introduction,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseNo == id);
        }
    }
