using System;

namespace _80sModelCollector.Data
{
    /// <summary>
    /// An EF class representing the stock table from the database.
    /// </summary>
    public class Stock
    {
        public int SerialNumber { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public int RemainingStock { get; set; }
    }
}
