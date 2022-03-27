using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace _80sModelCollector.Data.Configurations
{
    class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stock")
                .HasKey(x => x.SerialNumber)
                .HasName("SerialNumber_PK");

            builder.Property(x => x.SerialNumber)
                .HasMaxLength(8)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(64);
            
            builder.Property(x => x.Price)
                .HasMaxLength(8);

            builder.Property(x => x.Picture)
                .HasMaxLength(128);

            builder.Property(x => x.Description);

            builder.Property(x => x.RemainingStock)
                .HasMaxLength(8);

        }
    }
}
