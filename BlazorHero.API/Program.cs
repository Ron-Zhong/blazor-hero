using BlazorHero.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(conn), ServiceLifetime.Scoped);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/courses", async (DBContext context) => 
        await context.Courses
                        .Where(x => !x.IsDeleted)
                        .Take(20)
                        .AsNoTracking()
                        .ToListAsync())
    .WithName("Courses");


app.MapGet("/courses/{id}", async (DBContext context, int id) =>
        await context.Courses
                        .Where(x => x.CourseNo == id)
                        .AsNoTracking()
                        .FirstOrDefaultAsync())
    .WithName("CourseById");

app.Run();