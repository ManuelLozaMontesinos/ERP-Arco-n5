using System;
using System.Reflection;
using System.Resources;
using System.Web;

namespace ESD.ERP.WebApi.Infrastructure
{
   public sealed class SR
   {
      private static SR loader = null;
      private ResourceManager resourceManager = null;

      private SR()
      {
         Type type = GetType();
         Assembly assembly = type.Assembly;

         string resourceName = "ESD.ERP.WebApi.Infrastructure.Resources.Strings";

         resourceManager = new ResourceManager(resourceName, assembly);
      }
      private static SR GetLoader()
      {
         if (loader == null)
         {
            lock (typeof(SR))
            {
               if (loader == null)
                  loader = new SR();
            }
         }
         return loader;
      }
      public static string GetString(string key)
      {
         return getStringHelper(key, null);
      }
      public static string GetString(string key, params object[] args)
      {
         return getStringHelper(key, args);
      }
      private static string getStringHelper(string key, params object[] args)
      {

         string format = string.Empty;
         string formattedString = string.Empty;

         format = GetLoader().resourceManager.GetString(key);

         if (args != null && args.Length > 0)
         {
            if (format.Length > 2)
               formattedString = string.Format(format, args);
         }

         if (string.IsNullOrEmpty(formattedString))
            formattedString = format;

         return HttpUtility.HtmlDecode(formattedString);
      }
   }
}