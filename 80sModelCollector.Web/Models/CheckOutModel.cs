using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _80sModelCollector.Data;

namespace _80sModelCollector.Models
{
    /// <summary>
    /// A CheckOut model adding pricing functionality to the Basket Model
    /// </summary>
    public class CheckOutModel : BasketModel, ICheckout
    {
        private double _subTotal;
        private double _discountTotal;

        /// <summary>
        /// Simple mutator for the SubTotal
        /// </summary>
        /// <param name="value">A value to set as the SubTotal</param>
        /// <returns><see cref="bool"/>Will only return true if the passed in value is 0 or positive</returns>
        public bool SetSubTotal(double value)
        {
            bool success = false;

            if (value > -1)
            {
                _subTotal = value;
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Simple accessor for the SubTotal. Could have just used a {get;} but for variation I did this!
        /// </summary>
        /// <returns><see cref="double"/>The subTotal value</returns>
        public double GetSubTotal()
        {
            return _subTotal;
        }

        /// <summary>
        /// Simple mutator for the Discounted Price
        /// </summary>
        /// <param name="value">A value to set as the Discounted Price</param>
        /// <returns><see cref="bool"/>Will only return true if the passed in value is 0 or positive</returns>
        public bool SetDiscountedPrice(double value)
        {
            bool success = false;

            if (value > -1)
            {
                _discountTotal = value;
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Accessor for the DiscountedPrice. 
        /// </summary>
        /// <returns><see cref="double"/>The discounted price or subtotal value if there is no discount</returns>
        public double GetDiscountedPrice()
        {
            return (_discountTotal == 0.0 ? _subTotal : _discountTotal); 
        }

    }

}
