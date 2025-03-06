

using Leyadech.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Auto1040.Data.Repositories
{
    public class Repository<T>(DataContext dataContext) : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet = dataContext.Set<T>();

        public T? Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public bool Delete(int id)
        {
            try
            {
                _dbSet.Remove(GetById(id));
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetList()
        {
            return _dbSet.ToList();
        }

        public T? Update(int id, T entity)
        {
            try
            {
                T? source=GetById(id);
                UpdateAllProperties(source, entity);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void UpdateAllProperties(T target, T source)
        {
            if (target == null || source == null)
                throw new ArgumentNullException("Target or source object is null.");

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.CanWrite&& property.GetCustomAttribute<KeyAttribute>() == null) 
                {
                    var value = property.GetValue(source);
                    if (value != null)
                        property.SetValue(target, value);
                }
            }
        }

    }
}
