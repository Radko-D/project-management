using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models.Entities;

namespace ProjectManagement.Data
{
    public class ProjectManagementContext : DbContext
    {
        public ProjectManagementContext(DbContextOptions<ProjectManagementContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite keys
            modelBuilder.Entity<ProjectTeam>()
                .HasKey(pt => new { pt.ProjectId, pt.TeamId });

            modelBuilder.Entity<TaskTag>()
                .HasKey(tt => new { tt.TaskId, tt.TagId });

            // Configure relationships
            modelBuilder.Entity<ProjectTeam>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTeams)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectTeam>()
                .HasOne(pt => pt.Team)
                .WithMany(t => t.ProjectTeams)
                .HasForeignKey(pt => pt.TeamId);

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId);

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId);

            // Configure cascading deletes
            modelBuilder.Entity<Board>()
                .HasOne(b => b.Project)
                .WithMany(p => p.Boards)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Milestone)
                .WithMany(m => m.Tasks)
                .HasForeignKey(t => t.MilestoneId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Milestone>()
                .HasOne(m => m.Project)
                .WithMany(p => p.Milestones)
                .HasForeignKey(m => m.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Timeline>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Timelines)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TimelineEvent>()
                .HasOne(te => te.Timeline)
                .WithMany(t => t.Events)
                .HasForeignKey(te => te.TimelineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.Assignments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.TeamMember)
                .WithMany(tm => tm.TaskAssignments)
                .HasForeignKey(ta => ta.TeamMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // SQLite-specific configurations for DateTime
            modelBuilder.Entity<Project>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<Project>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<Project>()
                .Property(p => p.StartDate)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Project>()
                .Property(p => p.EndDate)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Board>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<Team>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<TeamMember>()
                .Property(tm => tm.JoinedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<ProjectTeam>()
                .Property(pt => pt.AssignedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<Milestone>()
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<Milestone>()
                .Property(m => m.DueDate)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Timeline>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<Timeline>()
                .Property(t => t.StartDate)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Timeline>()
                .Property(t => t.EndDate)
                .HasColumnType("TEXT");

            modelBuilder.Entity<TimelineEvent>()
                .Property(te => te.EventDate)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Tag>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.UpdatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.DueDate)
                .HasColumnType("TEXT");

            modelBuilder.Entity<TaskAssignment>()
                .Property(ta => ta.AssignedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            modelBuilder.Entity<Comment>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("datetime('now')")
                .HasColumnType("TEXT");

            // Configure string lengths for SQLite
            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.Description)
                .HasMaxLength(2000);

            modelBuilder.Entity<Board>()
                .Property(b => b.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Board>()
                .Property(b => b.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Team>()
                .Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .Property(t => t.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<TeamMember>()
                .Property(tm => tm.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<TeamMember>()
                .Property(tm => tm.Email)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<TeamMember>()
                .Property(tm => tm.Role)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Milestone>()
                .Property(m => m.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Milestone>()
                .Property(m => m.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Timeline>()
                .Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Timeline>()
                .Property(t => t.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<TimelineEvent>()
                .Property(te => te.Title)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<TimelineEvent>()
                .Property(te => te.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Tag>()
                .Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Tag>()
                .Property(t => t.Color)
                .HasMaxLength(7)
                .IsRequired()
                .HasDefaultValue("#000000");

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Description)
                .HasMaxLength(2000);

            modelBuilder.Entity<Comment>()
                .Property(c => c.Content)
                .HasMaxLength(2000)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(c => c.AuthorName)
                .HasMaxLength(100)
                .IsRequired();

            // Configure enums as strings for SQLite
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Priority)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Create unique indexes
            modelBuilder.Entity<TeamMember>()
                .HasIndex(tm => tm.Email)
                .IsUnique();

            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // Seed data
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "Bug", Color = "#FF0000", CreatedAt = DateTime.UtcNow },
                new Tag { Id = 2, Name = "Feature", Color = "#00FF00", CreatedAt = DateTime.UtcNow },
                new Tag { Id = 3, Name = "Enhancement", Color = "#0000FF", CreatedAt = DateTime.UtcNow },
                new Tag { Id = 4, Name = "Documentation", Color = "#FFA500", CreatedAt = DateTime.UtcNow },
                new Tag { Id = 5, Name = "Testing", Color = "#800080", CreatedAt = DateTime.UtcNow }
            );

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Project project)
                {
                    if (entry.State == EntityState.Added)
                    {
                        project.CreatedAt = DateTime.UtcNow;
                    }
                    project.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is TaskItem task)
                {
                    if (entry.State == EntityState.Added)
                    {
                        task.CreatedAt = DateTime.UtcNow;
                    }
                    task.UpdatedAt = DateTime.UtcNow;
                }
                // Add other entities that have timestamps as needed
            }
        }
    }
}