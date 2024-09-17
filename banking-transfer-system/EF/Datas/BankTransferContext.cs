using banking_transfer_system.EF.Entities;
using Microsoft.EntityFrameworkCore;
namespace banking_transfer_system.EF.Datas
{
    public class BankTransferContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        public BankTransferContext(DbContextOptions<BankTransferContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuracion de las relaciones
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.SourceAccount)
                .WithMany(a => a.OutgoingTransfers)
                .HasForeignKey(t => t.SourceAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.DestinationAccount)
                .WithMany(a => a.IncomingTransfers)
                .HasForeignKey(t => t.DestinationAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
