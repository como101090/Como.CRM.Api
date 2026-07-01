using Como.CRM.Api.Common;
using Como.CRM.Api.Domain.Entities;
using Como.CRM.Api.Domain.Entities.CompanyInfo;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;
using System.Text.Json;

namespace Como.CRM.Api.Data;

public class AppDbContext : DbContext
{
    private readonly ICurrentTenantService _tenant;
    private readonly ICurrentUserService _user;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentTenantService tenant,
        ICurrentUserService user)
        : base(options)
    {
        _tenant = tenant;
        _user = user;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<ProductServiceGroup> ProductServiceGroups => Set<ProductServiceGroup>();

    public DbSet<PublisherInfo> PublisherInfos => Set<PublisherInfo>();

    public DbSet<PublisherMailInfo> PublisherMailInfos => Set<PublisherMailInfo>();

    public DbSet<PublisherPhoneInfo> PublisherPhoneInfos => Set<PublisherPhoneInfo>();

    public DbSet<PublisherBankInfo> PublisherBankInfos => Set<PublisherBankInfo>();

    public DbSet<PublisherBankAccountInfo> PublisherBankAccountInfos => Set<PublisherBankAccountInfo>();



    private static void PublisherInfoDefaultType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublisherInfo>().HasData(new PublisherInfo
        {
            Id = 1,
            TaxCode = "08284674",
            TaxName = "Քոմո Կոդ ՍՊԸ",
            BrendName = "Como Code LLC",
            Description = "Como CRM publisher information"
        });

        modelBuilder.Entity<PublisherMailInfo>().HasData(new PublisherMailInfo
        {
            Id = 1,
            PublisherInfoId = 1,
            FromName = "Como CRM",
            FromEmail = "comocode.info@gmail.com",
            UserName = "comocode.info@gmail.com",
            Host = "smtp.gmail.com",
            Password = "",
            Port = 587
           
        });

        modelBuilder.Entity<PublisherPhoneInfo>().HasData(new PublisherPhoneInfo
        {
            Id = 1,
            PublisherId = 1,
            PhoneType = "Mobile",
            PhoneNumber = "+37477396770"
        });

        modelBuilder.Entity<PublisherBankInfo>().HasData(new PublisherBankInfo
        {
            Id = 1,
            Code = "0001",
            PublisherInfoId = 1,
            BankName = "Ameriabank"
        });

        modelBuilder.Entity<PublisherBankAccountInfo>().HasData(new PublisherBankAccountInfo
        {
            Id = 1,
            BankInfoId = 1,
            CurrencyName = "AMD",
            AccountNumber = "1570094723317200"
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        PublisherInfoDefaultType(modelBuilder);

        ConfigureTenant(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureRoles(modelBuilder);
        ConfigurePermissions(modelBuilder);
        ConfigureUserRoles(modelBuilder);
        ConfigureRolePermissions(modelBuilder);
        ConfigureProductServiceGroups(modelBuilder);
        ConfigureAuditLogs(modelBuilder);

        ConfigurePublisherInfo(modelBuilder);
        ConfigurePublisherPhoneInfo(modelBuilder);
        ConfigurePublisherMailInfo(modelBuilder);
        ConfigurePublisherBankInfo(modelBuilder);
        ConfigurePublisherBankAccountInfo(modelBuilder);

        ApplyTenantQueryFilters(modelBuilder);
    }


    private static void ConfigurePublisherInfo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublisherInfo>(entity =>
        {
            entity.ToTable(nameof(PublisherInfo));

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TaxCode)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.TaxName)
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(x => x.BrendName)
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(4000);
        });
    }

    private static void ConfigurePublisherPhoneInfo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublisherPhoneInfo>(entity =>
        {
            entity.ToTable(nameof(PublisherPhoneInfo));

            entity.HasKey(x => x.Id);

            entity.Property(x => x.PhoneType)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.PhoneNumber)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasOne(x => x.PublisherInfo)
                .WithMany(x => x.Phones)
                .HasForeignKey(x => x.PublisherId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }


    private static void ConfigurePublisherMailInfo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublisherMailInfo>(entity =>
        {
            entity.ToTable(nameof(PublisherMailInfo));

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Host)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Port)
                .IsRequired();

            entity.Property(x => x.FromName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.FromEmail)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.UserName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Password)
                .HasMaxLength(500);

            entity.HasOne(x => x.PublisherInfo)
                .WithMany(x => x.Mails)
                .HasForeignKey(x => x.PublisherInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigurePublisherBankInfo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublisherBankInfo>(entity =>
        {
            entity.ToTable(nameof(PublisherBankInfo));

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.BankName)
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(x => x.BankDockName)
                .HasMaxLength(300);

            entity.HasOne(x => x.PublisherInfo)
                .WithMany(x => x.Banks)
                .HasForeignKey(x => x.PublisherInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigurePublisherBankAccountInfo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublisherBankAccountInfo>(entity =>
        {
            entity.ToTable(nameof(PublisherBankAccountInfo));

            entity.HasKey(x => x.Id);

            entity.Property(x => x.CurrencyName)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.AccountCode)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.AccountNumber)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasOne(x => x.BankInfo)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.BankInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureTenant(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.LegalName)
                .HasMaxLength(300);

            b.Property(x => x.BrandName)
                .HasMaxLength(150)
                .IsRequired();

            b.Property(x => x.Host)
                .HasMaxLength(150)
                .IsRequired();

            b.Property(x => x.CompanyEmail)
                .HasMaxLength(150)
                .IsRequired();

            b.Property(x => x.ContactPhone)
                .HasMaxLength(50);

            b.HasIndex(x => x.Host)
                .IsUnique();
        });
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(b =>
        {
            ConfigureBase(b);

            b.Property(x => x.UserName)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(x => x.PasswordHash)
                .IsRequired();

            b.HasIndex(x => new { x.TenantId, x.UserName })
                .IsUnique();
        });
    }
    
    private static void ConfigureRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(b =>
        {
            ConfigureBase(b);

            b.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            b.HasIndex(x => new { x.TenantId, x.Name })
                .IsUnique();
        });
    }

    private static void ConfigureProductServiceGroups(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductServiceGroup>(b =>
        {
            ConfigureBase(b);

            b.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            b.Property(x => x.GroupName)
                .HasMaxLength(150)
                .IsRequired();

            b.HasIndex(x => new
            {
                x.TenantId,
                x.Code,
                x.IsProduct,
                x.IsRemove
            });
        });
    }

    private static void ConfigurePermissions(ModelBuilder modelBuilder)
    {
        var permissions = PermissionCodes.All
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .Select((code, index) => new Permission
            {
                Id = index + 1,
                Code = code,
                Description = code
            })
            .ToArray();

        modelBuilder.Entity<Permission>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Code)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(x => x.Description)
                .HasMaxLength(250);

            b.HasIndex(x => x.Code)
                .IsUnique();

            b.HasData(permissions);
        });
    }

    private void ConfigureUserRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>(b =>
        {
            b.HasKey(x => new { x.TenantId, x.UserId, x.RoleId });

            b.HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureRolePermissions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermission>(b =>
        {
            b.HasKey(x => new { x.TenantId, x.RoleId, x.PermissionId });

            b.HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureAuditLogs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.EntityName)
                .HasMaxLength(150)
                .IsRequired();

            b.Property(x => x.OperationType)
                .HasMaxLength(30)
                .IsRequired();

            b.HasIndex(x => new
            {
                x.TenantId,
                x.EntityName,
                x.EntityId
            });
        });
    }

    private void ApplyTenantQueryFilters(ModelBuilder modelBuilder)
    {
        var method = typeof(AppDbContext)
            .GetMethod(nameof(SetTenantQueryFilter), BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("SetTenantQueryFilter method was not found.");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (!typeof(IHasTenant).IsAssignableFrom(clrType))
                continue;

            method.MakeGenericMethod(clrType)
                .Invoke(this, new object[] { modelBuilder });
        }
    }

    private void SetTenantQueryFilter<TEntity>(ModelBuilder modelBuilder)
     where TEntity : class, IHasTenant
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(x =>
            x.TenantId == _tenant.TenantId);
    }

    // ինչ որ ժառանքժգածա BaseTenantEntity հանգարծ
    // առանց TenantId տող չգրանցվի
    private static void ConfigureBase<TEntity>(
    EntityTypeBuilder<TEntity> b)
    where TEntity : BaseTenantEntity
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.TenantId)
            .IsRequired();

        b.HasIndex(x => x.TenantId);

        b.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTenantAndAudit();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyTenantAndAudit();
        return base.SaveChanges();
    }

    private void ApplyTenantAndAudit()
    {
        var auditLogs = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries<IHasTenant>())
        {
            if (entry.State is EntityState.Detached or EntityState.Unchanged)
                continue;

            if (!_tenant.IsResolved && !_tenant.IsSystemContext)
                throw new UnauthorizedAccessException(
                    $"Tenant context is required for {entry.Entity.GetType().Name}.");

            if (entry.State == EntityState.Added && entry.Entity.TenantId <=0)
            {
                if (!_tenant.IsResolved)
                    throw new InvalidOperationException(
                        $"TenantId is required for {entry.Entity.GetType().Name}.");

                entry.Entity.TenantId = _tenant.TenantId;
            }

            if (entry.Entity.TenantId <= 0)
                throw new InvalidOperationException(
                    $"TenantId is required for {entry.Entity.GetType().Name}.");

            if (_tenant.IsResolved && entry.Entity.TenantId != _tenant.TenantId)
                throw new UnauthorizedAccessException("Cross-tenant write is blocked.");
        }

        foreach (var entry in ChangeTracker.Entries<BaseTenantEntity>())
        {
            if (entry.State is EntityState.Detached or EntityState.Unchanged)
                continue;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreateDate = DateTime.UtcNow;
                entry.Entity.CreateUserId ??= _user.UserId;
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();

                foreach (var property in entry.Properties.Where(p => p.IsModified))
                {
                    oldValues[property.Metadata.Name] = property.OriginalValue;
                    newValues[property.Metadata.Name] = property.CurrentValue;
                }

                if (oldValues.Count == 0)
                    continue;

                var operation = "Edit";

                if (oldValues.TryGetValue(nameof(BaseTenantEntity.IsRemove), out var oldRemoveValue) &&
                    newValues.TryGetValue(nameof(BaseTenantEntity.IsRemove), out var newRemoveValue))
                {
                    var oldIsRemove = oldRemoveValue as bool? ?? Convert.ToBoolean(oldRemoveValue);
                    var newIsRemove = newRemoveValue as bool? ?? Convert.ToBoolean(newRemoveValue);

                    operation = newIsRemove switch
                    {
                        true when oldIsRemove == false => "Delete",
                        false when oldIsRemove == true => "Restore",
                        _ => "Edit"
                    };
                }

                auditLogs.Add(new AuditLog
                {
                    TenantId = entry.Entity.TenantId,
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entry.Entity.Id,
                    OperationType = operation,
                    UserId = _user.UserId,
                    OldValues = JsonSerializer.Serialize(oldValues),
                    NewValues = JsonSerializer.Serialize(newValues),
                    CreateDate = DateTime.UtcNow
                });
            }
        }

        if (auditLogs.Count > 0)
            AuditLogs.AddRange(auditLogs);
    }
}