namespace Licencias.Application.Entities.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        Task<int> SaveChangesAsync();
        void ClearContext();
    }
}
