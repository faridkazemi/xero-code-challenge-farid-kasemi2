using RefactorThis.DB.Repository;
using RefactorThis.DB.Entity;

namespace DB.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    { 
        public ProductRepository(ProductsContext context): base(context)
        {

        }
    }
}
