using System.Text;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace ESD.Core.Web.JS
{
   public class GridHelper
   {
      public static HtmlString DefaultProperties()
      {
         return DefaultProperties(null);
      }
      public static HtmlString DefaultProperties(Dictionary<string,string> overwriteList) 
      {
         Dictionary<string, string> defaultProperties = new()
         {
            { "datatype", "'json'" },
            { "mtype", "'POST'" },
            { "loadonce", "true" },
            { "rowNum", "150" },
            { "rowList", "[150,300,500]" },
            { "sortorder", "'asc'" },
            { "locale", "'es'" },
            { "viewrecords", "true" },
            { "shrinkToFit", "false" },
            { "gridview", "true" },
            { "autoencode", "true" },
            { "emptyrows", "''" },
            { "autowidth", "true" },
            { "pgtext", "null" },
            { "styleUI", "'Bootstrap4'" },
            { "iconSet","'fontAwesome'"},
            { "responsive", "true" }
         };

         if (overwriteList != null) {
            foreach (var item in overwriteList) {
               if (defaultProperties.ContainsKey(item.Key))
                  defaultProperties[item.Key] = item.Value;
            }
         }

         StringBuilder sb = new();
         foreach (var item in defaultProperties)
         {
            if (!string.IsNullOrEmpty(item.Value))
               sb.AppendLine(item.Key + ": " + item.Value + ",");
         }

         return new HtmlString(HttpUtility.HtmlDecode(sb.ToString()));
      }

      public static HtmlString CreateEvent(string name, string parameters, string code) {

         StringBuilder sb = new();
         sb.AppendLine(name + ": function (" + parameters + ") {");
         sb.AppendLine("\t" + code);
         sb.AppendLine("},");

         return new HtmlString(HttpUtility.HtmlDecode(sb.ToString()));
      }
      public static HtmlString RowDeletedStyle(string column, string deletedValue)
      {
         StringBuilder sb = new();
         sb.AppendLine("rowattr: function (rd) {");
         sb.AppendLine("if (rd." + column + " == '" + deletedValue + "'){");
         sb.AppendLine("return { 'class': 'jqgrid-deletedrow' };");
         sb.AppendLine("}");
         sb.Append("},");

         return new HtmlString(HttpUtility.HtmlDecode(sb.ToString()));
      }

   }
}
