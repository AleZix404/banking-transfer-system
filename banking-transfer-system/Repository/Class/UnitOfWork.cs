using banking_transfer_system.EF.Datas;
using banking_transfer_system.Repository.Interfaces;

namespace banking_transfer_system.Repository.Class
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankTransferContext _context;

        public UnitOfWork(BankTransferContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
