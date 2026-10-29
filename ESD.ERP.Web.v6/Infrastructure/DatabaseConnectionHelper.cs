using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ESD.ERP.Web.Infrastructure
{
   public static class DatabaseConnectionHelper
   {
      private static string _cachedConnectionString;
      private static readonly object _lock = new object();

      public static string GetConnectionString(IConfiguration configuration = null)
      {
         if (!string.IsNullOrEmpty(_cachedConnectionString))
         {
            return _cachedConnectionString;
         }

         lock (_lock)
         {
            if (!string.IsNullOrEmpty(_cachedConnectionString))
            {
               return _cachedConnectionString;
            }

            // 1. Intentar desde IConfiguration (appsettings.json / variables de entorno)
            string cs = configuration?.GetConnectionString("DefaultConnection")
                     ?? configuration?.GetConnectionString("Default");

            if (!string.IsNullOrWhiteSpace(cs) && ProbarConexion(cs))
            {
               _cachedConnectionString = cs;
               return cs;
            }

            // 2. Intentar desde connectionStrings.xml (estándar nativo de ESD ERP)
            string xmlCs = LeerConnectionStringXml();
            if (!string.IsNullOrWhiteSpace(xmlCs) && ProbarConexion(xmlCs))
            {
               _cachedConnectionString = xmlCs;
               return xmlCs;
            }

            // 3. Si la conexión de IConfiguration estaba definida (aunque falle en pre-apertura inmediata), usarla
            if (!string.IsNullOrWhiteSpace(cs))
            {
               _cachedConnectionString = cs;
               return cs;
            }

            // 4. Probar candidatos estándar para entorno local/desarrollo
            string[] candidates = new[]
            {
               "Server=localhost;Database=esderp_v6;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=3;",
               "Server=.;Database=esderp_v6;User Id=sa;Password=admin123*;TrustServerCertificate=True;Connect Timeout=3;"
            };

            foreach (var cand in candidates)
            {
               if (ProbarConexion(cand))
               {
                  _cachedConnectionString = cand;
                  return cand;
               }
            }

            _cachedConnectionString = candidates[0];
            return _cachedConnectionString;
         }
      }

      private static bool ProbarConexion(string connectionString)
      {
         try
         {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            return true;
         }
         catch
         {
            return false;
         }
      }

      private static string LeerConnectionStringXml()
      {
         try
         {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string currDir = Directory.GetCurrentDirectory();

            string[] possiblePaths = new[]
            {
               Path.Combine(baseDir, "connectionStrings.xml"),
               Path.Combine(currDir, "connectionStrings.xml"),
               Path.Combine(baseDir, "..", "connectionStrings.xml"),
               Path.Combine(baseDir, "..", "ESD.ERP.WebApi.v6", "connectionStrings.xml"),
               Path.Combine(baseDir, "..", "ESD.ERP.ExportDataApi.v6", "connectionStrings.xml"),
               Path.Combine(currDir, "..", "ESD.ERP.WebApi.v6", "connectionStrings.xml"),
               Path.Combine(currDir, "..", "ESD.ERP.ExportDataApi.v6", "connectionStrings.xml")
            };

            foreach (var path in possiblePaths)
            {
               if (File.Exists(path))
               {
                  XDocument doc = XDocument.Load(path);
                  var elem = doc.Descendants("connectionString")
                                .FirstOrDefault(x => string.Equals(x.Attribute("name")?.Value, "default", StringComparison.OrdinalIgnoreCase));
                  string val = elem?.Attribute("value")?.Value;
                  if (!string.IsNullOrWhiteSpace(val))
                  {
                     return val;
                  }
               }
            }
         }
         catch { }

         return null;
      }
   }
}

