using BPM.Core.Domain.Entities;

namespace BPM.Core.Domain.Interfaces;

/// <summary>
/// Generic repository interface
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Unit of Work interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<ProcessDefinition> ProcessDefinitions { get; }
    IRepository<WorkflowInstance> WorkflowInstances { get; }
    IRepository<TaskInstance> TaskInstances { get; }
    IRepository<FormDefinition> FormDefinitions { get; }
    IRepository<FormSubmission> FormSubmissions { get; }
    IRepository<RuleDefinition> RuleDefinitions { get; }
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<Permission> Permissions { get; }
    IRepository<Tenant> Tenants { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
