using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFool.Repository.Interfaces;
using EventFool.Domain;
using System.Data.Entity;

namespace EventFool.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class 
    {
        protected readonly Eventfool _dbContext; 

        public GenericRepository(Eventfool dbContext) 
        {
            _dbContext = dbContext;
        }
        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public virtual T Read(Guid id)
        {
            return (_dbContext.Set<T>().Find(id));
        }

        public IEnumerable<T> ReadAll()
        {
            //return (_dbContext.Set<T>().ToList());
            return _dbContext.Set<T>();
        }

        public abstract void Update(T entity);

    }
}
