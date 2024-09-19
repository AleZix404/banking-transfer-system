using System.ComponentModel.DataAnnotations.Schema;

namespace banking_transfer_system.EF.Entities
{
    [Table("transfers")]
    public class Transfer
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("source_account_id")]
        public int SourceAccountId { get; set; }
        [Column("destination_account_id")]
        public int DestinationAccountId { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("transfer_date")]
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;

        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
    }

}
