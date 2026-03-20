using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QLPhongGym.Data;

namespace QLPhongGym.Data
{
    public class GymDbContextFactory : IDesignTimeDbContextFactory<GymDbContext>
    {
        public GymDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=dpg-d6ueucma2pns739b2hlg-a.oregon-postgres.render.com;Port=5432;Database=qlphonggym;Username=qlphonggym_user;Password=88HABANLEwK0eODFy2pgHQyORmbifNCR;SSL Mode=Require;Trust Server Certificate=true");

            return new GymDbContext(optionsBuilder.Options);
        }
    }
}
