using DB.Entity;
using Microsoft.EntityFrameworkCore;
using RefactorThis.DB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DB.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseEntity
    {
        internal ProductsContext Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(ProductsContext context)
        {
            this.Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;
            try
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
                return query.ToList();
            }
            catch (Exception e)
            {
                //TODO
                //Logging the exception
                throw (e);
            }


        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var a = DbSet.ToList();

            return a;
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual Guid Insert(TEntity entity)
        {
            try
            {
                DbSet.Add(entity);
                return entity.Id;
            }
            catch (Exception e)
            {

                //TODO
                //Logging the exception
                throw (e);
            }
        }

        public virtual void Delete(object id)
        {
            try
            {
                TEntity entityToDelete = DbSet.Find(id);
                DbSet.Remove(entityToDelete);
            }
            catch (Exception e)
            {
                //TODO
                //Logging the exception
                throw (e);
            }

        }
        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void DetachState(TEntity entity)
        {
            Context.Entry<TEntity>(entity).State = EntityState.Detached;

        }

    }
}
