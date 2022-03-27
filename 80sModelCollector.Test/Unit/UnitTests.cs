using System;
using _80sModelCollector.Controllers;
using _80sModelCollector.Data;
using _80sModelCollector.Models;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModuleProfile.Tests.Controllers;
using Xunit;
using Moq;

namespace _80sModelCollector.Test
{
    public class UnitTests
    {

        [Fact]
        public void BasketModel_AddGoodItem()
        {
            BasketModel basket = new BasketModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            Assert.True(basket.AddStockItem(item));
        }

        [Fact]
        public void BasketModel_AddFullItem()
        {
            BasketModel basket = new BasketModel();
            BasketItem item = new BasketItem()
            {
                SerialNumber = "2",
                Name = "Some Item",
                Orders = 1,
                Price = 20.50
            };

            Assert.True(basket.AddStockItem(item));
        }

        [Fact]
        public void BasketModel_Add3Items()
        {
            BasketModel basket = new BasketModel();
            BasketItem item = new BasketItem()
            {
                SerialNumber = "1",
                Name = "Some Item",
                Orders = 1,
                Price = 20.50
            };

            Assert.True(basket.AddStockItem(item));

            BasketItem item2 = new BasketItem()
            {
                SerialNumber = "2",
                Name = "Some other Item",
                Orders = 1,
                Price = 5.0
            };

            Assert.True(basket.AddStockItem(item2));

            BasketItem item3 = new BasketItem()
            {
                SerialNumber = "3",
                Name = "Yet another Item",
                Orders = 10,
                Price = 150.0
            };

            Assert.True(basket.AddStockItem(item3));
        }

        [Fact]
        public void BasketModel_BadItemFails()
        {
            BasketModel basket = new BasketModel();
            BasketItem item = new BasketItem();

            Assert.False(basket.AddStockItem(item));
        }

        [Fact]
        public void CheckOutModel_AddItemThroughSubClass()
        {
            CheckOutModel checkout = new CheckOutModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            Assert.True(checkout.AddStockItem(item));
        }

        [Fact]
        public void CheckOutModel_GoodSubTotal()
        {
            CheckOutModel checkout = new CheckOutModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            checkout.AddStockItem(item);

            Assert.True(checkout.SetSubTotal(50.5));
        }

        [Fact]
        public void CheckOutModel_BadSubTotal()
        {
            CheckOutModel checkout = new CheckOutModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            checkout.AddStockItem(item);

            Assert.False(checkout.SetSubTotal(-1.5));
        }

        [Fact]
        public void CheckOutModel_GoodDiscountPrice()
        {
            CheckOutModel checkout = new CheckOutModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            checkout.AddStockItem(item);

            Assert.True(checkout.SetDiscountedPrice(25.0));
        }

        [Fact]
        public void CheckOutModel_BadDiscountTotal()
        {
            CheckOutModel checkout = new CheckOutModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            checkout.AddStockItem(item);

            Assert.False(checkout.SetDiscountedPrice(-10.0));
        }


        [Fact]
        public void CheckOutModel_GetValidDiscountedPrice()
        {
            CheckOutModel checkout = new CheckOutModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            checkout.AddStockItem(item);
            checkout.SetSubTotal(50.5);
            checkout.SetDiscountedPrice(12.7);

            Assert.Equal(checkout.GetDiscountedPrice(), 12.7);
        }

        [Fact]
        public void CheckOutModel_GetInvalidDiscountedPrice()
        {
            CheckOutModel checkout = new CheckOutModel();
            BasketItem item = new BasketItem();

            item.SerialNumber = "1";

            checkout.AddStockItem(item);
            checkout.SetSubTotal(50.5);

            Assert.Equal(checkout.GetDiscountedPrice(), 50.5);
        }

        [Fact]
        public void HomeController_Construct()
        {
            var logger = Mock.Of<ILogger<HomeController>>();
            var stockdB = new InMemoryDb();
            var httpAccessor = Mock.Of<IHttpContextAccessor>();

            var controller = new HomeController(logger, httpAccessor, stockdB.Context);

            Assert.NotNull(controller);
        }

        [Fact]
        public void HomeController_Index()
        {
            var logger = Mock.Of<ILogger<HomeController>>();
            var stockdB = new InMemoryDb();
            var httpAccessor = Mock.Of<IHttpContextAccessor>();

            var controller = new HomeController(logger, httpAccessor, stockdB.Context);
            var controllerResult = controller.Index();

            Assert.NotNull(controllerResult);
            var viewResult = controllerResult as ViewResult;
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void HomeController_Login()
        {
            var logger = Mock.Of<ILogger<HomeController>>();
            var stockdB = new InMemoryDb();
            var httpAccessor = Mock.Of<IHttpContextAccessor>();
            
            var controller = new HomeController(logger, httpAccessor, stockdB.Context);
            var controllerResult = controller.Login();

            Assert.NotNull(controllerResult);
            var viewResult = controllerResult as ViewResult;
            Assert.NotNull(viewResult);
        }

        
        [Theory]
        [InlineData(1, true)]
        [InlineData(10, true)]
        [InlineData(111, true)]
        [InlineData(-1, true)]
        [InlineData(-263, true)]
        public void HomeController_BasketButtonClick(int testValue,  bool testExpected)
        {
            //arrange
            var logger = Mock.Of<ILogger<HomeController>>();
            var stockdB = new InMemoryDb();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeTenantId = "abcd";
            context.Request.Headers["Tenant-ID"] = fakeTenantId;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            Mock<ISession> sessionMock = new Mock<ISession>();
            var key = "1";
            int orderAmount= 1;
            var value = new byte[]
            {
                (byte)(orderAmount>> 24),
                (byte)(0xFF & (orderAmount>> 16)),
                (byte)(0xFF & (orderAmount>> 8)),
                (byte)(0xFF & orderAmount)
            };

            sessionMock.Setup(_ => _.TryGetValue(key, out value)).Returns(true);
            mockHttpContextAccessor.Object.HttpContext.Session = sessionMock.Object;

            var controller = new HomeController(logger, mockHttpContextAccessor.Object, stockdB.Context);

            //act
            var controllerResult = controller.BasketButtonClick(testValue);

            //assert
            Assert.NotNull(controllerResult);
            var viewResult = controllerResult as ViewResult;
            Assert.Equal(viewResult != null, testExpected); ;
        }


        [Fact]
        public void BasketController_Construct()
        {
            var logger = Mock.Of<ILogger<BasketController>>();
            var stockdB = new InMemoryDb();
            var httpAccessor = Mock.Of<IHttpContextAccessor>();
            var config = Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>();

            var controller = new BasketController(logger, httpAccessor, stockdB.Context, config);

            Assert.NotNull(controller);
        }

        [Fact]
        public void BasketController_Index()
        {
            var logger = Mock.Of<ILogger<BasketController>>();
            var stockdB = new InMemoryDb();
            var httpAccessor = Mock.Of<IHttpContextAccessor>();
            var config = Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>();


            var controller = new BasketController(logger, httpAccessor, stockdB.Context, config);
            var controllerResult = controller.Index();

            Assert.NotNull(controllerResult);
            var viewResult = controllerResult as ViewResult;
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void BasketController_Basket()
        {
            var logger = Mock.Of<ILogger<BasketController>>();
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


            var controller = new BasketController(logger, mockHttpContextAccessor.Object, stockdB.Context, config);
            var controllerResult = controller.Basket();

            Assert.NotNull(controllerResult);
            var viewResult = controllerResult as ViewResult;
            Assert.NotNull(viewResult);
        }

    }
}
