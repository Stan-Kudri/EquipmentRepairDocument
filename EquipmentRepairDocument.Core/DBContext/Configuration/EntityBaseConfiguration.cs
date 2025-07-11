using EquipmentRepairDocument.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EquipmentRepairDocument.Core.DBContext.Configuration
{
    public abstract class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            ConfigureModel(builder);
        }

        protected abstract void ConfigureModel(EntityTypeBuilder<T> builder);
    }
}
