using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTaskHub.Core.Entity;

namespace SmartTaskHub.DataAccess.Configurations;

/// <summary>
/// TaskTimeoutRuleConfig
/// </summary>
public class TaskTimeoutRuleConfig : IEntityTypeConfiguration<TaskTimeoutRule>
{
    public void Configure(EntityTypeBuilder<TaskTimeoutRule> builder)
    {
        builder.ToTable("TaskTimeoutRules");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TaskType)
            .IsRequired()
            .HasMaxLength(50);

        // 创建唯一索引
        builder.HasIndex(x => x.TaskType).IsUnique();
    }
}