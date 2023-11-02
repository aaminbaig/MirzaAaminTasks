using Microsoft.EntityFrameworkCore;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> contextOptions) : base(contextOptions) { }

    public DbSet<Todo> TodoTable { get; set; }
}

