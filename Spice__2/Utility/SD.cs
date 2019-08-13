using Spice__2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice__2.Utility
{
    public static class SD
    {
        public const string DefaultFoodImage = "default_food.png";
        public const string ManegerUser = "Manager";
        public const string kitchenUser = "Kitchen";
        public const string FrontDeskUser = "FronDesk";
        public const string CustomerEndUser = "Customer";
        //-----------------------------------------------------------//
        public const string ssShoppingCartCount = "ssCartCount";
        public const string ssCouponCode = "ssCouponCode";
        //----------------------------------------------------------//

        public const string StatusSubmitted = "Submitted";
        public const string StatusInProcess = "Being Prepared";
        public const string StatusReady = "Ready for pickup";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Canceled";
        public const string PaymentPending = "Pending";
        public const string PaymentApproved = "Approved";
        public const string PaymentRejected = "Rejected";

        //----------------------------------------------------------//
        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        //---------------------------------------------------------------------------------//
        public static double DiscountedPrice(Coupon couponFromDb, double OriginalOrderTotal)
        {
            if (couponFromDb == null)
            {
                return OriginalOrderTotal;
            }
            else
            {
                if (couponFromDb.MinimumAmount > OriginalOrderTotal)
                {
                    return OriginalOrderTotal;
                }
                else
                {
                    /// everything is valid. 
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.Dollar) //--> We check if the type of coupon is Dollar. we have to declare like this because the in the model class the type in enum. 
                    {
                        //$10 off $100
                        return Math.Round(OriginalOrderTotal - couponFromDb.Discount, 2);
                    }

                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.Percent)
                    {
                        //1%0 off $100
                        return Math.Round(OriginalOrderTotal - (OriginalOrderTotal * couponFromDb.Discount / 100), 2);
                    }
                }
            }

            return OriginalOrderTotal; 
        }
        //---------------------------------------------------------------------------------//

    }
}
