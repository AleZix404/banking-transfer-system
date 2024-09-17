namespace banking_transfer_system.EF.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public decimal Balance { get; set; }

        public ICollection<Transfer> OutgoingTransfers { get; set; }
        public ICollection<Transfer> IncomingTransfers { get; set; }
    }

}
