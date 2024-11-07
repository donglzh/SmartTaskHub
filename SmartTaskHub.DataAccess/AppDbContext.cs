using Microsoft.EntityFrameworkCore;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Core.Snowflake;
using SmartTaskHub.Core.Common;

namespace SmartTaskHub.DataAccess;

public class AppDbContext : DbContext
{
    private readonly IIdGenerator _idGenerator;
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IIdGenerator idGenerator) : base(options)
    {
        _idGenerator = idGenerator;
    }
    
    public DbSet<TaskTimeoutRule> TaskTimeoutRules { get; set; }
    
    public DbSet<EquipMaintOrder> EquipMaintOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 应用所有实体配置
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        ProcessEntries();
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessEntries();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void ProcessEntries()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var currentTime = TimestampHelper.GetCurrentTimestamp();
    
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Id = _idGenerator.NextId();
                    entry.Entity.CreatedAt = currentTime;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = currentTime;
                    break;
            }
        } 
    }
}