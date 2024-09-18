using System.ComponentModel.DataAnnotations.Schema;

namespace banking_transfer_system.EF.DTOs
{
    public class TransferDTO
    {
        public int SourceAccountId { get; set; }
        public int DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
