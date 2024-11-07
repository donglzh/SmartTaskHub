using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTaskHub.Core.Entity;

namespace SmartTaskHub.DataAccess.Configurations;

/// <summary>
/// EquipMaintOrderConfig
/// </summary>
public class EquipMaintOrderConfig : IEntityTypeConfiguration<EquipMaintOrder>
{
    public void Configure(EntityTypeBuilder<EquipMaintOrder> builder)
    {
        builder.ToTable("EquipMaintOrders");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x=>x.Code)
            .IsRequired()
            .HasMaxLength(50);

        // 创建唯一索引
        builder.HasIndex(x => x.Code).IsUnique();
    }
}