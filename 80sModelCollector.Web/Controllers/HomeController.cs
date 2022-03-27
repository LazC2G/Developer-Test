using System;
using System.Collections.Generic;
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
    /// The Home Controller class looks after the aspects of the site to do with home page and it's list of stock.
    /// It makes use of a database which holds the stock items for sale and it places the selected purchases into
    /// session storage which acts as a basket for the app.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly CollectorStockContext _context;

        /// <summary>
        /// Constructor for the Home Controller class. 
        /// </summary>
        /// <param name="logger">ILogger object for errors</param>
        /// <param name="accessor">IHttpContextAccessor for session storage</param>
        /// <param name="context">CollectorStockContext for accessing the database</param>
        /// <returns></returns>
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor accessor, CollectorStockContext context)
        {
            _accessor = accessor;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Index endpoint for the app.
        /// It uses the stock table in the database as a direct model with no need for a separate class.
        /// </summary>
        /// <returns><see cref="IActionResult"/>For the view, containing stock model from the database</returns>
        public IActionResult Index()
        {
            List<Stock> data = new List<Stock>();

            try
            {
                data = _context.Stock.ToList();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "HomeController:Index - database context error");
            }

            return View(data);
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
        /// Login endpoint for a dummy page.
        /// Note: One for enhancing at a later date and moving to it's own controller.
        /// </summary>
        /// <returns><see cref="IActionResult"/>For the login view, with no model</returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Callback for the basket button when selected.
        /// Will add an item into the session storage, or if it is already there increment its value for
        /// additional orders of the same product.
        /// </summary>
        /// <param name="serialNumber">a serial number from the database to add into session storage</param>
        /// <returns><see cref="IActionResult"/>A stock model straight from the database</returns>
        [Route("{Controller}/{Action}/{serialNumber}")]
        public IActionResult BasketButtonClick(int serialNumber)
        {
            int increment = 1;
            bool foundItems = false;

            //potential for LINQ function here
            foreach (string key in _accessor.HttpContext.Session.Keys)
            {
                if (key == serialNumber.ToString())
                {
                    foundItems = true;
                }
            }

            if (foundItems)
            {
                increment += (int)_accessor.HttpContext.Session.GetInt32(serialNumber.ToString());
                _accessor.HttpContext.Session.SetInt32(serialNumber.ToString(), increment);
            }
            else
            {
                _accessor.HttpContext.Session.SetInt32(serialNumber.ToString(), increment);
            }

            List<Stock> data = new List<Stock>();

            try
            {
                data = _context.Stock.ToList();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "HomeController:BasketButtonClick - database context error");
            }

            return View("Index", data);
        }
    }
}
