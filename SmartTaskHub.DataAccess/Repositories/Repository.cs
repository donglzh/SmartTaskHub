using Microsoft.EntityFrameworkCore;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Core.Interface;
namespace SmartTaskHub.DataAccess.Repositories;

/// <summary>
/// Repository
/// </summary>
/// <typeparam name="T">T</typeparam>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<T>();
    }

    /// <summary>
    /// GetById
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<T> GetByIdAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="entity">entity</param>
    public async Task AddAsync(T entity)
    {
       await _dbSet.AddAsync(entity);
       await _dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Update
    /// </summary>
    /// <param name="entity">entity</param>
    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _dbContext.SaveChanges();
    }
    
    
    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="entity">entity</param>
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _dbContext.SaveChanges();
    }
}