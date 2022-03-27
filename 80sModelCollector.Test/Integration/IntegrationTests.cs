using System;
using System.Collections.Generic;
using System.Text;
using _80sModelCollector.Controllers;
using _80sModelCollector.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModuleProfile.Tests.Controllers;
using Moq;
using Xunit;

namespace _80sModelCollector.Test.Integration
{
    public class IntegrationTests
    {
        [Fact]
        public void Database_Returns_One_Record()
        {
            var logger = Mock.Of<ILogger<HomeController>>();
            var stockdB = new InMemoryDb();
            var httpAccessor = Mock.Of<IHttpContextAccessor>();
            var config = Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeTenantId = "abcd";
            context.Request.Headers["Tenant-ID"] = fakeTenantId;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            Mock<ISession> sessionMock = new Mock<ISession>();
            var key = "1";
            int orderAmount = 1;
            var value = new byte[]
            {
                (byte)(orderAmount>> 24),
                (byte)(0xFF & (orderAmount>> 16)),
                (byte)(0xFF & (orderAmount>> 8)),
                (byte)(0xFF & orderAmount)
            };

            sessionMock.Setup(_ => _.TryGetValue(key, out value)).Returns(true);
            mockHttpContextAccessor.Object.HttpContext.Session = sessionMock.Object;


            using (var tx = stockdB.Context.Database.BeginTransaction())
            {
                var stock = new Stock
                {
                    SerialNumber = 1,
                    Name = "Test1",
                    Price = 34.99,
                    Picture = "Picture.jpg",
                    Description = "Something",
                    RemainingStock = 1
                };

                stockdB.Context.Stock.Add(stock);
                stockdB.Context.SaveChanges();
                tx.Commit();
            }

            var controller = new HomeController(logger, mockHttpContextAccessor.Object, stockdB.Context);
            var controllerResult = controller.Index();

            Assert.NotNull(controllerResult);
            var viewResult = controllerResult as ViewResult;
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);
            Assert.NotNull(viewResult.ViewData.Model);
            List<Stock> stockItems = (List<Stock>) viewResult.ViewData.Model;
            foreach (Stock dBRecord in stockItems)
            {
                Assert.Equal("Test1", dBRecord.Name );
            }

        }
        [Fact]
        public void Database_Returns_Three_Records()
        {
            var logger = Mock.Of<ILogger<HomeController>>();
            var stockdB = new InMemoryDb();
            var httpAccessor = Mock.Of<IHttpContextAccessor>();
            var config = Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeTenantId = "abcd";
            context.Request.Headers["Tenant-ID"] = fakeTenantId;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            Mock<ISession> sessionMock = new Mock<ISession>();
            var key = "1";
            int orderAmount = 1;
            var value = new byte[]
            {
                (byte)(orderAmount>> 24),
                (byte)(0xFF & (orderAmount>> 16)),
                (byte)(0xFF & (orderAmount>> 8)),
                (byte)(0xFF & orderAmount)
            };

            sessionMock.Setup(_ => _.TryGetValue(key, out value)).Returns(true);
            mockHttpContextAccessor.Object.HttpContext.Session = sessionMock.Object;


            using (var tx = stockdB.Context.Database.BeginTransaction())
            {
                var stock = new Stock
                {
                    SerialNumber = 1,
                    Name = "Test1",
                    Price = 34.99,
                    Picture = "Picture.jpg",
                    Description = "Something",
                    RemainingStock = 1
                };

                stockdB.Context.Stock.Add(stock);

                var stock2 = new Stock
                {
                    SerialNumber = 2,
                    Name = "Test2",
                    Price = 19.95,
                    Picture = "Picture.jpg",
                    Description = "Something else",
                    RemainingStock = 10
                };

                stockdB.Context.Stock.Add(stock2);

                var stock3 = new Stock
                {
                    SerialNumber = 3,
                    Name = "Test3",
                    Price = 50.00,
                    Picture = "Picture.jpg",
                    Description = "Something completely different",
                    RemainingStock = 5
                };

                stockdB.Context.Stock.Add(stock3);
                stockdB.Context.SaveChanges();

                tx.Commit();
            }

            var controller = new HomeController(logger, mockHttpContextAccessor.Object, stockdB.Context);
            var controllerResult = controller.Index();

            Assert.NotNull(controllerResult);
            var viewResult = controllerResult as ViewResult;
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.ViewData);
            Assert.NotNull(viewResult.ViewData.Model);
            List<Stock> stockItems = (List<Stock>)viewResult.ViewData.Model;
            Assert.Equal(3, stockItems.Count);

            int i = 1;
            foreach (Stock dBRecord in stockItems)
            {
                Assert.Equal(i++, dBRecord.SerialNumber);
            }

        }
    }
}
