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
        // TODO get connection string from config file
        var connectionString = "Host=localhost;Port=5432;Database=task_tracker;Username=postgres;Password=postgres";
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTask>(task =>
        {
            task.Property(t => t.Id).UseSerialColumn();
            task.Property(t => t.Title).HasColumnType("varchar").HasMaxLength(1000);
            task.Property(t => t.Description).HasColumnType("varchar").HasMaxLength(100000);

            task.Property(t => t.CompletionDate).HasColumnType("timestamp with time zone");
            task.Property(t => t.DueDate).HasColumnType("timestamp with time zone");
            task.Property(t => t.DeletionDate).HasColumnType("timestamp with time zone");
        });

        modelBuilder.Entity<Folder>(folder =>
        {
            folder.Property(f => f.Id).UseSerialColumn();
            folder.Property(f => f.Title).HasColumnType("varchar").HasMaxLength(100);

            folder.Ignore(f => f.CompletedTasks);
            folder.Ignore(f => f.IncompleteTasks);

            folder.HasMany(f => f.Tasks).WithOne();
        });
    }
}
