using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb.Database {
    public class ArchiveContext : DbContext {
        public ArchiveContext(string connectionString) : base(getOptions(connectionString)) { }

        public ArchiveContext(DbContextOptions<ArchiveContext> options) : base(options) { }

        private static DbContextOptions getOptions(string connectionString) =>
            SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Payee> Payees { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionSplit> TransactionSplits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            var account = modelBuilder.Entity<Account>();
            var transaction = modelBuilder.Entity<Transaction>();

            account.HasMany(a => a.Transactions).WithOne(t => t.Account).HasForeignKey(t => t.AccountId);
            account.HasMany(a => a.TransferTransactions).WithOne(t => t.TransferAccount).HasForeignKey(a => a.TransferAccountId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseLazyLoadingProxies();
    }
}
