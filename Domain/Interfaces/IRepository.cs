using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(int id);
        IQueryable<TEntity> GetByQuery(Expression<Func<TEntity, bool>> expression);
        TEntity Add(TEntity entity);
        TEntity Save(TEntity entity);
        void DeleteById(int id);
    }
}
