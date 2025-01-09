using Laundry.Domain.Contracts.Repositories;
using Laundry.Domain.Entities;
using Laundry.Domain.Interfaces;
using Laundry.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Laundry.DataAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly LaundryDbContext _context;

    public GenericRepository(LaundryDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<T>>> GetAllAsync()
    {
        try
        {
            var list = await _context.Set<T>().AsNoTracking().ToListAsync();
            if (list == null)
            {
                return Result<IEnumerable<T>>.Fail<IEnumerable<T>>($"No {typeof(T)}s");
            }

            return Result.Success((IEnumerable<T>) list);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<T>>($"Error fetching {typeof(T)}: {ex.Message}");
        }
    }

    public async Task<Result<T>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return Result<T>.Fail<T>($"No entity {id}");
            }
            
            return Result<T>.Success<T>(entity);
        }
        catch (Exception e)
        {
            return Result<T>.Fail<T>(e.Message);
        }
    }

    public async Task<Result<int>> CreateAsync(T entity)
    {
        try
        {
            var entityEntry = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return Result<int>.Success(entityEntry.Entity.Id);
        }
        catch (Exception e)
        {
            return Result<int>.Fail<int>(e.Message);
        }
    }

    public async Task<Result> UpdateAsync(T entity)
    {
        try
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}