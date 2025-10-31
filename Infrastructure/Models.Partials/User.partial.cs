using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Users
{
    public GenderType Gender { get; set; }
}
public partial class UserConfiguration : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> e)
    {
        e.Property(p => p.Gender).HasColumnType("\"GenderType\"");
    }
}
