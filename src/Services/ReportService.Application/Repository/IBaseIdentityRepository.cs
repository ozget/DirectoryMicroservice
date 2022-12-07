using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Application.Repository
{
    public interface IBaseIdentityRepository<T> where T : class
    {
        T Add(T entity);

        int GetPK(string nextval);

        bool Delete(T entity);

        T Update(T entity);

        EntityEntry<T> AddWithoutSave(T entity);
        EntityEntry<T> UpdateWithoutSave(T entity);
        EntityEntry<T> DeleteWithoutSave(T entity);
        IQueryable<T> All();

        IQueryable<T> Where(Expression<Func<T, bool>> where);

        IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc);
        bool Save();

        Task<int> SaveAsync();

        Task<bool> AddAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        Task<bool> UpdateAsync(T entity);
    }
}
