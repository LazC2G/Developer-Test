using System;
using System.Collections.Generic;
using System.Text;

namespace _80sModelCollector.Data
{
    /// <summary>
    /// A class representing a single basket item.
    /// It differs from the stock table in the database due to not all of the fields being necessary for the basket use,
    /// and this contains value representing the number of orders for an item.
    /// </summary>
    public class BasketItem
    {
        public string SerialNumber { get; set; }
        public int Orders { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }

    }
}
