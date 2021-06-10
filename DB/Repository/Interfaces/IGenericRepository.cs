using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RefactorThis.DB.Repository
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter, string includeProperties);

        IEnumerable<T> GetAll();
        T GetById(object id);
        Guid Insert(T entity);
        void Delete(object id);
        void Update(T entityToUpdate);

        void DetachState(T entity);

    }
}
