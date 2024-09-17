namespace banking_transfer_system.EF.Entities
{
    public class Transfer
    {
        public int Id { get; set; }
        public int SourceAccountId { get; set; }
        public int DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }

        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
    }

}
