using BPM.Core.Domain.Common;

namespace BPM.Core.Domain.Entities;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Phone { get; set; }
    public string? Department { get; set; }
    public string? TenantId { get; set; }
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}

/// <summary>
/// Represents a role
/// </summary>
public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? TenantId { get; set; }
    
    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

/// <summary>
/// Represents a permission
/// </summary>
public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? Description { get; set; }
}

/// <summary>
/// Many-to-many relationship between users and roles
/// </summary>
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}

/// <summary>
/// Many-to-many relationship between roles and permissions
/// </summary>
public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    // Navigation properties
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}

/// <summary>
/// Direct user permissions (in addition to role-based)
/// </summary>
public class UserPermission : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid PermissionId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}

/// <summary>
/// Represents a tenant for multi-tenancy
/// </summary>
public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string TenantKey { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? ConnectionString { get; set; }
    public string? Theme { get; set; }
    public string? Logo { get; set; }
}
