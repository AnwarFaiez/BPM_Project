using Microsoft.EntityFrameworkCore.Storage;
using BPM.Core.Domain.Entities;
using BPM.Core.Domain.Interfaces;
using BPM.Infrastructure.Data.Context;

namespace BPM.Infrastructure.Data.Repositories;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly BpmDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(BpmDbContext context)
    {
        _context = context;
        ProcessDefinitions = new Repository<ProcessDefinition>(context);
        WorkflowInstances = new Repository<WorkflowInstance>(context);
        TaskInstances = new Repository<TaskInstance>(context);
        FormDefinitions = new Repository<FormDefinition>(context);
        FormSubmissions = new Repository<FormSubmission>(context);
        RuleDefinitions = new Repository<RuleDefinition>(context);
        Users = new Repository<User>(context);
        Roles = new Repository<Role>(context);
        Permissions = new Repository<Permission>(context);
        Tenants = new Repository<Tenant>(context);
    }

    public IRepository<ProcessDefinition> ProcessDefinitions { get; }
    public IRepository<WorkflowInstance> WorkflowInstances { get; }
    public IRepository<TaskInstance> TaskInstances { get; }
    public IRepository<FormDefinition> FormDefinitions { get; }
    public IRepository<FormSubmission> FormSubmissions { get; }
    public IRepository<RuleDefinition> RuleDefinitions { get; }
    public IRepository<User> Users { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Permission> Permissions { get; }
    public IRepository<Tenant> Tenants { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}
