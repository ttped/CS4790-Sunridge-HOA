using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue, string modelUsed)
        {
            switch (modelUsed)
            {
                case "ClassifiedCategory":
                    return from item in items
                           select new SelectListItem
                           {
                               Text = item.GetPropertyValue("ClassifiedCategoryId"),
                               Value = item.GetPropertyValue("ClassifiedCategoryId"),
                               Selected = item.GetPropertyValue("ClassifiedCategoryId").Equals(selectedValue.ToString())
                           };

                case "Owner":
                    return from item in items
                           select new SelectListItem
                           {
                               Text = item.GetPropertyValue("FirstName") + " " + item.GetPropertyValue("LastName"),
                               Value = item.GetPropertyValue("OwnerId"),
                               Selected = item.GetPropertyValue("OwnerId").Equals(selectedValue.ToString())
                           };
                case "Key":
                    return from item in items
                           select new SelectListItem
                           {
                               Text = item.GetPropertyValue("KeyId"),
                               Value = item.GetPropertyValue("KeyId"),
                               Selected = item.GetPropertyValue("KeyId").Equals(selectedValue.ToString())
                           };
                default:
                    return from item in items
                           select new SelectListItem
                           {
                               Text = "Error",
                               Value = "Error",
                               Selected = item.Equals(null)
                           };

            } 
        }

        public static IEnumerable<SelectListItem> ownerNameToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("FirstName") + " " + item.GetPropertyValue("LastName"),
                       Value = item.GetPropertyValue("OwnerId"),
                       Selected = item.GetPropertyValue("OwnerId").Equals(selectedValue.ToString())
                   };
        }

        public static IEnumerable<SelectListItem> keyIdToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("SerialNumber"),
                       Value = item.GetPropertyValue("KeyId"),
                       Selected = item.GetPropertyValue("KeyId").Equals(selectedValue.ToString())
                   };
        }

    }
}
