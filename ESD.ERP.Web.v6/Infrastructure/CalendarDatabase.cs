using System;
using Microsoft.Extensions.Configuration;

namespace ESD.ERP.Web.Infrastructure
{
   public static class CalendarDatabase
   {
      public static string GetConnectionString(IConfiguration configuration)
      {
         string connectionString = configuration?.GetConnectionString("Default");
         if (!string.IsNullOrWhiteSpace(connectionString))
         {
            return connectionString;
         }

         // Keep local development working when no environment-specific value is configured.
         return "Server=localhost;Database=esderp_v6;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=3;";
      }
   }
}
