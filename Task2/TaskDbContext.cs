using Microsoft.EntityFrameworkCore;
using Task2;
using Task2.Models;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> contextOptions) : base(contextOptions) { }

    public DbSet<Todo> TodoTable { get; set; }

    public DbSet<User> Users { get; set; }
    
}

