using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Persistence
{
    public class EntityFrameworkDAL : IDAL
    {
        private readonly DbContextPSW dbContext;

        public EntityFrameworkDAL(DbContextPSW dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Insert<T>(T entity) where T : class
        {
            dbContext.Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return dbContext.Set<T>();
        }

        public T GetById<T>(IComparable id) where T : class
        {
            return dbContext.Set<T>().Find(id);
        }

        public bool Exists<T>(IComparable id) where T : class
        {
            return dbContext.Set<T>().Find(id) != null;
        }

        public void Clear<T>() where T : class
        {
            dbContext.Set<T>().RemoveRange(dbContext.Set<T>());
        }

        public IEnumerable<T> GetWhere<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return dbContext.Set<T>().Where(predicate).AsEnumerable();
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public void RemoveAllData()
        {
            dbContext.RemoveAllData();
        }

        public void Update<T>(T modifiedEntity) where T : class
        {
            dbContext.Entry(modifiedEntity).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
}
