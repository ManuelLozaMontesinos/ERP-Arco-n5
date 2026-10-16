using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESD.Core.Web
{

   public class WebHelper
   {
      public static List<SelectListItem> ToSelectListItem<T>(IEnumerable<T> enumerable, Func<T, string> getValue, Func<T, string> getText,
         SelectListFirstOptionType? firstOption)
      {
         return ToSelectListItem<T>(enumerable, getValue, getText, firstOption, string.Empty);
      }
      public static List<SelectListItem> ToSelectListItem<T>(IEnumerable<T> enumerable, Func<T, string> getValue, Func<T, string> getText,
         SelectListFirstOptionType? firstOption, string selectedValue)
      {
         List<SelectListItem> selectedList = new List<SelectListItem>();

         if (!firstOption.HasValue)
            firstOption = SelectListFirstOptionType.Empty;

         string firtsText = SelectListFirstOptionNameConstant.BLANK;
         switch (firstOption)
         {
            case SelectListFirstOptionType.Blank:
               break;
            case SelectListFirstOptionType.Select:
               firtsText = SelectListFirstOptionNameConstant.SELECT;
               break;
            case SelectListFirstOptionType.All:
               firtsText = SelectListFirstOptionNameConstant.ALL;
               break;
         }

         if (firstOption.Value != SelectListFirstOptionType.Empty) {
            selectedList.Add(new SelectListItem
            {
               Text = firtsText,
               Value = string.Empty,
               Selected = (string.IsNullOrEmpty(selectedValue) ? true : false)
            });
         }

         if (enumerable != null)
         {
            var tempList = enumerable
              .Select(x => new SelectListItem
              {
                 Selected = (getValue(x) == selectedValue),
                 Text = getText(x),
                 Value = getValue(x)
              })
              .ToList();

            selectedList.AddRange(tempList);
         }

         return selectedList;
      }
      public static List<SelectListItem> ToSelectListItem(SelectListFirstOptionType firstOption)
      {
         List<SelectListItem> selectedList = new List<SelectListItem>();

         string firtsText = SelectListFirstOptionNameConstant.BLANK;
         switch (firstOption)
         {
            case SelectListFirstOptionType.Blank:
               break;
            case SelectListFirstOptionType.Select:
               firtsText = SelectListFirstOptionNameConstant.SELECT;
               break;
            case SelectListFirstOptionType.All:
               firtsText = SelectListFirstOptionNameConstant.ALL;
               break;
         }

         selectedList.Add(new SelectListItem
         {
            Text = firtsText,
            Value = string.Empty,
            Selected = true
         });

         return selectedList;
      }
      public static void SelectedListItemSetValue(List<SelectListItem> list, object value)
      {
         if (value == null)
            return;

         if (value == DBNull.Value)
            return;

         SelectListItem selectListItem = list.Where(x => x.Value == value.ToString())
            .FirstOrDefault();

         if (selectListItem != null)
            selectListItem.Selected = true;
      }
   }
}
