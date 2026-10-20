using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ESD.ERP.Web.Controllers
{
   public class CalendarioController : AuthorizedController
   {
      private static string _cachedConnectionString;

      public static string GetDbConnectionString()
      {
         if (!string.IsNullOrEmpty(_cachedConnectionString))
         {
            return _cachedConnectionString;
         }

         string[] candidates = new[]
         {
            "Server=localhost;Database=esderp_v6;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=3;",
            "Server=localhost;Database=esderp_v6;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=3;",
            "Server=.;Database=esderp_v6;User Id=sa;Password=admin123*;TrustServerCertificate=True;Connect Timeout=3;"
         };

         foreach (var cs in candidates)
         {
            try
            {
               using (var testConn = new SqlConnection(cs))
               {
                  testConn.Open();
                  _cachedConnectionString = cs;
                  return cs;
               }
            }
            catch { }
         }
         return candidates[0];
      }

      [HttpGet]
      [HttpPost]
      public IActionResult Index(string cid)
      {
         ViewBag.cid = cid;
         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();
         ViewBag.Usuario = usuarioDTO?.UsuarioLogon ?? "administrador";
         ViewBag.UsuarioId = usuarioDTO?.UsuarioId ?? 1;
         ViewBag.EsAdmin = EsAdministrador();

         if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
         {
            return PartialView();
         }
         return View();
      }

      [HttpGet]
      [HttpPost]
      public JsonResult ObtenerEventos()
      {
         var eventos = new List<object>();
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT evento_id, usuario_id, usuario_logon, titulo, contexto, 
                            fecha_inicio, fecha_fin, todo_el_dia, color, fecha_creacion
                     FROM calendario_evento
                     WHERE estado = 'A'
                     ORDER BY fecha_inicio ASC";

                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        int id = Convert.ToInt32(reader["evento_id"]);
                        string titulo = reader["titulo"].ToString();
                        string contexto = reader["contexto"] != DBNull.Value ? reader["contexto"].ToString() : "";
                        DateTime fInicio = Convert.ToDateTime(reader["fecha_inicio"]);
                        DateTime? fFin = reader["fecha_fin"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["fecha_fin"]) : null;
                        bool todoElDia = Convert.ToBoolean(reader["todo_el_dia"]);
                        string color = reader["color"].ToString();
                        string usuario = reader["usuario_logon"].ToString();
                        DateTime fCreacion = Convert.ToDateTime(reader["fecha_creacion"]);

                        eventos.Add(new
                        {
                           id = id.ToString(),
                           title = titulo,
                           start = todoElDia ? fInicio.ToString("yyyy-MM-dd") : fInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                           end = fFin.HasValue ? (todoElDia ? fFin.Value.ToString("yyyy-MM-dd") : fFin.Value.ToString("yyyy-MM-ddTHH:mm:ss")) : null,
                           allDay = todoElDia,
                           backgroundColor = color,
                           borderColor = color,
                           extendedProps = new
                           {
                              contexto = contexto,
                              usuario = usuario,
                              fechaCreacion = fCreacion.ToString("dd/MM/yyyy HH:mm")
                           }
                        });
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message, data = eventos });
         }

         return Json(eventos);
      }

      public class EventoInputModel
      {
         public int? id { get; set; }
         public string titulo { get; set; }
         public string contexto { get; set; }
         public string fechaInicio { get; set; }
         public string fechaFin { get; set; }
         public bool todoElDia { get; set; } = true;
         public string color { get; set; } = "#0d6efd";
      }

      [HttpPost]
      public JsonResult GuardarEvento([FromBody] EventoInputModel model)
      {
         if (model == null || string.IsNullOrWhiteSpace(model.titulo) || string.IsNullOrWhiteSpace(model.fechaInicio))
         {
            return Json(new { success = false, message = "El título y la fecha son obligatorios." });
         }

         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();
         int usuarioId = usuarioDTO?.UsuarioId ?? 1;
         string usuarioLogon = usuarioDTO?.UsuarioLogon ?? "administrador";

         DateTime fInicio;
         if (!DateTime.TryParse(model.fechaInicio, out fInicio))
         {
            return Json(new { success = false, message = "Formato de fecha de inicio inválido." });
         }

         DateTime? fFin = null;
         if (!string.IsNullOrWhiteSpace(model.fechaFin) && DateTime.TryParse(model.fechaFin, out DateTime parsedFin))
         {
            fFin = parsedFin;
         }

         int eventoIdGuardado = 0;

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  if (model.id.HasValue && model.id.Value > 0)
                  {
                     cmd.CommandText = @"
                        UPDATE calendario_evento
                        SET titulo = @titulo,
                            contexto = @contexto,
                            fecha_inicio = @fInicio,
                            fecha_fin = @fFin,
                            todo_el_dia = @todoElDia,
                            color = @color,
                            fecha_modifica = GETDATE()
                        WHERE evento_id = @id AND estado = 'A'";
                     cmd.Parameters.AddWithValue("@id", model.id.Value);
                     eventoIdGuardado = model.id.Value;
                  }
                  else
                  {
                     cmd.CommandText = @"
                        INSERT INTO calendario_evento (usuario_id, usuario_logon, titulo, contexto, fecha_inicio, fecha_fin, todo_el_dia, color, estado, fecha_creacion)
                        OUTPUT INSERTED.evento_id
                        VALUES (@usuarioId, @usuarioLogon, @titulo, @contexto, @fInicio, @fFin, @todoElDia, @color, 'A', GETDATE())";
                     cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                     cmd.Parameters.AddWithValue("@usuarioLogon", usuarioLogon);
                  }

                  cmd.Parameters.AddWithValue("@titulo", model.titulo.Trim());
                  cmd.Parameters.AddWithValue("@contexto", (object)model.contexto?.Trim() ?? DBNull.Value);
                  cmd.Parameters.AddWithValue("@fInicio", fInicio);
                  cmd.Parameters.AddWithValue("@fFin", (object)fFin ?? DBNull.Value);
                  cmd.Parameters.AddWithValue("@todoElDia", model.todoElDia);
                  cmd.Parameters.AddWithValue("@color", string.IsNullOrWhiteSpace(model.color) ? "#0d6efd" : model.color);

                  if (model.id.HasValue && model.id.Value > 0)
                  {
                     cmd.ExecuteNonQuery();
                  }
                  else
                  {
                     eventoIdGuardado = Convert.ToInt32(cmd.ExecuteScalar());
                  }
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al guardar evento: " + ex.Message });
         }

         return Json(new { success = true, id = eventoIdGuardado, message = "Evento guardado exitosamente." });
      }

      [HttpPost]
      public JsonResult EliminarEvento(int id)
      {
         if (id <= 0)
         {
            return Json(new { success = false, message = "ID de evento no válido." });
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     UPDATE calendario_evento
                     SET estado = 'I',
                         fecha_modifica = GETDATE()
                     WHERE evento_id = @id";
                  cmd.Parameters.AddWithValue("@id", id);
                  cmd.ExecuteNonQuery();
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al eliminar evento: " + ex.Message });
         }

         return Json(new { success = true, message = "Evento eliminado exitosamente." });
      }
   }
}
