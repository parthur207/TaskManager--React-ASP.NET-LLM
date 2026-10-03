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
        public DbSet<TaskChildrenEntity> TaskChildren { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region UserEntity
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedNever();

                entity.Property(u => u.Name)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(u => u.Role)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(u => u.Status)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(u => u.CreatedAt).IsRequired();

                entity.Property(u => u.UpdatedDate)
                      .IsRequired(false);

                entity.Property(u => u.TwoFactores)
                      .IsRequired()
                      .HasDefaultValue(false);

                entity.OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Value)
                         .HasColumnName("Email")
                         .HasMaxLength(200)
                         .IsRequired();

                    email.HasIndex(e => e.Value)
                         .IsUnique();
                });

                entity.OwnsOne(u => u.PasswordHash, pwd =>
                {
                    pwd.Property(p => p.Value)
                       .HasColumnName("PasswordHash")
                       .HasMaxLength(500)
                       .IsRequired();
                });

                entity.HasMany(u => u.Tasks)
                      .WithOne(t => t.OwnerUser)
                      .HasForeignKey(t => t.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Spaces)
                      .WithOne(sm => sm.User)
                      .HasForeignKey(sm => sm.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region SpaceEntity
            modelBuilder.Entity<SpaceEntity>(entity =>
            {
                entity.ToTable("Spaces");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedNever();

                entity.Property(s => s.Name)
                      .HasMaxLength(60)
                      .IsRequired();

                entity.Property(s => s.OwnerId).IsRequired();

                entity.Property(s => s.CreatedAt).IsRequired();

                entity.Property(s => s.UpdatedAt).IsRequired(false);

                entity.HasOne(s => s.Owner)
                      .WithMany()
                      .HasForeignKey(s => s.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Members)
                      .WithOne(sm => sm.Space)
                      .HasForeignKey(sm => sm.SpaceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.Tasks)
                      .WithOne(t => t.Space)
                      .HasForeignKey(t => t.SpaceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.TaskCategories)
                      .WithOne(tc => tc.Space)
                      .HasForeignKey(tc => tc.SpaceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region SpaceMemberEntity
            modelBuilder.Entity<SpaceMemberEntity>(entity =>
            {
                entity.ToTable("SpaceMembers");
                entity.HasKey(sm => new { sm.SpaceId, sm.UserId });

                entity.Property(sm => sm.JoinedAt).IsRequired();

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

                entity.HasIndex(sm => sm.SpaceId);
                entity.HasIndex(sm => sm.UserId);
            });
            #endregion

            #region TaskCategoryEntity
            modelBuilder.Entity<TaskCategoryEntity>(entity =>
            {
                entity.ToTable("TaskCategories");
                entity.HasKey(tc => tc.Id);
                entity.Property(tc => tc.Id).ValueGeneratedNever();

                entity.Property(tc => tc.Name)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(tc => tc.CreatedAt).IsRequired();

                entity.Property(tc => tc.UpdatedAt).IsRequired(false);

                entity.Property(tc => tc.OwnerId).IsRequired();

                entity.Property(tc => tc.SpaceId).IsRequired();

                entity.HasOne(tc => tc.UserOwner)
                      .WithMany()
                      .HasForeignKey(tc => tc.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tc => tc.Space)
                      .WithMany(s => s.TaskCategories)
                      .HasForeignKey(tc => tc.SpaceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(tc => tc.Tasks)
                      .WithOne(t => t.Category)
                      .HasForeignKey(t => t.CategoryId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasIndex(tc => tc.SpaceId);
                entity.HasIndex(tc => tc.OwnerId);
            });
            #endregion

            #region TaskEntity
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.ToTable("Tasks");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedNever();

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

                entity.Property(t => t.Term).IsRequired();

                entity.Property(t => t.CreatedAt).IsRequired();

                entity.Property(t => t.UpdatedAt).IsRequired(false);

                entity.Property(t => t.OwnerId).IsRequired();

                entity.Property(t => t.SpaceId).IsRequired();

                entity.Property(t => t.ResponsibleUserId).IsRequired(false);

                entity.Property(t => t.CategoryId).IsRequired(false);

                entity.Property(t => t.PrioritySort)
                      .IsRequired()
                      .HasDefaultValue(0);

                entity.Property(t => t.JustifyPriority)
                      .HasMaxLength(500)
                      .IsRequired(false);

                entity.Property(t => t.ExecutionPlan)
                      .HasMaxLength(2000)
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
                      .WithMany(tc => tc.Tasks)
                      .HasForeignKey(t => t.CategoryId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(t => t.Space)
                      .WithMany(s => s.Tasks)
                      .HasForeignKey(t => t.SpaceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.ChildTasks)
                      .WithOne(tc => tc.ParentTask)
                      .HasForeignKey(tc => tc.ParentTaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(t => t.SpaceId);
                entity.HasIndex(t => t.OwnerId);
                entity.HasIndex(t => t.ResponsibleUserId);
                entity.HasIndex(t => t.CategoryId);
            });
            #endregion

            #region TaskChildrenEntity
            modelBuilder.Entity<TaskChildrenEntity>(entity =>
            {
                entity.ToTable("TaskChildren");
                entity.HasKey(tc => tc.Id);
                entity.Property(tc => tc.Id).ValueGeneratedNever();

                entity.Property(tc => tc.Title)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(tc => tc.Description)
                      .HasMaxLength(1000)
                      .IsRequired(false);

                entity.Property(tc => tc.StatusEnum)
                      .HasConversion<string>()
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(tc => tc.Term).IsRequired();

                entity.Property(tc => tc.ParentTaskId).IsRequired();

                entity.HasOne(tc => tc.ParentTask)
                      .WithMany(t => t.ChildTasks)
                      .HasForeignKey(tc => tc.ParentTaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(tc => tc.ParentTaskId);
            });
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}

#region migrations (path e command)
//    dotnet ef migrations add NomeDaMigracao `
//  --project TaskManager.Adapters\TaskManager.Adapters.csproj `
//  --startup-project TaskManager.API\TaskManager.API.csproj

//    dotnet ef database update `
//  --project TaskManager.Adapters\TaskManager.Adapters.csproj `
//  --startup-project TaskManager.API\TaskManager.API.csproj
#endregion