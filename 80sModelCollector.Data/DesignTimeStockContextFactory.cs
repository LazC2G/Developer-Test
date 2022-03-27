using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _80sModelCollector.Data
{

    /// <summary>
    /// Used by EF design time tools.
    ///
    /// E.g. the following command will update the specified database
    ///
    /// dotnet ef database update
    /// </summary>
    public class DesignTimeStockContextFactory : IDesignTimeDbContextFactory<CollectorStockContext>
    {
        public CollectorStockContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<CollectorStockContext>()
                .UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=CollectorStock; Integrated Security=True",
                    x => x.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        CollectorStockContext.SchemaName))
                .Options;
            return new CollectorStockContext(options);
        }
    }
}