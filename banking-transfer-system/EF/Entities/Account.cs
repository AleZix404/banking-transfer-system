using System.ComponentModel.DataAnnotations.Schema;

namespace banking_transfer_system.EF.Entities
{
    [Table("accounts")]
    public class Account
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("account_number")]
        public string AccountNumber { get; set; }
        [Column("account_holder")]
        public string AccountHolder { get; set; }
        [Column("balance")]
        public decimal Balance { get; set; }

        public ICollection<Transfer> OutgoingTransfers { get; set; }
        public ICollection<Transfer> IncomingTransfers { get; set; }
    }

}
