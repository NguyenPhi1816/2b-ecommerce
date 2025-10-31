using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using _2b_ecommerce.Infrastructure.Models;

namespace _2b_ecommerce.Infrastructure.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Batches> Batches { get; set; }

    public virtual DbSet<CartItems> CartItems { get; set; }

    public virtual DbSet<Carts> Carts { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<DeliveryInfo> DeliveryInfo { get; set; }

    public virtual DbSet<Discounts> Discounts { get; set; }

    public virtual DbSet<InventoryMovements> InventoryMovements { get; set; }

    public virtual DbSet<OptionValues> OptionValues { get; set; }

    public virtual DbSet<Options> Options { get; set; }

    public virtual DbSet<OrderItems> OrderItems { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<Payments> Payments { get; set; }

    public virtual DbSet<Permissions> Permissions { get; set; }

    public virtual DbSet<PurchaseItems> PurchaseItems { get; set; }

    public virtual DbSet<Purchases> Purchases { get; set; }

    public virtual DbSet<RankUsers> RankUsers { get; set; }

    public virtual DbSet<Ranks> Ranks { get; set; }

    public virtual DbSet<Reviews> Reviews { get; set; }

    public virtual DbSet<RolePermissions> RolePermissions { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Sku> Sku { get; set; }

    public virtual DbSet<SkuDiscounts> SkuDiscounts { get; set; }

    public virtual DbSet<SkuOptionValues> SkuOptionValues { get; set; }

    public virtual DbSet<Spu> Spu { get; set; }

    public virtual DbSet<SpuImages> SpuImages { get; set; }

    public virtual DbSet<Suppliers> Suppliers { get; set; }

    public virtual DbSet<Units> Units { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    public virtual DbSet<VoucherRedemptions> VoucherRedemptions { get; set; }

    public virtual DbSet<Vouchers> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("public", "DiscountMode", new[] { "PERCENT", "AMOUNT" })
            .HasPostgresEnum("public", "GenderType", new[] { "MALE", "FEMALE" })
            .HasPostgresEnum("public", "InventoryType", new[] { "IN", "OUT", "TRANSFER_IN", "TRANSFER_OUT", "ADJUST" })
            .HasPostgresEnum("public", "OrderStatus", new[] { "PENDING", "CONFIRMED", "SHIPPED", "DELIVERED", "CANCELLED", "RETURNED" })
            .HasPostgresEnum("public", "PaymentMethod", new[] { "COD", "EWALLET" })
            .HasPostgresEnum("public", "PaymentStatus", new[] { "PENDING", "PAID", "FAILED", "REFUNDED" })
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Batches>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Batches_pkey");

            entity.HasIndex(e => new { e.SkuId, e.BatchCode }, "uq_batches_sku_batch").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.OnHand).HasDefaultValue(0);
            entity.Property(e => e.SellPrice).HasPrecision(18, 2);

            entity.HasOne(d => d.Sku).WithMany(p => p.Batches)
                .HasForeignKey(d => d.SkuId)
                .HasConstraintName("Batches_SkuId_fkey");
        });

        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CartItems_pkey");

            entity.HasIndex(e => new { e.CartId, e.SkuId }, "uq_cart_item").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("CartItems_CartId_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("CartItems_SkuId_fkey");
        });

        modelBuilder.Entity<Carts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Carts_pkey");

            entity.HasIndex(e => e.UserId, "Carts_UserId_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.User).WithOne(p => p.Carts)
                .HasForeignKey<Carts>(d => d.UserId)
                .HasConstraintName("Carts_UserId_fkey");
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Categories_pkey");

            entity.HasIndex(e => e.Slug, "Categories_Slug_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Categories_ParentId_fkey");
        });

        modelBuilder.Entity<DeliveryInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DeliveryInfo_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.IsDefault).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.DeliveryInfo)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("DeliveryInfo_UserId_fkey");
        });

        modelBuilder.Entity<Discounts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Discounts_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.MaxDiscount).HasPrecision(18, 2);
            entity.Property(e => e.Value).HasPrecision(18, 2);
        });

        modelBuilder.Entity<InventoryMovements>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("InventoryMovements_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.MovedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Quantity).HasPrecision(18, 3);

            entity.HasOne(d => d.Batch).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("InventoryMovements_BatchId_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("InventoryMovements_UnitId_fkey");
        });

        modelBuilder.Entity<OptionValues>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OptionValues_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.SortOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Option).WithMany(p => p.OptionValues)
                .HasForeignKey(d => d.OptionId)
                .HasConstraintName("OptionValues_OptionId_fkey");
        });

        modelBuilder.Entity<Options>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Options_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.SortOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Spu).WithMany(p => p.Options)
                .HasForeignKey(d => d.SpuId)
                .HasConstraintName("Options_SpuId_fkey");
        });

        modelBuilder.Entity<OrderItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OrderItems_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.LineTotal).HasPrecision(18, 2);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

            entity.HasOne(d => d.Discount).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("OrderItems_DiscountId_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("OrderItems_OrderId_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("OrderItems_SkuId_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("OrderItems_UnitId_fkey");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Orders_pkey");

            entity.HasIndex(e => e.Code, "Orders_Code_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.ProductDiscount).HasPrecision(18, 2);
            entity.Property(e => e.ShippingFee).HasPrecision(18, 2);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.VoucherDiscount).HasPrecision(18, 2);

            entity.HasOne(d => d.DeliveryInfo).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DeliveryInfoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Orders_DeliveryInfoId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Orders_UserId_fkey");
        });

        modelBuilder.Entity<Payments>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Payments_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.RawPayload).HasColumnType("jsonb");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("Payments_OrderId_fkey");
        });

        modelBuilder.Entity<Permissions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Permissions_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<PurchaseItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PurchaseItems_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.ImportPrice).HasPrecision(18, 2);
            entity.Property(e => e.UnitConversionFactor)
                .HasPrecision(18, 3)
                .HasDefaultValueSql("1");

            entity.HasOne(d => d.Batch).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("PurchaseItems_BatchId_fkey");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("PurchaseItems_PurchaseId_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("PurchaseItems_UnitId_fkey");
        });

        modelBuilder.Entity<Purchases>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Purchases_pkey");

            entity.HasIndex(e => e.PoNumber, "Purchases_PoNumber_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.PurchasedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Purchases_SupplierId_fkey");
        });

        modelBuilder.Entity<RankUsers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RankUsers_pkey");

            entity.HasIndex(e => new { e.UserId, e.RankId }, "uq_rank_users").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.AchievedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.ExpiredAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastOrderAt).HasDefaultValueSql("now()");
            entity.Property(e => e.TotalSpent).HasPrecision(18, 2);

            entity.HasOne(d => d.Rank).WithMany(p => p.RankUsers)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("RankUsers_RankId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.RankUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("RankUsers_UserId_fkey");
        });

        modelBuilder.Entity<Ranks>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Ranks_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.MaxOrderValue).HasPrecision(18, 2);
            entity.Property(e => e.MinOrderValue).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Reviews>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Reviews_pkey");

            entity.HasIndex(e => new { e.UserId, e.SkuId, e.OrderItemId }, "uq_review_purchase").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("Reviews_OrderItemId_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.SkuId)
                .HasConstraintName("Reviews_SkuId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Reviews_UserId_fkey");
        });

        modelBuilder.Entity<RolePermissions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RolePermissions_pkey");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "uq_role_permission").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("RolePermissions_PermissionId_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("RolePermissions_RoleId_fkey");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.HasIndex(e => e.Code, "Roles_Code_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<Sku>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sku_pkey");

            entity.HasIndex(e => e.SkuCode, "Sku_SkuCode_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.BaseQtyPerSku).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(d => d.Spu).WithMany(p => p.Sku)
                .HasForeignKey(d => d.SpuId)
                .HasConstraintName("Sku_SpuId_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Sku)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Sku_UnitId_fkey");
        });

        modelBuilder.Entity<SkuDiscounts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SkuDiscounts_pkey");

            entity.HasIndex(e => new { e.DiscountId, e.SkuId }, "uq_discount_sku").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Discount).WithMany(p => p.SkuDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("SkuDiscounts_DiscountId_fkey");

            entity.HasOne(d => d.Sku).WithMany(p => p.SkuDiscounts)
                .HasForeignKey(d => d.SkuId)
                .HasConstraintName("SkuDiscounts_SkuId_fkey");
        });

        modelBuilder.Entity<SkuOptionValues>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SkuOptionValues_pkey");

            entity.HasIndex(e => new { e.ValueId, e.SkuId }, "uq_sku_option_value").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Sku).WithMany(p => p.SkuOptionValues)
                .HasForeignKey(d => d.SkuId)
                .HasConstraintName("SkuOptionValues_SkuId_fkey");

            entity.HasOne(d => d.Value).WithMany(p => p.SkuOptionValues)
                .HasForeignKey(d => d.ValueId)
                .HasConstraintName("SkuOptionValues_ValueId_fkey");
        });

        modelBuilder.Entity<Spu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Spu_pkey");

            entity.HasIndex(e => e.SpuCode, "Spu_SpuCode_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Category).WithMany(p => p.Spu)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Spu_CategoryId_fkey");
        });

        modelBuilder.Entity<SpuImages>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SpuImages_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.SortOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Spu).WithMany(p => p.SpuImages)
                .HasForeignKey(d => d.SpuId)
                .HasConstraintName("SpuImages_SpuId_fkey");
        });

        modelBuilder.Entity<Suppliers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Suppliers_pkey");

            entity.HasIndex(e => e.SupplierCode, "Suppliers_SupplierCode_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Units>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Units_pkey");

            entity.HasIndex(e => e.Code, "Units_Code_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.HasIndex(e => e.Email, "Users_Email_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Users_RoleId_fkey");
        });

        modelBuilder.Entity<VoucherRedemptions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VoucherRedemptions_pkey");

            entity.HasIndex(e => new { e.VoucherId, e.OrderId }, "uq_voucher_order").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.RedeemedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Order).WithMany(p => p.VoucherRedemptions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("VoucherRedemptions_OrderId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.VoucherRedemptions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("VoucherRedemptions_UserId_fkey");

            entity.HasOne(d => d.Voucher).WithMany(p => p.VoucherRedemptions)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("VoucherRedemptions_VoucherId_fkey");
        });

        modelBuilder.Entity<Vouchers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vouchers_pkey");

            entity.HasIndex(e => e.Code, "Vouchers_Code_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaxDiscount).HasPrecision(18, 2);
            entity.Property(e => e.MinOrderTotal)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("0");
            entity.Property(e => e.Value).HasPrecision(18, 2);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
