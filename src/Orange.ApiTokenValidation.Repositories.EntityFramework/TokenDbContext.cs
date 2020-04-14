using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Repositories.EntityFramework.Models;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework
{
    class TokenDbContext : DbContext
    {
        private readonly IOptions<ConnectionStrings> _connectionSettings;

        public TokenDbContext(IOptions<ConnectionStrings> connectionSettings,
                              IOptions<CommonConfiguration> commonConfiguration)
        {
            _connectionSettings = connectionSettings;
            InstanceId = commonConfiguration.Value.InstanceId;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<TokenDbModel>();
            entityTypeBuilder.ToTable("tokentable");
            entityTypeBuilder.Property(x => x.Issuer).HasColumnName("issuer").IsRequired();
            entityTypeBuilder.Property(x => x.Audience).HasColumnName("audience").IsRequired();
            entityTypeBuilder.Property(x => x.PrivateKey).HasColumnName("private_key").IsRequired();
            entityTypeBuilder.Property(x => x.Ttl).HasColumnName("ttl").IsRequired();
            entityTypeBuilder.Property(x => x.ExpirationDate).HasColumnName("expiration_time").IsRequired();
            entityTypeBuilder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
            entityTypeBuilder.Property(x => x.PayLoad).HasColumnName("payload").HasColumnType("jsonb");
            entityTypeBuilder.Property(x => x.CreatedTime).HasColumnName("created_time").IsRequired();
            entityTypeBuilder.Property(x => x.Creator).HasColumnName("creator").IsRequired();
            entityTypeBuilder.Property(x => x.UpdatedTime).HasColumnName("updated_time").IsRequired();
            entityTypeBuilder.Property(x => x.Updater).HasColumnName("updater").IsRequired();

            entityTypeBuilder.HasKey(c => new { issuer = c.Issuer, audience = c.Audience });


            var versionInfoTypeBuilder = modelBuilder.Entity<VersionInfo>();
            versionInfoTypeBuilder.ToTable("VersionInfo");
            versionInfoTypeBuilder.Property(x => x.Version).HasColumnName("Version");
            versionInfoTypeBuilder.Property(x => x.AppliedOn).HasColumnName("AppliedOn");
            versionInfoTypeBuilder.Property(x => x.Description).HasColumnName("Description");
            versionInfoTypeBuilder.HasKey(x => x.Version);
        }

        public DbSet<TokenDbModel> Tokens { get; set; }

        public DbSet<VersionInfo> Versions { get; set; }

        public string InstanceId { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionSettings.Value.DefaultConnection);
        }
    }
}
