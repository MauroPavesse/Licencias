using Licencias.Application.Shared;
using Licencias.Domain.Common;
using Licencias.Infrastructure.Data;
using Licencias.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Licencias.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error al insertar en la base de datos.", ex);
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error al eliminar en la base de datos.", ex);
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error al consultar en la base de datos.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, IEnumerable<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool disableTracking = false)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = include(query);
                    }
                }

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (disableTracking)
                {
                    query = query.AsNoTracking();
                }
                else
                {
                    query = query.AsTracking();
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error al consultar en la base de datos.", ex);
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error al obtener por Id en la base de datos.", ex);
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var trackedEntity = await _dbSet.FindAsync(entity.Id);

                if (trackedEntity != null)
                {
                    _dbSet.Entry(trackedEntity).CurrentValues.SetValues(entity);
                    _dbSet.Entry(trackedEntity).State = EntityState.Modified;
                }
                else
                {
                    _dbSet.Entry(entity).State = EntityState.Added;
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error al actualizar en la base de datos.", ex);
            }
        }
    }
}
