using RefactorThis.DB.Repository;
using RefactorThis.DB.Entity;

namespace DB.Repository
{
    public class ProductOptionRepository : GenericRepository<ProductOption>, IProductOptionRepository
    { 
        public ProductOptionRepository(ProductsContext context): base(context)
        {

        }
    }
}
