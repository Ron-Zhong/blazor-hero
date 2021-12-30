using Microsoft.EntityFrameworkCore;

namespace BlazorHero.Database;

public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Course> Courses { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;database=demo;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.CourseHash, "AK_Courses_CourseHash")
                .IsUnique();

            entity.HasIndex(e => e.CourseNo, "AK_Courses_CourseNo")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CourseHash).HasDefaultValueSql("(N'')");

            entity.Property(e => e.CourseNo).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
