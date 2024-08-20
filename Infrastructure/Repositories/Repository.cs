using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _unitOfWork;

        public Repository(AppDbContext unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        IQueryable<TEntity> IRepository<TEntity>.GetAll()
        {
            return GetSet().AsQueryable<TEntity>(); ;
        }

        public IQueryable<TEntity> GetByQuery(Expression<Func<TEntity, bool>> expression)
        {
            return GetSet().Where(expression).AsQueryable();
        }

        public TEntity Create(TEntity entity)
        {
            GetSet().Add(entity);
            _unitOfWork.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _unitOfWork.Attach<TEntity>(entity);
            _unitOfWork.SaveChanges();
            return entity;
        }

        public bool DeleteById(Guid id)
        {
            try
            {
                var item = this.GetById(id);

                if (item != null)
                {
                    _unitOfWork.Attach(item);
                    GetSet().Remove(item);
                    _unitOfWork.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public TEntity GetById(Guid id)
        {
            return GetSet().Find(id);
        }

        protected DbSet<TEntity> GetSet()
        {
            return _unitOfWork.Set<TEntity>();
        }
    }
}
