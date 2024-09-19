namespace banking_transfer_system.EF.DTOs
{
    public class AccountTransferRequestDto
    {
        public string SourceAccountNumber { get; set; }
        public string DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
