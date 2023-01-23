using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class ApplicationContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<DoctorModel> Doctors { get; set; }
    public DbSet<ReceptionModel> Receptions { get; set; }
    public DbSet<ScheduleModel> Schedules { get; set; }
    public DbSet<ProfileModel> Profiles { get; set; }
    
    public ApplicationContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserModel>().HasIndex(model => model.Username);
    }
}