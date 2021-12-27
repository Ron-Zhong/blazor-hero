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

    public virtual DbSet<Accreditation> Accreditations { get; set; } = null!;
    public virtual DbSet<Admin> Admins { get; set; } = null!;
    public virtual DbSet<Certificate> Certificates { get; set; } = null!;
    public virtual DbSet<CommonCode> CommonCodes { get; set; } = null!;
    public virtual DbSet<Course> Courses { get; set; } = null!;
    public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
    public virtual DbSet<PreviewAccount> PreviewAccounts { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;database=devmedudb;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accreditation>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasIndex(e => e.Email, "AK_Admins_Email")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<CommonCode>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("CommonCode");

            entity.Property(e => e.CodeKey).HasMaxLength(100);

            entity.Property(e => e.CodeType).HasMaxLength(100);

            entity.Property(e => e.CodeValue).HasMaxLength(100);

            entity.Property(e => e.EndDate).HasColumnType("datetime");

            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.CourseHash, "AK_Courses_CourseHash")
                .IsUnique();

            entity.HasIndex(e => e.CourseNo, "AK_Courses_CourseNo")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.AttributionMode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CourseHash).HasDefaultValueSql("(N'')");

            entity.Property(e => e.CourseNo).ValueGeneratedOnAdd();

            entity.Property(e => e.Cpdpoints).HasColumnName("CPDPoints");

            entity.Property(e => e.HavingCertificate)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");

            entity.Property(e => e.HavingInterstitialAds)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");

            entity.Property(e => e.IsLimitedSeats)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");

            entity.HasOne(d => d.Certificate)
                .WithMany(p => p.Courses)
                .HasForeignKey(d => d.CertificateId);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.IsSubmitted)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");

            entity.HasOne(d => d.CourseNoNavigation)
                .WithMany(p => p.Enrollments)
                .HasPrincipalKey(p => p.CourseNo)
                .HasForeignKey(d => d.CourseNo);

            entity.HasOne(d => d.Sso)
                .WithMany(p => p.Enrollments)
                .HasPrincipalKey(p => p.SsoId)
                .HasForeignKey(d => d.SsoId);
        });

        modelBuilder.Entity<PreviewAccount>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "AK_Users_Email")
                .IsUnique();

            entity.HasIndex(e => e.SsoId, "AK_Users_SsoId")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Email).HasDefaultValueSql("(N'')");

            entity.Property(e => e.PracticeType)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.WorkplaceType)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
