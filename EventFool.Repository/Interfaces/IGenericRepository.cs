using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        void Create(T entity);
        IEnumerable<T> ReadAll();
        T Read(Guid id);
        void Update(T entity);
        void Delete(T entity);

    }
}
