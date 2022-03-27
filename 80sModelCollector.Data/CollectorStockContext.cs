using Microsoft.EntityFrameworkCore;
using _80sModelCollector.Data.Configurations;


namespace _80sModelCollector.Data
{
    /// <summary>
    /// A context class for accessing the database from the app.
    /// </summary>
    public class CollectorStockContext : DbContext
    {
        internal const string SchemaName = "dbo";


        /// <summary>
        /// This constructor used to configure for tests and the real application.
        /// </summary>
        /// <param name="options"></param>
        public CollectorStockContext(DbContextOptions<CollectorStockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Stock> Stock { get; set; }

        /// <summary>
        /// Helper method for creating the database tables
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns>void</returns>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);
            modelBuilder.ApplyConfiguration(new StockConfiguration());
        }
    }
}