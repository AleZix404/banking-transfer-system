using banking_transfer_system.EF.DTOs;
using banking_transfer_system.EF.Entities;

namespace banking_transfer_system.SingleClass
{
    public class TransferData
    {
        public AccountTransferRequestDto TransferRequest { get; set; }
        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
    }
}
