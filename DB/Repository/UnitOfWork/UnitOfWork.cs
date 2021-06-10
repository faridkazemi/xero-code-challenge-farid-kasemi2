using RefactorThis.DB.Repository;
using System;

namespace DB.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        ProductsContext _context;
        IProductRepository _productRepository;
        IProductOptionRepository _productOptionRepository;
        private bool _disposed = false;
        public UnitOfWork(ProductsContext dbContext)
        {
            _context = dbContext;

        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }

        public IProductOptionRepository ProductOptionRepository
        {
            get
            {
                if (_productOptionRepository == null)
                    _productOptionRepository = new ProductOptionRepository(_context);
                return _productOptionRepository;
            }
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                // TODO A propare way of logging need to be implemented. Mammad joon call me to discuss about it
                Console.WriteLine(e);
                throw;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
