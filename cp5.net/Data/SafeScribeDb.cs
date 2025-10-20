using Microsoft.EntityFrameworkCore;
using Cp5.Net.Models;

namespace Cp5.Net.Data;

public class SafeScribeDb : DbContext
{
    public SafeScribeDb(DbContextOptions<SafeScribeDb> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Note> Notes => Set<Note>();
}


