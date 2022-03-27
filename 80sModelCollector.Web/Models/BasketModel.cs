using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _80sModelCollector.Data;

namespace _80sModelCollector.Models
{
    /// <summary>
    /// A Model for the basket functionality.
    /// You could argue that functionality should not be in here as the model should just be data, but it's only a little thing!
    /// </summary>
    public class BasketModel
    {
        private List<BasketItem> _basket;

        /// <summary>
        /// Constructor for the Basket model.
        /// Holds a list of Basket items.
        /// </summary>
        public BasketModel() 
        {
            _basket = new List<BasketItem>();
        }

        /// <summary>
        /// Add an item into the basket.
        /// </summary>
        /// <param name="basketItem">BasketItem object representing a application side item order</param>
        /// <returns><see cref="bool"/>Will only return true if the BasketItem is valid</returns>
        public bool AddStockItem(BasketItem basketItem)
        {
            bool success = false;

            if (basketItem.SerialNumber != null)
            {
                success = true;
                _basket.Add(basketItem);
            }

            return success;
        }

        /// <summary>
        /// Return the basket list.
        /// </summary>
        /// <returns><see cref="List<BasketItem>"/>A List of Basket Items</returns>
        public List<BasketItem> GetWholeBasket()
        {
            return _basket;
        }
    }

}
