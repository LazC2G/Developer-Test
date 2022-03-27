using System;
using _80sModelCollector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using _80sModelCollector.Data;
using _80sModelCollector.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace _80sModelCollector.Controllers
{
    /// <summary>
    /// The Basket Controller class looks after the aspects of the site to do with basket operation and it's contents.
    /// It uses session storage to hold the contents of the basket which will time out after 10 minutes.
    /// The session storage holds key value pairs of a basket item serial number and the quantity of orders for each.
    /// It makes use of a database to flesh out the details of the basket item using the serial number provided.
    /// </summary>
    public class BasketController : Controller
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly CollectorStockContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for the Basket Controller class. 
        /// </summary>
        /// <param name="logger">ILogger object for errors</param>
        /// <param name="accessor">IHttpContextAccessor for session storage</param>
        /// <param name="context">CollectorStockContext for accessing the database</param>
        /// <param name="configuration">CollectorStockContext for accessing the database</param>
        public BasketController(ILogger<BasketController> logger, IHttpContextAccessor accessor, CollectorStockContext context, IConfiguration configuration)
        {
            _accessor = accessor;
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Index endpoint for good measure in case someone types it into the broswer
        /// </summary>
        /// <returns><see cref="IActionResult"/>For the view, with empty model</returns>
        public IActionResult Index()
        {
            CheckOutModel basketData = new CheckOutModel();

            return View("Basket", basketData);
        }

        /// <summary>
        /// Error endpoint for bad addresses
        /// </summary>
        /// <returns><see cref="IActionResult"/>For the view, with an error model</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Endpoint for the populated basket page
        /// </summary>
        /// <returns><see cref="IActionResult"/>For the view, with a data filled model</returns>
        public IActionResult Basket()
        {
            CheckOutModel basketData = new CheckOutModel();

            PopulateBasket(basketData);
            

            return View("Basket", basketData);
        }

        /// <summary>
        /// Helper method to fill a CheckOutModel with data from the database and session storage
        /// </summary>
        /// <param name="basket">CheckOutModel to populate</param>
        /// <returns><see cref="void"/>Uses pass by reference to fill the model</returns>
        private void PopulateBasket(CheckOutModel basket)
        {
            foreach (string key in _accessor.HttpContext.Session.Keys)
            {
                Stock dBRecord = new Stock();
                BasketItem item = new BasketItem();

                item.SerialNumber = key;

                try
                {
                    dBRecord = _context.Stock.Where(s => s.SerialNumber == int.Parse(key)).FirstOrDefault();
                    item.Orders = (int)_accessor.HttpContext.Session.GetInt32(key);
                    item.Name = dBRecord.Name;
                    item.Price = dBRecord.Price;
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "BasketController:PopulateBasket - database context error");
                }

                basket.AddStockItem(item);
            }

            foreach (var item in basket.GetWholeBasket())
            {
                double subTotal = basket.GetSubTotal();
                subTotal += item.Price;
                if (item.Orders > 1)
                {
                    for (int i = 1; i < item.Orders; i++)
                    {
                        subTotal += item.Price;
                    }
                }
                basket.SetSubTotal(subTotal);
            }
        }

        /// <summary>
        /// Callback for the discount button when selected.
        /// Will calculate a discounted basket price if a valid code has been used.
        /// </summary>
        /// <param name="discountCode">a user entered code to check</param>
        /// <returns><see cref="IActionResult"/>A basket model with a discounted price added</returns>
        [Route("{Controller}/{Action}/{discountCode}")]
        public IActionResult DiscountButtonClick(string discountCode)
        {
            CheckOutModel basketData = new CheckOutModel();

            PopulateBasket(basketData);

            double subTotal = basketData.GetSubTotal();
            if (discountCode == _configuration.GetSection("DiscountCode").Value)
            {
                if (subTotal > 1)
                {
                    basketData.SetDiscountedPrice(subTotal - subTotal / 100 * 15);
                }
            }
            else
            {
                basketData.SetDiscountedPrice(subTotal);
            }
            
            return View("Basket", basketData);
        }

        /// <summary>
        /// Callback for the remove item from basket button.
        /// Will decrement a multiple basket item or remove the item from the sessions storage.
        /// </summary>
        /// <param name="itemToDelete">The serial code of the item in the basket</param>
        /// <returns><see cref="IActionResult"/>A basket model with a discounted price added</returns>
        [Route("{Controller}/{Action}/{itemToDelete}")]
        public IActionResult RemoveItemButtonClick(string itemToDelete)
        {
            bool deleteFlag = false;
            bool modifyFlag = false;
            int increment = 0;

            foreach (string key in _accessor.HttpContext.Session.Keys)
            {
                if (key == itemToDelete)
                {
                    increment = (int)_accessor.HttpContext.Session.GetInt32(itemToDelete);
                    if (increment > 1)
                    {
                        modifyFlag = true;
                    }
                    else
                    {
                        deleteFlag = true;
                    }
                }
            }

            //These can't be done inside of a for loop over the collection, as it will change it
            if (modifyFlag)
            {
                _accessor.HttpContext.Session.SetInt32(itemToDelete, --increment);
            }
            if (deleteFlag)
            {
                _accessor.HttpContext.Session.Remove(itemToDelete);
            }

            CheckOutModel basketData = new CheckOutModel();

            PopulateBasket(basketData);

            return View("Basket", basketData);
        }

        /// <summary>
        /// Callback to count the number of items in the basket currently.
        /// Uses session storage for the basket.
        /// </summary>
        /// <returns><see cref="int"/>The number of items found in session storage</returns>
        [HttpGet]
        public int CountBasketItems()
        {
            int count = 0;
            foreach (string key in _accessor.HttpContext.Session.Keys)
            {
                count++;
                int multipleItems = (int)_accessor.HttpContext.Session.GetInt32(key);
                if (multipleItems > 1)
                {
                    for (int i = 1; i < multipleItems; i++)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
