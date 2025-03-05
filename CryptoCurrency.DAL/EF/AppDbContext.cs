using CryptoCurrency.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptoCurrency.DAL.EF
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Crypto> Crypto { get; set; }
        public DbSet<PriceHistory> PriceHistorries { get; set; }
        public DbSet<UserCrypto> UserCryptos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<UserCrypto>()
                .HasOne(uc => uc.AppUser)
                .WithMany(u => u.Wallet)
                .HasForeignKey(uc => uc.AppUserId);

            builder.Entity<UserCrypto>()
                .HasOne(uc => uc.Crypto)
                .WithMany()
                .HasForeignKey(uc => uc.CryptoSymbol);

            builder.Entity<PriceHistory>()
                .HasOne(p => p.Crypto)
                .WithMany(c => c.PriceHistory)
                .HasForeignKey(p => p.CryptoSymbol)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
