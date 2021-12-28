using Microsoft.EntityFrameworkCore;
using BlazorHero.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//inject custom services
var conn = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(conn));
builder.Services.AddScoped<IRepository<CourseModel>, CourseRepo>();

//add CORS policies
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        builder => builder.WithOrigins("https://localhost:5001")
    )
);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors();

//setup custom Api calls
app.MapGet("/api/courses", 
        async (IRepository<CourseModel> repo) => await repo.QueryAll().ToListAsync())
    .WithName("GetCourses");

app.MapGet("/api/courses/{id}", 
        async (IRepository<CourseModel> repo, int id) => await repo.GetAsync(id))
    .WithName("GetCourseById");

app.Run();