using Microsoft.EntityFrameworkCore;
using BlazorHero.Database;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(conn));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("/courses", async (DBContext context) =>
        await context.Courses
                        .Where(x => !x.IsDeleted)
                        .Take(20)
                        .AsNoTracking()
                        .ToListAsync())
    .WithName("GetCourses");


app.MapGet("/courses/{id}", async (DBContext context, int id) =>
        await context.Courses
                        .Where(x => x.CourseNo == id)
                        .AsNoTracking()
                        .FirstOrDefaultAsync())
    .WithName("GetCourseById");

app.Run();