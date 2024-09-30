using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geordi.Test.Database.Entities.Configurations;

public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractType>
{
    public void Configure(EntityTypeBuilder<ContractType> entity)
    {
        RelationalEntityTypeBuilderExtensions.ToTable((EntityTypeBuilder) entity, "ContractTypes");

        entity.Property(e => e.Id).HasDefaultValueSql("newid()");

        entity.Property(e => e.Type)
            .IsRequired()
            .HasColumnType("varchar(500)");
    }
}
