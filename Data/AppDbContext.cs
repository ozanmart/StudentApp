using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

namespace StudentApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    public DbSet<Student> Students { get; set; } = null!;
}