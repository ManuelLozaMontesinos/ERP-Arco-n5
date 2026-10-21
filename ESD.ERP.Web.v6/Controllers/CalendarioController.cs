using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace ESD.ERP.Web.Controllers
{
   public class CalendarioController : AuthorizedController
   {
      private readonly IConfiguration configuration;

      public CalendarioController(IConfiguration configuration)
      {
         this.configuration = configuration;
      }

      private string GetDbConnectionString() => CalendarDatabase.GetConnectionString(configuration);

      [HttpGet]
      [HttpPost]
      public IActionResult Index(string cid)
      {
         ViewBag.cid = cid;
         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();
         int usuarioId = ObtenerUsuarioId();
         ViewBag.Usuario = usuarioDTO?.UsuarioLogon ?? string.Empty;
         ViewBag.UsuarioId = usuarioId;
         ViewBag.EsAdmin = usuarioId > 0 && usuarioDTO != null && EsAdministrador();

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
         int usuarioActualId = ObtenerUsuarioId();
         if (usuarioActualId <= 0)
         {
            return Json(new { success = false, message = "No se pudo identificar al usuario de la sesión.", data = eventos });
         }

         try
         {
            bool esAdmin = EsAdministrador();
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                      SELECT evento_id, usuario_id, usuario_logon, titulo, contexto,
                            fecha_inicio, fecha_fin, todo_el_dia, color, fecha_creacion
                      FROM dbo.calendario_evento
                     WHERE estado = 'A'
                     ORDER BY fecha_inicio ASC";

                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        int id = Convert.ToInt32(reader["evento_id"]);
                        int propietarioId = Convert.ToInt32(reader["usuario_id"]);
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
                           editable = esAdmin || propietarioId == usuarioActualId,
                           backgroundColor = color,
                           borderColor = color,
                           extendedProps = new
                           {
                              contexto = contexto,
                              usuario = usuario,
                              usuarioId = propietarioId,
                              puedeEditar = esAdmin || propietarioId == usuarioActualId,
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
         public bool todoElDia { get; set; }
         public string color { get; set; } = "#0d6efd";
      }

      [HttpPost]
      public JsonResult GuardarEvento([FromBody] EventoInputModel model)
      {
         if (model == null || string.IsNullOrWhiteSpace(model.titulo) || string.IsNullOrWhiteSpace(model.fechaInicio))
         {
            return Json(new { success = false, message = "El título y la fecha son obligatorios." });
         }

         if (model.titulo.Trim().Length > 150)
         {
            return Json(new { success = false, message = "El título no puede superar los 150 caracteres." });
         }

         string color = string.IsNullOrWhiteSpace(model.color) ? "#0d6efd" : model.color.Trim();
         if (!EsColorHexValido(color))
         {
            return Json(new { success = false, message = "El color del evento no es válido." });
         }

         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();
         int usuarioId = ObtenerUsuarioId();
         if (usuarioId <= 0 || usuarioDTO == null)
         {
            return Json(new { success = false, message = "No se pudo identificar al usuario de la sesión." });
         }

         string usuarioLogon = usuarioDTO.UsuarioLogon;

         DateTime fInicio;
         if (!DateTime.TryParse(model.fechaInicio, CultureInfo.InvariantCulture, DateTimeStyles.None, out fInicio)
            || (!model.todoElDia && model.fechaInicio.IndexOf('T') <= 0))
         {
            return Json(new { success = false, message = "Selecciona una fecha y hora de inicio válidas." });
         }

         DateTime? fFin = null;
         if (!string.IsNullOrWhiteSpace(model.fechaFin))
         {
            if (!DateTime.TryParse(model.fechaFin, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFin))
            {
               return Json(new { success = false, message = "Formato de fecha y hora de fin inválido." });
            }

            fFin = parsedFin;
            if (fFin <= fInicio)
            {
               return Json(new { success = false, message = "La fecha y hora de fin debe ser posterior al inicio." });
            }
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
                         UPDATE dbo.calendario_evento
                        SET titulo = @titulo,
                            contexto = @contexto,
                            fecha_inicio = @fInicio,
                            fecha_fin = @fFin,
                            todo_el_dia = @todoElDia,
                            color = @color,
                             recordatorio_estado = CASE WHEN fecha_inicio <> @fInicio OR todo_el_dia <> @todoElDia THEN 'P' ELSE recordatorio_estado END,
                             recordatorio_reclamado_hasta = CASE WHEN fecha_inicio <> @fInicio OR todo_el_dia <> @todoElDia THEN NULL ELSE recordatorio_reclamado_hasta END,
                             recordatorio_enviado_en = CASE WHEN fecha_inicio <> @fInicio OR todo_el_dia <> @todoElDia THEN NULL ELSE recordatorio_enviado_en END,
                            fecha_modifica = GETDATE()
                         WHERE evento_id = @id AND estado = 'A'
                           AND (usuario_id = @usuarioId OR @esAdmin = 1)";
                     cmd.Parameters.AddWithValue("@id", model.id.Value);
                     cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                     cmd.Parameters.AddWithValue("@esAdmin", EsAdministrador());
                     eventoIdGuardado = model.id.Value;
                  }
                  else
                  {
                     cmd.CommandText = @"
                         INSERT INTO dbo.calendario_evento (usuario_id, usuario_logon, titulo, contexto, fecha_inicio, fecha_fin, todo_el_dia, color, estado, fecha_creacion)
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
                  cmd.Parameters.AddWithValue("@color", color);

                  if (model.id.HasValue && model.id.Value > 0)
                  {
                     if (cmd.ExecuteNonQuery() == 0)
                     {
                        return Json(new { success = false, message = "No tienes permiso para modificar este evento o ya no está disponible." });
                     }
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
            int usuarioId = ObtenerUsuarioId();
            if (usuarioId <= 0)
            {
               return Json(new { success = false, message = "No se pudo identificar al usuario de la sesión." });
            }

            bool esAdmin = EsAdministrador();
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                      UPDATE dbo.calendario_evento
                     SET estado = 'I',
                         fecha_modifica = GETDATE()
                      WHERE evento_id = @id AND estado = 'A'
                        AND (usuario_id = @usuarioId OR @esAdmin = 1)";
                  cmd.Parameters.AddWithValue("@id", id);
                   cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                   cmd.Parameters.AddWithValue("@esAdmin", esAdmin);
                   if (cmd.ExecuteNonQuery() == 0)
                   {
                      return Json(new { success = false, message = "No tienes permiso para eliminar este evento o ya no está disponible." });
                   }
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al eliminar evento: " + ex.Message });
         }

         return Json(new { success = true, message = "Evento eliminado exitosamente." });
      }

      private static bool EsColorHexValido(string color)
      {
         if (color == null || color.Length != 7 || color[0] != '#')
         {
            return false;
         }

         for (int i = 1; i < color.Length; i++)
         {
            if (!Uri.IsHexDigit(color[i]))
            {
               return false;
            }
         }

         return true;
      }
   }
}
