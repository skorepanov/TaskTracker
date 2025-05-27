using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Bll.Models;

namespace TaskTracker.Dal;

public class ApplicationContext : DbContext
{
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public ApplicationContext() { }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTask>(ConfigureUserTask);
        modelBuilder.Entity<Folder>(ConfigureFolder);
        modelBuilder.Entity<Tag>(ConfigureTag);
    }

    private void ConfigureUserTask(EntityTypeBuilder<UserTask> builder)
    {
        builder.Property(t => t.Id)
            .UseSerialColumn();

        builder.Property(t => t.Title)
            .HasColumnType("varchar")
            .HasMaxLength(1000);

        builder.Property(t => t.Description)
            .HasColumnType("varchar")
            .HasMaxLength(100000);

        builder.Property(t => t.CompletedDateTime)
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.DueDateTime)
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.MovedToTrashDateTime)
            .HasColumnType("timestamp with time zone");

        builder.HasOne(t => t.Folder)
            .WithMany(f => f.Tasks)
            .HasForeignKey(t => t.FolderId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureFolder(EntityTypeBuilder<Folder> builder)
    {
        builder.Property(f => f.Id)
            .UseSerialColumn();

        builder.Property(f => f.Title)
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Ignore(f => f.CompletedTasks);

        builder.Ignore(f => f.IncompleteTasks);
    }

    private void ConfigureTag(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(t => t.Id)
            .UseSerialColumn();

        builder.Property(t => t.Title)
            .HasColumnType("varchar")
            .HasMaxLength(100);
    }
}
