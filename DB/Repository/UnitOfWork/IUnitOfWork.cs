using RefactorThis.DB.Repository;

namespace DB.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Save();
        IProductRepository ProductRepository { get; }
        IProductOptionRepository ProductOptionRepository { get; }  
    }
}
