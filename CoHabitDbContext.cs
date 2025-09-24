using CoHabit.API.Enitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API;
public class CoHabitDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public CoHabitDbContext(DbContextOptions<CoHabitDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Characteristic> Characteristics { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)

    {
        base.OnModelCreating(builder);

        //quy định độ dài của các trường trong bảng User
        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(u => u.FirstName).HasMaxLength(10);
            entity.Property(u => u.LastName).HasMaxLength(10);
            entity.Property(u => u.PhoneNumber).HasMaxLength(10);
            entity.Property(u => u.Image).HasMaxLength(2048);
            entity.HasIndex(e => e.Id, "UQ__User__1788CC4DF7FFBA69").IsUnique();
            entity.HasIndex(e => e.PhoneNumber, "UQ__User__536C85E43394AC26").IsUnique();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasMany(u => u.Characteristics)
                    .WithMany(c => c.Users)
                    .UsingEntity<Dictionary<string, object>>(
                    "UserCharacteristic",
                    j => j
                        .HasOne<Characteristic>()
                        .WithMany()
                        .HasForeignKey("CharId")
                        .HasConstraintName("FK_UserCharacteristic_Characteristic_CharId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserCharacteristic_User_UserId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("UserId", "CharId");
                        j.ToTable("UserCharacteristics");
                    });
        });

        builder.Entity<Characteristic>(entity =>
        {
            entity.HasKey(e => e.CharId);
            entity.Property(e => e.CharId).HasMaxLength(5);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.HasIndex(e => e.CharId, "UQ__Characte__1788CC4D1E3E2C2E").IsUnique();
        });

        builder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.OtpId);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.HasIndex(e => e.Phone, "UQ__Otp__C8EE201F536C85E4").IsUnique();
        });

        //Seed data mặc định vào bảng asp.net role
        builder.Entity<IdentityRole<Guid>>()
            .HasData(new List<IdentityRole<Guid>>
            {
                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                },

                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = "ProMember",
                    NormalizedName = "PROMEMBER"
                },

                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = "PlusMember",
                    NormalizedName = "PLUSMEMBER"
                },

                new IdentityRole<Guid>()
                {
                    Id = Guid.NewGuid(),
                    Name = "BasicMember",
                    NormalizedName = "BASICMEMBER"
                }
            });

        builder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);
            entity.Property(e => e.PaymentId).HasMaxLength(6);
            entity.Property(e => e.Price).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.UserId).HasMaxLength(40).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
