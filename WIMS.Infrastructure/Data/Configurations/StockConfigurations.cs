using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WIMS.Domain.Entity;

namespace WIMS.Infrastructure.Data.Configurations;

public class StockRecordConfiguration : IEntityTypeConfiguration<StockRecord>
{
    public void Configure(EntityTypeBuilder<StockRecord> b)
    {
        b.ToTable("stock_records");

        b.Property(x => x.Quantity).HasPrecision(12, 2).IsRequired();
        b.Property(x => x.LastUpdatedAt).IsRequired();

        b.HasIndex(x => new { x.ProductId, x.BinId }).IsUnique();

        b.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Bin)
            .WithMany()
            .HasForeignKey(x => x.BinId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> b)
    {
        b.ToTable("stock_movements");

        b.Property(x => x.QuantityChange).HasPrecision(12, 2).IsRequired();
        b.Property(x => x.QuantityBefore).HasPrecision(12, 2).IsRequired();
        b.Property(x => x.QuantityAfter).HasPrecision(12, 2).IsRequired();
        b.Property(x => x.ReferenceType).HasMaxLength(50);
        b.Property(x => x.Notes).HasMaxLength(500);
        b.Property(x => x.PerformedAt).IsRequired();

        b.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Bin)
            .WithMany()
            .HasForeignKey(x => x.BinId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.PerformedByUser)
            .WithMany()
            .HasForeignKey(x => x.PerformedBy)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
    }
}