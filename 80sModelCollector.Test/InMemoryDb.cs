using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _80sModelCollector.Data;
using Xunit;
using _80sModelCollector.Controllers;
using Moq;
using Microsoft.EntityFrameworkCore;
using _80sModelCollector.Models;


namespace ModuleProfile.Tests.Controllers
{
    public class InMemoryDb : IDisposable
    {
        public CollectorStockContext Context { get; set; }

        public InMemoryDb()
        {
            var options = new DbContextOptionsBuilder<CollectorStockContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            Context = new CollectorStockContext(options);
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
