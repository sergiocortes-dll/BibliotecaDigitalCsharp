using BibliotecaDigital.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDigital.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Lean> Leans { get; set; }
}