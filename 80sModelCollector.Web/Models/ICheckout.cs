using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _80sModelCollector.Data;

namespace _80sModelCollector.Models
{
    /// <summary>
    /// A simple interface for the sake of showing one.
    /// There's not a lot of business logic or hierarchy of classes in this project
    /// </summary>
    interface ICheckout
    {
        bool AddStockItem(BasketItem basketItem);
        bool SetSubTotal(double subTotal);
        double GetSubTotal();
        bool SetDiscountedPrice(double value);
        double GetDiscountedPrice();
        List<BasketItem> GetWholeBasket();
    }

}
