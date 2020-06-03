using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Entity;

namespace DataMigration
{
    public class DataMigrationContext : DbContext
    {
        public DataMigrationContext(DbContextOptions<DataMigrationContext> options
            ) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:khoapiterrr99.database.windows.net,1433;Initial Catalog=weatherforcast;Persist Security Info=False;User ID=khoapiterrr;Password=linhcutehotmexinchao9(;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        public DbSet<MesBotEntity> MesBotEntities { get; set; }
        public DbSet<UserEntiry> UserEntiries { get; set; }
        public DbSet<TextRandomEntity> TextRandomEntities { get; set; }
    }
}