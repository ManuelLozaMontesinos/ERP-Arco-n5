using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace ESD.ERP.Web.Infrastructure
{
   public static class UserPreferenceHelper
   {
      private static readonly ConcurrentDictionary<int, string> _themeCache = new ConcurrentDictionary<int, string>();
      private static bool _tableChecked = false;
      private static readonly object _tableLock = new object();

      private static string GetDbConnectionString()
      {
         return DatabaseConnectionHelper.GetConnectionString();
      }

      private static void EnsureTableExists(SqlConnection conn)
      {
         if (_tableChecked) return;
         lock (_tableLock)
         {
            if (_tableChecked) return;
            try
            {
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'usuario_preferencia')
                     BEGIN
                         CREATE TABLE dbo.usuario_preferencia (
                             usuario_id INT NOT NULL PRIMARY KEY,
                             tema VARCHAR(20) NOT NULL DEFAULT 'light',
                             fecha_modificacion DATETIME DEFAULT GETDATE()
                         );
                     END";
                  cmd.ExecuteNonQuery();
               }
               _tableChecked = true;
            }
            catch { }
         }
      }

      public static string ObtenerTemaUsuario(int usuarioId, HttpContext context = null)
      {
         if (usuarioId <= 0)
         {
            return "light";
         }

         // 1. Revisar cache en memoria
         if (_themeCache.TryGetValue(usuarioId, out string cachedTheme) && !string.IsNullOrEmpty(cachedTheme))
         {
            return cachedTheme;
         }

         // 2. Revisar cookie específica del usuario
         if (context != null && context.Request != null)
         {
            string cookieTheme = context.Request.Cookies["app_theme_user_" + usuarioId];
            if (!string.IsNullOrEmpty(cookieTheme))
            {
               cookieTheme = cookieTheme.ToLower() == "dark" ? "dark" : "light";
               _themeCache[usuarioId] = cookieTheme;
               return cookieTheme;
            }
         }

         // 3. Consultar base de datos
         string tema = "light";
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               EnsureTableExists(conn);

               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = "SELECT tema FROM dbo.usuario_preferencia WHERE usuario_id = @usuarioId";
                  cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                  var val = cmd.ExecuteScalar();
                  if (val != null && val != DBNull.Value)
                  {
                     tema = val.ToString()?.Trim()?.ToLower() == "dark" ? "dark" : "light";
                  }
               }
            }
         }
         catch { }

         _themeCache[usuarioId] = tema;
         return tema;
      }

      public static void GuardarTemaUsuario(int usuarioId, string tema, HttpContext context = null)
      {
         if (usuarioId <= 0) return;

         tema = tema?.Trim()?.ToLower() == "dark" ? "dark" : "light";
         _themeCache[usuarioId] = tema;

         if (context != null && context.Response != null && !context.Response.HasStarted)
         {
            try
            {
               context.Response.Cookies.Append("app_theme_user_" + usuarioId, tema, new CookieOptions
               {
                  Expires = DateTimeOffset.UtcNow.AddYears(1),
                  Path = "/",
                  SameSite = SameSiteMode.Lax
               });
            }
            catch { }
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               EnsureTableExists(conn);

               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     IF EXISTS (SELECT 1 FROM dbo.usuario_preferencia WHERE usuario_id = @usuarioId)
                         UPDATE dbo.usuario_preferencia 
                         SET tema = @tema, fecha_modificacion = GETDATE() 
                         WHERE usuario_id = @usuarioId;
                     ELSE
                         INSERT INTO dbo.usuario_preferencia (usuario_id, tema, fecha_modificacion) 
                         VALUES (@usuarioId, @tema, GETDATE());";

                  cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                  cmd.Parameters.AddWithValue("@tema", tema);
                  cmd.ExecuteNonQuery();
               }
            }
         }
         catch { }
      }
   }
}
