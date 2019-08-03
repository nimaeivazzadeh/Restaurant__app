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

        public const string ssShoppingCartCount = "ssCartCount";
        public const string ssCouponCode = "ssCouponCode";

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
        //public static double DiscountedPrice(Coupon couponFromDb, double OriginalOrderTotal)
        //{
        //    if (couponFromDb == null)
        //    {
        //        return OriginalOrderTotal;
        //    }
        //    else
        //    {
        //        if (couponFromDb.MinimumAmount > OriginalOrderTotal)
        //        {
        //            return OriginalOrderTotal;
        //        }
        //        else
        //        {
        //            /// everything is valid.
                    
        //        }
        //    }
        //}

    }
}
