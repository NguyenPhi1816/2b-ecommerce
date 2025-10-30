using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class User
{
    public GenderType Gender { get; set; }
}
public partial class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> e)
    {
        e.Property(p => p.Gender).HasColumnType("gender_type");
    }
}
