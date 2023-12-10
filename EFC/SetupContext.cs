using Domain;
using Microsoft.EntityFrameworkCore;

namespace EFC;

public class SetupContext : DbContext
{
    public DbSet<Data> Datas { get; set; }
    public DbSet<CompleteData> CompleteDatas { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = C:/Users/madsl/Downloads/SEP (3)/SEP/EFC/Fiberline.db");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Data>().HasKey(data => data.BatchId);
        modelBuilder.Entity<CompleteData>().HasKey(completeData => completeData.BatchId);
        modelBuilder.Entity<User>().HasKey(user => user.UserId);
    }
}