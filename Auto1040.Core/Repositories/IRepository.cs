using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetList();

        T? GetById(int id);

        T? Add(T entity);

        bool Delete(int id);

        T? Update(int id, T entity);
    }
}
