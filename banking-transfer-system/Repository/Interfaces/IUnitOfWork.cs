namespace banking_transfer_system.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync();
    }
}
