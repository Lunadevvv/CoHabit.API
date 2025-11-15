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
    public DbSet<UserFeedback> UserFeedbacks { get; set; }
    public DbSet<Characteristic> Characteristics { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Furniture> Furnitures { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostImage> PostImages { get; set; }
    public DbSet<PostFeedback> PostFeedbacks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Subcription> Subcriptions { get; set; }
    public DbSet<UserSubcription> UserSubcriptions { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

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
            // entity.HasIndex(e => e.Id, "UQ__User__1788CC4DF7FFBA69").IsUnique(); // Commented: Id is already unique as primary key
            entity.HasIndex(e => e.PhoneNumber, "IX_Users_PhoneNumber").IsUnique();
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ");

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

            entity.HasMany(u => u.FavoritePosts)
                    .WithMany(p => p.LikedByUsers)
                    .UsingEntity<Dictionary<string, object>>(
                    "UserFavoritePost",
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_UserFavoritePost_Post_PostId")
                        .OnDelete(DeleteBehavior.NoAction),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserFavoritePost_User_UserId")
                        .OnDelete(DeleteBehavior.NoAction),
                    j =>
                    {
                        j.HasKey("UserId", "PostId");
                        j.ToTable("UserFavoritePosts");
                    });
        });

        builder.Entity<UserFeedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SenderId).IsRequired();
            entity.Property(e => e.ReceiverId).IsRequired();
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();

            entity.HasOne(e => e.Sender)
                .WithMany(u => u.SentFeedbacks)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Receiver)
                .WithMany(u => u.ReceivedFeedbacks)
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Characteristic>(entity =>
        {
            entity.HasKey(e => e.CharId);
            entity.Property(e => e.CharId).HasMaxLength(5);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ");
            // entity.HasIndex(e => e.CharId, "UQ__Characte__1788CC4D1E3E2C2E").IsUnique(); // Commented: CharId is already unique as primary key
        });

        builder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.OtpId);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ");
            entity.Property(e => e.ExpiredAt).HasColumnType("TIMESTAMPTZ");
            entity.HasIndex(e => e.Phone, "IX_Otps_Phone").IsUnique();
        });

        //Seed data mặc định vào bảng asp.net role
        builder.Entity<IdentityRole<Guid>>()
            .HasData(new List<IdentityRole<Guid>>
            {
                new IdentityRole<Guid>()
                {
                    Id = Guid.Parse("2e11f535-051e-4d86-9ddc-3543c6eabfd4"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<Guid>()
                {
                    Id = Guid.Parse("6a207f7c-9614-4fec-96e8-53cfe31ed8f2"),
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                },

                new IdentityRole<Guid>()
                {
                    Id = Guid.Parse("fc5ac104-d527-4d25-b9c9-358295d54ea4"),
                    Name = "ProMember",
                    NormalizedName = "PROMEMBER"
                },

                new IdentityRole<Guid>()
                {
                    Id = Guid.Parse("e15a9a60-10ae-4f93-acb7-3d38a4cc4125"),
                    Name = "PlusMember",
                    NormalizedName = "PLUSMEMBER"
                },

                new IdentityRole<Guid>()
                {
                    Id = Guid.Parse("3416a9bb-49a6-420f-832f-78b197f57bc2"),
                    Name = "BasicMember",
                    NormalizedName = "BASICMEMBER"
                }
            });

        // Configure AspNetUserRoles delete behavior
        builder.Entity<IdentityUserRole<Guid>>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<IdentityUserRole<Guid>>()
            .HasOne<IdentityRole<Guid>>()
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);
            entity.Property(e => e.PaymentId).HasMaxLength(26);
            entity.Property(e => e.PaymentLinkId).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Price).IsRequired();
            // entity.Property(e => e.Status).HasMaxLength(20).IsRequired(); // Commented: Status is enum, no need to set max length
            entity.Property(e => e.CreatedDate).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.UpdatedDate).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.Description).HasMaxLength(20);
            entity.HasIndex(e => e.PaymentLinkId, "IX_Payments_PaymentLinkId");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Furniture>(entity =>
        {
            entity.HasKey(e => e.FurId);
            entity.Property(e => e.FurId).HasMaxLength(6);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();
        });

        builder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId);
            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Price).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(3000);
            entity.Property(e => e.Condition).HasMaxLength(3000);
            entity.Property(e => e.DepositPolicy).HasMaxLength(3000);
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Furnitures)
                    .WithMany(f => f.Posts)
                    .UsingEntity<Dictionary<string, object>>(
                    "PostFurniture",
                    j => j
                        .HasOne<Furniture>()
                        .WithMany()
                        .HasForeignKey("FurId")
                        .HasConstraintName("FK_PostFurniture_Furniture_FurId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostFurniture_Post_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("PostId", "FurId");
                        j.ToTable("PostFurnitures");
                    });
            
        });

        builder.Entity<PostImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageUrl).HasMaxLength(2048).IsRequired();
            entity.Property(e => e.PostId).IsRequired();

            entity.HasOne(e => e.Post)
                .WithMany(p => p.PostImages)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<PostFeedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PostId).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();

            entity.HasOne(e => e.Post)
                .WithMany(p => p.PostFeedbacks)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.PostFeedbacks)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.PostId).IsRequired();
            entity.Property(e => e.OwnerId).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Post)
                .WithMany(p => p.Orders)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Conversation)
                .WithOne(c => c.Order)
                .HasForeignKey<Order>(e => e.ConversationId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Subcription>(entity =>
        {
            entity.HasKey(e => e.SubcriptionId);
            entity.Property(e => e.SubcriptionId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Price).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DurationInDays).IsRequired();
        });

        builder.Entity<UserSubcription>(entity =>
        {
            entity.HasKey(e => e.UserSubcriptionId);
            entity.Property(e => e.UserSubcriptionId).ValueGeneratedOnAdd();
            entity.Property(e => e.StartDate).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.EndDate).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.IsActive).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserSubcriptions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Subcription)
                .WithMany(u => u.UserSubcriptions)
                .HasForeignKey(e => e.SubcriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PostId).IsRequired();
            entity.Property(e => e.OwnerId).IsRequired();
            entity.Property(e => e.InterestedUserId).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.LastMessageAt).HasColumnType("TIMESTAMPTZ");

            entity.HasOne(e => e.Post)
                .WithMany(p => p.Conversations)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Owner)
                .WithMany(u => u.OwnedConversations)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InterestedUser)
                .WithMany(u => u.InterestedConversations)
                .HasForeignKey(e => e.InterestedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ConversationId).IsRequired();
            entity.Property(e => e.SenderId).IsRequired();
            entity.Property(e => e.Content).HasMaxLength(2000).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMPTZ").IsRequired();
            entity.Property(e => e.IsRead).IsRequired();

            entity.HasOne(e => e.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
