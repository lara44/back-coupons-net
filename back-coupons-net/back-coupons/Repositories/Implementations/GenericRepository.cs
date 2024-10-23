using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _dbContext;
        private readonly DbSet<T> _entity;

        public GenericRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _entity = dbContext.Set<T>();
        }

        public virtual async Task<ActionResponse<T>> GetAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row != null)
            {
                return new ActionResponse<T>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<T>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                Successfully = true,
                Result = await _entity.ToListAsync()
            };
        }


        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();
            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);
            return new ActionResponse<IEnumerable<T>>
            {
                Successfully = true,
                Result = await queryable
                    .Paginate(pagination)
                    .ToListAsync(),
                TotalPage = totalPages
            };
        }

        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
        {
            _dbContext.Add(entity);
            try
            {
                await _dbContext.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    Successfully = true,
                    Result = entity
                };
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException!.Message.Contains("duplicate"))
                {
                    return DbUpdateExceptionActionResponse();
                }

                return new ActionResponse<T>
                {
                    Successfully = false,
                    Message = ex.Message
                };

            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row == null)
            {
                return new ActionResponse<T>
                {
                    Successfully = false,
                    Message = "Registro no encontrado"
                };
            }

            try
            {
                _entity.Remove(row);
                await _dbContext.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    Successfully = true,
                };
            }
            catch
            {
                return new ActionResponse<T>
                {
                    Successfully = false,
                    Message = "No se puede borrar, porque tiene registros relacionados"
                };
            }
        }

        public virtual async Task<ActionResponse<T>> UpdateAsync(int id, T updatedEntity)
        {
            var entity = await _entity.FindAsync(id);

            if (entity == null)
            {
                return new ActionResponse<T>
                {
                    Successfully = false,
                    Message = "Registro no encontrado"
                };
            }

            try
            {
                _dbContext.Entry(entity).CurrentValues.SetValues(updatedEntity);
                await _dbContext.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    Successfully = true,
                    Result = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                Successfully = false,
                Message = exception.Message
            };
        }

        private ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                Successfully = false,
                Message = "Ya existe el registro que estas intentando crear."
            };
        }
    }
}