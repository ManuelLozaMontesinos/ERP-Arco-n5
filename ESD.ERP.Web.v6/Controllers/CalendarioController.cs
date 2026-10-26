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
using System.Linq;

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

      public class AreaItemModel
      {
         public int AreaId { get; set; }
         public string Nombre { get; set; }
         public string Descripcion { get; set; }
         public string Color { get; set; }
      }

      [HttpGet]
      [HttpPost]
      public IActionResult Index(string cid)
      {
         ViewBag.cid = cid;
         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();
         int usuarioId = ObtenerUsuarioId();
         bool esAdmin = usuarioId > 0 && usuarioDTO != null && EsAdministrador();
         ViewBag.Usuario = usuarioDTO?.UsuarioLogon ?? string.Empty;
         ViewBag.UsuarioId = usuarioId;
         ViewBag.EsAdmin = esAdmin;

         List<AreaItemModel> areas = ObtenerAreasUsuarioInterno(usuarioId, esAdmin);
         ViewBag.Areas = areas;
         ViewBag.TieneMultiplesAreas = esAdmin || (areas != null && areas.Count > 1);
         ViewBag.AreaSeleccionadaId = areas.Count > 0 ? areas[0].AreaId : 2;
         ViewBag.AreaSeleccionadaNombre = areas.Count > 0 ? areas[0].Nombre : "Ventas";
         bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                    || !string.IsNullOrEmpty(Request.Headers["x-app-cid"])
                    || string.Equals(Request.Method, "POST", StringComparison.OrdinalIgnoreCase);

         if (isAjax)
         {
            return PartialView();
         }

         return View();
      }

      private List<AreaItemModel> ObtenerAreasUsuarioInterno(int usuarioId, bool esAdmin)
      {
         var areas = new List<AreaItemModel>();
         if (usuarioId <= 0) return areas;

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  if (esAdmin)
                  {
                     cmd.CommandText = @"
                        SELECT area_id, nombre, descripcion, color 
                        FROM dbo.calendario_area 
                        WHERE estado = 'A' 
                        ORDER BY orden ASC, nombre ASC";
                  }
                  else
                  {
                     cmd.CommandText = @"
                        INSERT INTO dbo.calendario_usuario_area (usuario_id, area_id, es_principal, fecha_creacion)
                        SELECT DISTINCT @usuarioId, m.area_id, 0, SYSDATETIME()
                        FROM dbo.rol_usuario ru
                        JOIN dbo.rol r ON ru.rol_id = r.rol_id
                        CROSS APPLY (
                            SELECT area_id FROM dbo.calendario_area
                            WHERE estado = 'A' AND (
                                (area_id = 2 AND (r.nombre LIKE '%Venta%' OR r.nombre LIKE '%Comercial%' OR r.rol IN ('00003','00004','00010','00017')))
                                OR (area_id = 3 AND (r.nombre LIKE '%Market%' OR r.rol IN ('00015')))
                                OR (area_id = 4 AND (r.nombre LIKE '%Producci%' OR r.rol IN ('00023')))
                                OR (area_id = 5 AND (r.nombre LIKE '%Almac%' OR r.nombre LIKE '%Log%stica%' OR r.nombre LIKE '%Despacho%' OR r.nombre LIKE '%Operacion%' OR r.rol IN ('00007','00011','00013','00014','00016','00018','00019','00021','00022')))
                                OR (area_id = 6 AND (r.nombre LIKE '%Contab%' OR r.nombre LIKE '%Tesor%' OR r.nombre LIKE '%Caja%' OR r.rol IN ('00005','00006','00008','00012')))
                                OR (area_id = 7 AND (r.nombre LIKE '%Capital Humano%' OR r.nombre LIKE '%RRHH%' OR r.rol IN ('00009')))
                                OR (area_id = 8 AND (r.rol = '00020' OR r.nombre = 'TI' OR r.nombre LIKE '%TI %' OR r.nombre LIKE '% TI' OR r.nombre LIKE '%Sistemas%'))
                                OR (area_id = 1 AND (r.rol = 'S0001' OR r.nombre LIKE '%Admin%'))
                            )
                        ) m
                        WHERE ru.usuario_id = @usuarioId
                          AND NOT EXISTS (
                              SELECT 1 FROM dbo.calendario_usuario_area cua 
                              WHERE cua.usuario_id = @usuarioId AND cua.area_id = m.area_id
                          );

                        IF EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @usuarioId)
                           AND NOT EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @usuarioId AND es_principal = 1)
                        BEGIN
                            UPDATE TOP (1) dbo.calendario_usuario_area 
                            SET es_principal = 1 
                            WHERE usuario_id = @usuarioId;
                        END

                        SELECT a.area_id, a.nombre, a.descripcion, a.color 
                        FROM dbo.calendario_area a
                        INNER JOIN dbo.calendario_usuario_area cua ON a.area_id = cua.area_id
                        WHERE cua.usuario_id = @usuarioId AND a.estado = 'A'
                        ORDER BY cua.es_principal DESC, a.orden ASC, a.nombre ASC";
                     cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                  }

                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        areas.Add(new AreaItemModel
                        {
                           AreaId = Convert.ToInt32(reader["area_id"]),
                           Nombre = reader["nombre"].ToString(),
                           Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : "",
                           Color = reader["color"] != DBNull.Value ? reader["color"].ToString() : "#0d6efd"
                        });
                     }
                  }

                  if (!esAdmin && areas.Count == 0)
                  {
                     cmd.Parameters.Clear();
                     cmd.CommandText = @"
                        IF EXISTS (
                           SELECT 1 FROM dbo.rol_usuario ru
                           JOIN dbo.rol r ON ru.rol_id = r.rol_id
                           WHERE ru.usuario_id = @uid AND (r.rol IN ('00003','00004','00010') OR r.nombre LIKE '%Venta%')
                        )
                        BEGIN
                           IF NOT EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @uid AND area_id = 2)
                           BEGIN
                              INSERT INTO dbo.calendario_usuario_area (usuario_id, area_id, es_principal, fecha_creacion)
                              VALUES (@uid, 2, 1, GETDATE());
                           END
                        END

                        SELECT TOP 1 area_id, nombre, descripcion, color 
                        FROM dbo.calendario_area 
                        WHERE estado = 'A' AND (area_id = 2 OR nombre LIKE '%Venta%') 
                        ORDER BY area_id ASC";
                     cmd.Parameters.AddWithValue("@uid", usuarioId);
                     using (var reader = cmd.ExecuteReader())
                     {
                        if (reader.Read())
                        {
                           areas.Add(new AreaItemModel
                           {
                              AreaId = Convert.ToInt32(reader["area_id"]),
                              Nombre = reader["nombre"].ToString(),
                              Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : "",
                              Color = reader["color"] != DBNull.Value ? reader["color"].ToString() : "#198754"
                           });
                        }
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            ESD.Core.Logging.Logger.Error("Error al obtener áreas de usuario en Calendario: " + ex);
         }

         return areas;
      }

      [HttpGet]
      public JsonResult ObtenerAreas()
      {
         int usuarioId = ObtenerUsuarioId();
         bool esAdmin = EsAdministrador();
         var areas = ObtenerAreasUsuarioInterno(usuarioId, esAdmin);
         return Json(new { success = true, esAdmin = esAdmin, data = areas });
      }

      [HttpGet]
      [HttpPost]
      public JsonResult ObtenerEventos(int? areaId)
      {
         var eventos = new List<object>();
         int usuarioActualId = ObtenerUsuarioId();
         if (usuarioActualId <= 0)
         {
            return Json(new { success = false, message = "No se pudo identificar al usuario de la sesión.", data = eventos });
         }

         bool esAdmin = EsAdministrador();
         var areasPermitidas = ObtenerAreasUsuarioInterno(usuarioActualId, esAdmin);

         int idAreaFinal;
         if (esAdmin)
         {
            idAreaFinal = (areaId.HasValue && areaId.Value > 0)
               ? areaId.Value
               : (areasPermitidas.Count > 0 ? areasPermitidas[0].AreaId : 1);
         }
         else
         {
            if (areasPermitidas.Count == 0)
            {
               return Json(new { success = false, message = "No tienes un área asignada para ver el calendario.", data = eventos });
            }
            idAreaFinal = (areaId.HasValue && areaId.Value > 0 && areasPermitidas.Any(a => a.AreaId == areaId.Value))
               ? areaId.Value
               : areasPermitidas[0].AreaId;
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                      SELECT e.evento_id, e.area_id, e.usuario_id, e.usuario_logon, e.titulo, e.contexto,
                             e.fecha_inicio, e.fecha_fin, e.todo_el_dia, e.color, e.fecha_creacion,
                             ISNULL(a.nombre, 'General') AS area_nombre
                      FROM dbo.calendario_evento e
                      LEFT JOIN dbo.calendario_area a ON e.area_id = a.area_id
                      WHERE e.estado = 'A' AND e.area_id = @areaId
                      ORDER BY e.fecha_inicio ASC";
                  cmd.Parameters.AddWithValue("@areaId", idAreaFinal);

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
                        string areaNombre = reader["area_nombre"].ToString();
                        bool puedeEditar = esAdmin || areasPermitidas.Any(a => a.AreaId == idAreaFinal);

                        eventos.Add(new
                        {
                           id = id.ToString(),
                           title = titulo,
                           start = todoElDia ? fInicio.ToString("yyyy-MM-dd") : fInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                           end = fFin.HasValue ? (todoElDia ? fFin.Value.ToString("yyyy-MM-dd") : fFin.Value.ToString("yyyy-MM-ddTHH:mm:ss")) : null,
                           allDay = todoElDia,
                           editable = puedeEditar,
                           backgroundColor = color,
                           borderColor = color,
                           textColor = (color == "#ffc107") ? "#212529" : "#ffffff",
                           extendedProps = new
                           {
                              contexto = contexto,
                              usuario = usuario,
                              usuarioId = propietarioId,
                              areaId = idAreaFinal,
                              areaNombre = areaNombre,
                              puedeEditar = puedeEditar,
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
         public int? areaId { get; set; }
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
         bool esAdmin = EsAdministrador();

         var areasPermitidas = ObtenerAreasUsuarioInterno(usuarioId, esAdmin);
         int areaIdFinal;
         if (esAdmin)
         {
            areaIdFinal = (model.areaId.HasValue && model.areaId.Value > 0) ? model.areaId.Value : 1;
         }
         else
         {
            if (areasPermitidas.Count == 0)
            {
               return Json(new { success = false, message = "No tienes permiso para registrar o modificar eventos." });
            }
            areaIdFinal = (model.areaId.HasValue && model.areaId.Value > 0 && areasPermitidas.Any(a => a.AreaId == model.areaId.Value))
               ? model.areaId.Value
               : areasPermitidas[0].AreaId;
         }

         DateTime fInicio;
         string fInicioStr = (model.fechaInicio ?? "").Trim();
         if (!model.todoElDia && fInicioStr.Length == 10 && fInicioStr.IndexOf('T') < 0)
         {
            fInicioStr += "T09:00:00";
         }

         if (!DateTime.TryParse(fInicioStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out fInicio) &&
             !DateTime.TryParse(fInicioStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out fInicio))
         {
            return Json(new { success = false, message = "Selecciona una fecha y hora de inicio válidas." });
         }

         DateTime? fFin = null;
         if (!string.IsNullOrWhiteSpace(model.fechaFin))
         {
            string fFinStr = model.fechaFin.Trim();
            if (!model.todoElDia && fFinStr.Length == 10 && fFinStr.IndexOf('T') < 0)
            {
               fFinStr += "T10:00:00";
            }

            if (!DateTime.TryParse(fFinStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFin) &&
                !DateTime.TryParse(fFinStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedFin))
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
                     cmd.CommandText = "SELECT area_id FROM dbo.calendario_evento WHERE evento_id = @id AND estado = 'A'";
                     cmd.Parameters.AddWithValue("@id", model.id.Value);
                     object existingAreaObj = cmd.ExecuteScalar();
                     if (existingAreaObj == null || existingAreaObj == DBNull.Value)
                     {
                        return Json(new { success = false, message = "El evento no existe o ya no está disponible." });
                     }
                     int existingAreaId = Convert.ToInt32(existingAreaObj);

                     if (!esAdmin && !areasPermitidas.Any(a => a.AreaId == existingAreaId))
                     {
                        return Json(new { success = false, message = "No tienes permiso para modificar eventos de esta área." });
                     }

                     cmd.Parameters.Clear();
                     cmd.CommandText = @"
                         UPDATE dbo.calendario_evento
                         SET titulo = @titulo,
                             contexto = @contexto,
                             fecha_inicio = @fInicio,
                             fecha_fin = @fFin,
                             todo_el_dia = @todoElDia,
                             color = @color,
                             area_id = COALESCE(@areaId, area_id),
                             recordatorio_estado = CASE WHEN fecha_inicio <> @fInicio OR todo_el_dia <> @todoElDia OR area_id <> @areaId THEN 'P' ELSE recordatorio_estado END,
                             recordatorio_reclamado_hasta = CASE WHEN fecha_inicio <> @fInicio OR todo_el_dia <> @todoElDia OR area_id <> @areaId THEN NULL ELSE recordatorio_reclamado_hasta END,
                             recordatorio_enviado_en = CASE WHEN fecha_inicio <> @fInicio OR todo_el_dia <> @todoElDia OR area_id <> @areaId THEN NULL ELSE recordatorio_enviado_en END,
                             fecha_modifica = GETDATE()
                         WHERE evento_id = @id AND estado = 'A'";
                     cmd.Parameters.AddWithValue("@id", model.id.Value);
                     cmd.Parameters.AddWithValue("@areaId", areaIdFinal);
                     eventoIdGuardado = model.id.Value;
                  }
                  else
                  {
                     cmd.CommandText = @"
                         INSERT INTO dbo.calendario_evento (area_id, usuario_id, usuario_logon, titulo, contexto, fecha_inicio, fecha_fin, todo_el_dia, color, estado, fecha_creacion)
                         OUTPUT INSERTED.evento_id
                         VALUES (@areaId, @usuarioId, @usuarioLogon, @titulo, @contexto, @fInicio, @fFin, @todoElDia, @color, 'A', GETDATE())";
                     cmd.Parameters.AddWithValue("@areaId", areaIdFinal);
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
                        return Json(new { success = false, message = "No se pudo actualizar el evento." });
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
            var areasPermitidas = ObtenerAreasUsuarioInterno(usuarioId, esAdmin);

            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = "SELECT area_id FROM dbo.calendario_evento WHERE evento_id = @id AND estado = 'A'";
                  cmd.Parameters.AddWithValue("@id", id);
                  object existingAreaObj = cmd.ExecuteScalar();
                  if (existingAreaObj == null || existingAreaObj == DBNull.Value)
                  {
                     return Json(new { success = false, message = "El evento no existe o ya no está disponible." });
                  }
                  int existingAreaId = Convert.ToInt32(existingAreaObj);

                  if (!esAdmin && !areasPermitidas.Any(a => a.AreaId == existingAreaId))
                  {
                     return Json(new { success = false, message = "No tienes permiso para eliminar eventos de esta área." });
                  }

                  cmd.Parameters.Clear();
                  cmd.CommandText = @"
                      UPDATE dbo.calendario_evento
                      SET estado = 'I',
                          fecha_modifica = GETDATE()
                      WHERE evento_id = @id AND estado = 'A'";
                  cmd.Parameters.AddWithValue("@id", id);
                  if (cmd.ExecuteNonQuery() == 0)
                  {
                     return Json(new { success = false, message = "No se pudo eliminar el evento." });
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

      // =========================================================================
      // Gestión de Etiquetas Arrastrables Personalizables por Área
      // =========================================================================
      public class EtiquetaInputModel
      {
         public int? areaId { get; set; }
         public int slotIndex { get; set; }
         public string titulo { get; set; }
         public string color { get; set; }
      }

      [HttpGet]
      public JsonResult ObtenerEtiquetas(int? areaId)
      {
         int usuarioId = ObtenerUsuarioId();
         var list = new List<object>();
         if (usuarioId <= 0) return Json(new { success = true, data = list });

         bool esAdmin = EsAdministrador();
         var areasPermitidas = ObtenerAreasUsuarioInterno(usuarioId, esAdmin);
         int areaIdFinal;
         if (esAdmin)
         {
            areaIdFinal = (areaId.HasValue && areaId.Value > 0)
               ? areaId.Value
               : (areasPermitidas.Count > 0 ? areasPermitidas[0].AreaId : 1);
         }
         else
         {
            if (areasPermitidas.Count == 0)
            {
               return Json(new { success = false, message = "No tienes un área asignada.", areaId = 2, data = list });
            }
            areaIdFinal = (areaId.HasValue && areaId.Value > 0 && areasPermitidas.Any(a => a.AreaId == areaId.Value))
               ? areaId.Value
               : areasPermitidas[0].AreaId;
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = "SELECT slot_index, titulo, color FROM dbo.calendario_etiqueta WHERE area_id = @areaId ORDER BY slot_index";
                  cmd.Parameters.AddWithValue("@areaId", areaIdFinal);
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        list.Add(new
                        {
                           slotIndex = Convert.ToInt32(reader["slot_index"]),
                           titulo = reader["titulo"] != DBNull.Value ? reader["titulo"].ToString() : "",
                           color = reader["color"].ToString()
                        });
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            ESD.Core.Logging.Logger.Error("Error al obtener etiquetas en Calendario: " + ex);
         }

         return Json(new { success = true, areaId = areaIdFinal, data = list });
      }

      [HttpPost]
      public JsonResult GuardarEtiqueta([FromBody] EtiquetaInputModel model)
      {
         int usuarioId = ObtenerUsuarioId();
         if (usuarioId <= 0 || model == null || model.slotIndex < 0 || model.slotIndex > 10)
         {
            return Json(new { success = false, message = "Datos inválidos para la etiqueta." });
         }

         bool esAdmin = EsAdministrador();
         var areasPermitidas = ObtenerAreasUsuarioInterno(usuarioId, esAdmin);
         int areaIdFinal;
         if (esAdmin)
         {
            areaIdFinal = (model.areaId.HasValue && model.areaId.Value > 0)
               ? model.areaId.Value
               : (areasPermitidas.Count > 0 ? areasPermitidas[0].AreaId : 1);
         }
         else
         {
            if (areasPermitidas.Count == 0)
            {
               return Json(new { success = false, message = "No tienes permiso para editar eventos de esta área." });
            }
            areaIdFinal = (model.areaId.HasValue && model.areaId.Value > 0 && areasPermitidas.Any(a => a.AreaId == model.areaId.Value))
               ? model.areaId.Value
               : areasPermitidas[0].AreaId;
         }

         string color = string.IsNullOrWhiteSpace(model.color) ? "#0d6efd" : model.color.Trim();
         string titulo = string.IsNullOrWhiteSpace(model.titulo) ? "" : model.titulo.Trim();

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     MERGE dbo.calendario_etiqueta AS target
                     USING (SELECT @areaId AS area_id, @slotIndex AS slot_index) AS source
                     ON (target.area_id = source.area_id AND target.slot_index = source.slot_index)
                     WHEN MATCHED THEN
                         UPDATE SET titulo = @titulo, color = @color, usuario_id = @usuarioId, fecha_modifica = GETDATE()
                     WHEN NOT MATCHED THEN
                         INSERT (usuario_id, area_id, slot_index, titulo, color, fecha_modifica)
                         VALUES (@usuarioId, @areaId, @slotIndex, @titulo, @color, GETDATE());";
                  cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                  cmd.Parameters.AddWithValue("@areaId", areaIdFinal);
                  cmd.Parameters.AddWithValue("@slotIndex", model.slotIndex);
                  cmd.Parameters.AddWithValue("@titulo", (object)titulo ?? DBNull.Value);
                  cmd.Parameters.AddWithValue("@color", color);
                  cmd.ExecuteNonQuery();
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al guardar etiqueta: " + ex.Message });
         }

         return Json(new { success = true, areaId = areaIdFinal, message = "Evento arrastrable guardado exitosamente para el área." });
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

      #region Administracion de Areas y Usuarios (Solo Administradores)

      public class AreaAdminInputModel
      {
         public int AreaId { get; set; }
         public string Nombre { get; set; }
         public string Descripcion { get; set; }
         public string Color { get; set; }
         public int Orden { get; set; }
         public string Estado { get; set; }
      }

      [HttpGet]
      public JsonResult AdminObtenerAreas()
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         var list = new List<object>();
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT a.area_id, a.nombre, a.descripcion, a.color, a.orden, a.estado, a.fecha_creacion,
                            COUNT(DISTINCT cua.usuario_id) AS total_usuarios,
                            COUNT(DISTINCT e.evento_id) AS total_eventos
                     FROM dbo.calendario_area a
                     LEFT JOIN dbo.calendario_usuario_area cua ON a.area_id = cua.area_id
                     LEFT JOIN dbo.calendario_evento e ON a.area_id = e.area_id AND e.estado = 'A'
                     GROUP BY a.area_id, a.nombre, a.descripcion, a.color, a.orden, a.estado, a.fecha_creacion
                     ORDER BY a.orden ASC, a.nombre ASC";
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        list.Add(new
                        {
                           areaId = Convert.ToInt32(reader["area_id"]),
                           nombre = reader["nombre"].ToString(),
                           descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : "",
                           color = reader["color"] != DBNull.Value ? reader["color"].ToString() : "#0d6efd",
                           orden = Convert.ToInt32(reader["orden"]),
                           estado = reader["estado"].ToString().Trim(),
                           totalUsuarios = Convert.ToInt32(reader["total_usuarios"]),
                           totalEventos = Convert.ToInt32(reader["total_eventos"]),
                           fechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]).ToString("dd/MM/yyyy")
                        });
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al obtener áreas: " + ex.Message });
         }

         return Json(new { success = true, data = list });
      }

      [HttpPost]
      public JsonResult AdminGuardarArea([FromBody] AreaAdminInputModel model)
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         if (model == null || string.IsNullOrWhiteSpace(model.Nombre))
         {
            return Json(new { success = false, message = "El nombre del área es obligatorio." });
         }

         string nombre = model.Nombre.Trim();
         string descripcion = model.Descripcion?.Trim() ?? string.Empty;
         string color = !string.IsNullOrWhiteSpace(model.Color) && EsColorHexValido(model.Color) ? model.Color : "#0d6efd";
         int orden = model.Orden > 0 ? model.Orden : 1;
         string estado = string.Equals(model.Estado, "I", StringComparison.OrdinalIgnoreCase) ? "I" : "A";

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  if (model.AreaId <= 0)
                  {
                     cmd.CommandText = @"
                        IF @orden = 1 AND EXISTS (SELECT 1 FROM dbo.calendario_area)
                        BEGIN
                           SELECT @orden = ISNULL(MAX(orden), 0) + 1 FROM dbo.calendario_area;
                        END

                        INSERT INTO dbo.calendario_area (nombre, descripcion, color, orden, estado, fecha_creacion)
                        VALUES (@nombre, @descripcion, @color, @orden, 'A', SYSDATETIME());
                        SELECT SCOPE_IDENTITY();";
                     cmd.Parameters.AddWithValue("@nombre", nombre);
                     cmd.Parameters.AddWithValue("@descripcion", (object)descripcion ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@color", color);
                     cmd.Parameters.AddWithValue("@orden", orden);

                     var newId = Convert.ToInt32(cmd.ExecuteScalar());
                     return Json(new { success = true, message = "Área creada exitosamente.", areaId = newId });
                  }
                  else
                  {
                     cmd.CommandText = @"
                        UPDATE dbo.calendario_area
                        SET nombre = @nombre,
                            descripcion = @descripcion,
                            color = @color,
                            orden = @orden,
                            estado = @estado
                        WHERE area_id = @areaId";
                     cmd.Parameters.AddWithValue("@areaId", model.AreaId);
                     cmd.Parameters.AddWithValue("@nombre", nombre);
                     cmd.Parameters.AddWithValue("@descripcion", (object)descripcion ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@color", color);
                     cmd.Parameters.AddWithValue("@orden", orden);
                     cmd.Parameters.AddWithValue("@estado", estado);

                     cmd.ExecuteNonQuery();
                     return Json(new { success = true, message = "Área actualizada exitosamente.", areaId = model.AreaId });
                  }
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al guardar área: " + ex.Message });
         }
      }

      [HttpPost]
      public JsonResult AdminCambiarEstadoArea(int areaId, string estado)
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         if (areaId <= 0)
         {
            return Json(new { success = false, message = "Identificador de área inválido." });
         }

         string nuevoEstado = string.Equals(estado, "A", StringComparison.OrdinalIgnoreCase) ? "A" : "I";
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = "UPDATE dbo.calendario_area SET estado = @estado WHERE area_id = @areaId";
                  cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                  cmd.Parameters.AddWithValue("@areaId", areaId);
                  cmd.ExecuteNonQuery();
               }
            }
            return Json(new { success = true, message = (nuevoEstado == "A" ? "Área activada correctamente." : "Área desactivada correctamente.") });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al cambiar estado del área: " + ex.Message });
         }
      }

      [HttpPost]
      public JsonResult AdminEliminarArea(int areaId)
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         if (areaId <= 0)
         {
            return Json(new { success = false, message = "Identificador de área inválido." });
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = "SELECT COUNT(*) FROM dbo.calendario_evento WHERE area_id = @areaId AND estado = 'A'";
                  cmd.Parameters.AddWithValue("@areaId", areaId);
                  int eventosCount = Convert.ToInt32(cmd.ExecuteScalar());

                  if (eventosCount > 0)
                  {
                     return Json(new
                     {
                        success = false,
                        message = $"No se puede eliminar el área porque contiene {eventosCount} evento(s) registrado(s). Puedes desactivarla para ocultarla."
                     });
                  }

                  cmd.CommandText = @"
                     DELETE FROM dbo.calendario_evento WHERE area_id = @areaId;
                     DELETE FROM dbo.calendario_etiqueta WHERE area_id = @areaId;
                     DELETE FROM dbo.calendario_usuario_area WHERE area_id = @areaId;
                     DELETE FROM dbo.calendario_area WHERE area_id = @areaId;";
                  cmd.ExecuteNonQuery();
               }
            }
            return Json(new { success = true, message = "Área eliminada exitosamente." });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al eliminar área: " + ex.Message });
         }
      }

      [HttpGet]
      public JsonResult AdminObtenerUsuariosPorArea(int areaId)
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         if (areaId <= 0)
         {
            return Json(new { success = false, message = "Área inválida." });
         }

         var asignados = new List<object>();
         var disponibles = new List<object>();
         string areaNombre = "";
         string areaColor = "#0d6efd";

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();

               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = "SELECT nombre, color FROM dbo.calendario_area WHERE area_id = @areaId";
                  cmd.Parameters.AddWithValue("@areaId", areaId);
                  using (var reader = cmd.ExecuteReader())
                  {
                     if (reader.Read())
                     {
                        areaNombre = reader["nombre"].ToString();
                        areaColor = reader["color"] != DBNull.Value ? reader["color"].ToString() : "#0d6efd";
                     }
                  }
               }

               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT u.usuario_id, u.usuario_logon, u.nombre_completo, cua.es_principal, cua.fecha_creacion,
                            STRING_AGG(r.nombre, ', ') AS roles
                     FROM dbo.calendario_usuario_area cua
                     JOIN dbo.usuario u ON cua.usuario_id = u.usuario_id
                     LEFT JOIN dbo.rol_usuario ru ON u.usuario_id = ru.usuario_id
                     LEFT JOIN dbo.rol r ON ru.rol_id = r.rol_id
                     WHERE cua.area_id = @areaId
                     GROUP BY u.usuario_id, u.usuario_logon, u.nombre_completo, cua.es_principal, cua.fecha_creacion
                     ORDER BY cua.es_principal DESC, u.nombre_completo ASC";
                  cmd.Parameters.AddWithValue("@areaId", areaId);
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        asignados.Add(new
                        {
                           usuarioId = Convert.ToInt32(reader["usuario_id"]),
                           usuarioLogon = reader["usuario_logon"].ToString(),
                           nombreCompleto = reader["nombre_completo"].ToString(),
                           esPrincipal = Convert.ToBoolean(reader["es_principal"]),
                           roles = reader["roles"] != DBNull.Value ? reader["roles"].ToString() : "Sin rol asignado",
                           fechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]).ToString("dd/MM/yyyy")
                        });
                     }
                  }
               }

               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT u.usuario_id, u.usuario_logon, u.nombre_completo,
                            STRING_AGG(r.nombre, ', ') AS roles
                     FROM dbo.usuario u
                     LEFT JOIN dbo.rol_usuario ru ON u.usuario_id = ru.usuario_id
                     LEFT JOIN dbo.rol r ON ru.rol_id = r.rol_id
                     WHERE u.estado = 'A'
                       AND u.usuario_id NOT IN (SELECT usuario_id FROM dbo.calendario_usuario_area WHERE area_id = @areaId)
                     GROUP BY u.usuario_id, u.usuario_logon, u.nombre_completo
                     ORDER BY u.nombre_completo ASC";
                  cmd.Parameters.AddWithValue("@areaId", areaId);
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        disponibles.Add(new
                        {
                           usuarioId = Convert.ToInt32(reader["usuario_id"]),
                           usuarioLogon = reader["usuario_logon"].ToString(),
                           nombreCompleto = reader["nombre_completo"].ToString(),
                           roles = reader["roles"] != DBNull.Value ? reader["roles"].ToString() : "Sin rol asignado"
                        });
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al obtener usuarios del área: " + ex.Message });
         }

         return Json(new
         {
            success = true,
            areaId = areaId,
            areaNombre = areaNombre,
            areaColor = areaColor,
            asignados = asignados,
            disponibles = disponibles
         });
      }

      [HttpPost]
      public JsonResult AdminAsignarUsuarioArea(int usuarioId, int areaId, bool esPrincipal = false)
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         if (usuarioId <= 0 || areaId <= 0)
         {
            return Json(new { success = false, message = "Usuario y área son requeridos." });
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     IF NOT EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @uid AND area_id = @aid)
                     BEGIN
                         IF @esPrincipal = 1
                         BEGIN
                             UPDATE dbo.calendario_usuario_area SET es_principal = 0 WHERE usuario_id = @uid;
                         END
                         ELSE IF NOT EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @uid AND es_principal = 1)
                         BEGIN
                             SET @esPrincipal = 1;
                         END

                         INSERT INTO dbo.calendario_usuario_area (usuario_id, area_id, es_principal, fecha_creacion)
                         VALUES (@uid, @aid, @esPrincipal, SYSDATETIME());
                     END
                     ELSE
                     BEGIN
                         IF @esPrincipal = 1
                         BEGIN
                             UPDATE dbo.calendario_usuario_area SET es_principal = 0 WHERE usuario_id = @uid;
                             UPDATE dbo.calendario_usuario_area SET es_principal = 1 WHERE usuario_id = @uid AND area_id = @aid;
                         END
                     END";
                  cmd.Parameters.AddWithValue("@uid", usuarioId);
                  cmd.Parameters.AddWithValue("@aid", areaId);
                  cmd.Parameters.AddWithValue("@esPrincipal", esPrincipal);
                  cmd.ExecuteNonQuery();
               }
            }
            return Json(new { success = true, message = "Usuario asignado exitosamente al área." });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al asignar usuario al área: " + ex.Message });
         }
      }

      [HttpPost]
      public JsonResult AdminRemoverUsuarioArea(int usuarioId, int areaId)
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         if (usuarioId <= 0 || areaId <= 0)
         {
            return Json(new { success = false, message = "Usuario y área son requeridos." });
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     DELETE FROM dbo.calendario_usuario_area 
                     WHERE usuario_id = @uid AND area_id = @aid;

                     IF NOT EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @uid AND es_principal = 1)
                     BEGIN
                         UPDATE TOP (1) dbo.calendario_usuario_area SET es_principal = 1 WHERE usuario_id = @uid;
                     END";
                  cmd.Parameters.AddWithValue("@uid", usuarioId);
                  cmd.Parameters.AddWithValue("@aid", areaId);
                  cmd.ExecuteNonQuery();
               }
            }
            return Json(new { success = true, message = "Usuario removido del área correctamente." });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al remover usuario del área: " + ex.Message });
         }
      }

      [HttpPost]
      public JsonResult AdminToggleUsuarioPrincipal(int usuarioId, int areaId)
      {
         if (!EsAdministrador())
         {
            return Json(new { success = false, message = "Acceso denegado. Se requieren permisos de administrador." });
         }

         if (usuarioId <= 0 || areaId <= 0)
         {
            return Json(new { success = false, message = "Usuario y área son requeridos." });
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     UPDATE dbo.calendario_usuario_area SET es_principal = 0 WHERE usuario_id = @uid;
                     UPDATE dbo.calendario_usuario_area SET es_principal = 1 WHERE usuario_id = @uid AND area_id = @aid;";
                  cmd.Parameters.AddWithValue("@uid", usuarioId);
                  cmd.Parameters.AddWithValue("@aid", areaId);
                  cmd.ExecuteNonQuery();
               }
            }
            return Json(new { success = true, message = "Área establecida como principal para el usuario." });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al actualizar área principal: " + ex.Message });
         }
      }

      #endregion
   }
}