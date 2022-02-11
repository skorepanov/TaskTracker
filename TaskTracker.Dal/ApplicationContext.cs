using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;

namespace TaskTracker.Dal;

public class ApplicationContext : DbContext
{
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<Folder> Folders { get; set; }

    public ApplicationContext() { }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO get db name from config file
        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var sqlitePath = Path.Combine(folderPath, @"TaskTracker.db");

        optionsBuilder.UseSqlite($"Data source={sqlitePath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTask>(task =>
        {
            task.Property(t => t.Title).HasColumnType("nvarchar").HasMaxLength(1000);
            task.Property(t => t.Description).HasColumnType("nvarchar").HasMaxLength(100000);

            task.Property(t => t.CompletionDate).HasColumnType("datetime");
            task.Property(t => t.DueDate).HasColumnType("datetime");
            task.Property(t => t.DeletionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Folder>(folder =>
        {
            folder.Property(f => f.Title).HasColumnType("nvarchar").HasMaxLength(100);

            folder.Ignore(f => f.CompletedTasks);
            folder.Ignore(f => f.IncompleteTasks);

            folder.HasMany(f => f.Tasks).WithOne();
        });
    }
}
