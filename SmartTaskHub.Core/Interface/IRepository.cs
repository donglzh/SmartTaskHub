using SmartTaskHub.Core.Entity;

namespace SmartTaskHub.Core.Interface;

/// <summary>
/// IRepository
/// </summary>
/// <typeparam name="T">T</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// GetById
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    Task<T> GetByIdAsync(long id);
    
    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();
    
    /// <summary>
    /// Add
    /// </summary>
    /// <param name="entity">entity</param>
    Task AddAsync(T entity);
    
    /// <summary>
    /// Update
    /// </summary>
    /// <param name="entity">entity</param>
    void Update(T entity);
    
    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="entity">entity</param>
    void Delete(T entity);
}