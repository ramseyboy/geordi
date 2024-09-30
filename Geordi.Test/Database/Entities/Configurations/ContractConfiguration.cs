using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geordi.Test.Database.Entities.Configurations;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> entity)
    {
        RelationalEntityTypeBuilderExtensions.ToTable((EntityTypeBuilder) entity, "Contracts");

        entity.Property(e => e.Id).HasDefaultValueSql("newid()");

        entity.Property(e => e.Buyer)
            .IsRequired()
            .HasColumnType("varchar(250)");

        entity.Property(e => e.EndDate).HasColumnType("date");

        entity.Property(e => e.Name)
            .IsRequired()
            .HasColumnType("varchar(250)");

        entity.Property(e => e.Number)
            .IsRequired()
            .HasColumnType("varchar(50)");

        entity.Property(e => e.Status)
            .IsRequired()
            .HasColumnType("varchar(250)");

        entity.Property(e => e.Supplier)
            .IsRequired()
            .HasColumnType("varchar(250)");

        RelationalForeignKeyBuilderExtensions.HasConstraintName((ReferenceCollectionBuilder) entity.HasOne(d => d.ContractType)
            .WithMany(p => p.Contracts)
            .HasForeignKey(d => d.TypeId)
            .OnDelete(DeleteBehavior.Restrict), "FK_Contract_ContractType");
    }
}
