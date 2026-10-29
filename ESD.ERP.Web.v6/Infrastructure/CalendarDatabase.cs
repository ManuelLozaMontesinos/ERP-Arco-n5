using System;
using Microsoft.Extensions.Configuration;

namespace ESD.ERP.Web.Infrastructure
{
   public static class CalendarDatabase
   {
      public static string GetConnectionString(IConfiguration configuration)
      {
         return DatabaseConnectionHelper.GetConnectionString(configuration);
      }
   }
}
