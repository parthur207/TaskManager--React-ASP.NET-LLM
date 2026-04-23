using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;

namespace TaskManager.Adapters.Persistence
{
    public sealed class DbContextTaskManager : DbContext
    {
        public DbContextTaskManager(DbContextOptions<DbContextTaskManager> options) : base(options) { }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<TaskEntity> Task { get; set; }
        public DbSet<TaskCategoryEntity> TaskCategory { get; set; }
        public DbSet<SpaceEntity> Space { get; set; }
        public DbSet<SpaceMemberEntity> SpaceMember { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region UserEntity
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .ValueGeneratedNever();

                entity.Property(u => u.Name)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Value)
                         .HasColumnName("Email")
                         .HasMaxLength(200)
                         .IsRequired();
                });

                entity.OwnsOne(u => u.PasswordHash, pwd =>
                {
                    pwd.Property(p => p.Value)
                       .HasColumnName("PasswordHash")
                       .HasMaxLength(500)
                       .IsRequired();
                });

                entity.Property(u => u.Role)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(u => u.Status)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(u => u.CreatedAt)
                      .IsRequired();

                entity.Property(u => u.UpdatedDate)
                      .IsRequired(false);
            });
            #endregion

            #region SpaceEntity
            modelBuilder.Entity<SpaceEntity>(entity =>
            {
                entity.ToTable("Spaces");

                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id)
                      .ValueGeneratedNever();

                entity.Property(s => s.Name)
                      .HasMaxLength(60)
                      .IsRequired();

                entity.Property(s => s.OwnerId)
                      .IsRequired();

                entity.HasOne(s => s.Owner)
                      .WithMany()
                      .HasForeignKey(s => s.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region SpaceMemberEntity
            modelBuilder.Entity<SpaceMemberEntity>(entity =>
            {
                entity.ToTable("SpaceMembers");

                entity.HasKey(sm => new { sm.SpaceId, sm.UserId });

                entity.Property(sm => sm.JoinedAt)
                      .IsRequired();

                entity.Property(sm => sm.IsAdmin)
                      .IsRequired()
                      .HasDefaultValue(false);

                entity.HasOne(sm => sm.Space)
                      .WithMany(s => s.Members)
                      .HasForeignKey(sm => sm.SpaceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sm => sm.User)
                      .WithMany(u => u.Spaces)
                      .HasForeignKey(sm => sm.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region TaskCategoryEntity
            modelBuilder.Entity<TaskCategoryEntity>(entity =>
            {
                entity.ToTable("TaskCategories");

                entity.HasKey(tc => tc.Id);

                entity.Property(tc => tc.Id)
                      .ValueGeneratedNever();

                entity.Property(tc => tc.Name)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(tc => tc.CreatedAt)
                      .IsRequired();

                entity.Property(tc => tc.UpdatedAt)
                      .IsRequired(false);

                entity.HasOne(tc => tc.User)
                      .WithMany()
                      .HasForeignKey(tc => tc.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region TaskEntity
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.ToTable("Tasks");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Id)
                      .ValueGeneratedNever();

                entity.Property(t => t.Title)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(t => t.Description)
                      .HasMaxLength(1000)
                      .IsRequired(false);

                entity.Property(t => t.StatusEnum)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(t => t.Term)
                      .IsRequired();

                entity.Property(t => t.CreatedAt)
                      .IsRequired();

                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);

                entity.HasOne(t => t.OwnerUser)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(t => t.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ResponsibleUser)
                      .WithMany()
                      .HasForeignKey(t => t.ResponsibleUserId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Category)
                      .WithMany(x => x.Tasks)
                      .HasForeignKey(t => t.CategoryId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Space)
                      .WithMany(s => s.Tasks)
                      .HasForeignKey(t => t.SpaceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
    }
