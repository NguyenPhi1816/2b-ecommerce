using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Sku
{
    public Guid Id { get; set; }

    public Guid SpuId { get; set; }

    public Guid UnitId { get; set; }

    public string SkuCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Image { get; set; } = null!;

    public bool IsActive { get; set; }

    public int BaseQtyPerSku { get; set; }

    public virtual ICollection<Batches> Batches { get; set; } = new List<Batches>();

    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();

    public virtual ICollection<SkuDiscounts> SkuDiscounts { get; set; } = new List<SkuDiscounts>();

    public virtual ICollection<SkuOptionValues> SkuOptionValues { get; set; } = new List<SkuOptionValues>();

    public virtual Spu Spu { get; set; } = null!;

    public virtual Units Unit { get; set; } = null!;
}
