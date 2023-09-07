using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class MyDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Contract> Contracts { get; set; }

    public string DbPath => "employees.db";

    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        :base(options)  {
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}
