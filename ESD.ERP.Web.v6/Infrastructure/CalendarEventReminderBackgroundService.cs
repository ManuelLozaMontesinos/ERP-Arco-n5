using ESD.ERP.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace ESD.ERP.Web.Infrastructure
{
   public class CalendarEventReminderBackgroundService : BackgroundService
   {
      private const int ReminderMinutes = 15;
      private const int ClaimLeaseMinutes = 5;
      private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(15);

      private readonly INotifyService notifyService;
      private readonly ILogger<CalendarEventReminderBackgroundService> logger;
      private readonly IConfiguration configuration;

      public CalendarEventReminderBackgroundService(
         INotifyService notifyService,
         ILogger<CalendarEventReminderBackgroundService> logger,
         IConfiguration configuration)
      {
         this.notifyService = notifyService;
         this.logger = logger;
         this.configuration = configuration;
      }

      protected override async Task ExecuteAsync(CancellationToken stoppingToken)
      {
         while (!stoppingToken.IsCancellationRequested)
         {
            try
            {
               List<DueCalendarEvent> events = ClaimDueEvents();
               foreach (DueCalendarEvent calendarEvent in events)
               {
                  try
                  {
                     var (memberIds, areaNombre) = ObtenerMiembrosArea(calendarEvent.AreaId);
                     if (memberIds.Count == 0 && calendarEvent.UserId > 0)
                     {
                        memberIds.Add(calendarEvent.UserId);
                     }
                     await notifyService.SendCalendarReminderToUsersAsync(memberIds, calendarEvent.Title, calendarEvent.Start, areaNombre);
                     MarkReminderSent(calendarEvent.Id);
                  }
                  catch (Exception ex)
                  {
                     ReleaseReminder(calendarEvent.Id);
                     logger.LogError(ex, "No se pudo enviar el recordatorio del evento {EventId}. Se volverá a intentar.", calendarEvent.Id);
                  }
               }
            }
            catch (Exception ex)
            {
               logger.LogError(ex, "No se pudieron consultar los recordatorios pendientes del calendario.");
            }

            try
            {
               await Task.Delay(PollInterval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
               break;
            }
         }
      }

      private List<DueCalendarEvent> ClaimDueEvents()
      {
         var events = new List<DueCalendarEvent>();

         using (var connection = new SqlConnection(CalendarDatabase.GetConnectionString(configuration)))
         {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
               command.CommandText = @"
                  ;WITH eventos_pendientes AS
                  (
                     SELECT TOP (25) evento_id
                     FROM dbo.calendario_evento WITH (UPDLOCK, READPAST, ROWLOCK)
                     WHERE estado = 'A'
                       AND todo_el_dia = 0
                       AND fecha_inicio > GETDATE()
                       AND fecha_inicio <= DATEADD(MINUTE, @minutosAviso, GETDATE())
                       AND
                       (
                          recordatorio_estado = 'P'
                          OR (recordatorio_estado = 'R' AND recordatorio_reclamado_hasta <= GETDATE())
                       )
                     ORDER BY fecha_inicio
                  )
                  UPDATE evento
                  SET recordatorio_estado = 'R',
                      recordatorio_reclamado_hasta = DATEADD(MINUTE, @minutosReclamo, GETDATE())
                  OUTPUT inserted.evento_id, inserted.usuario_id, inserted.area_id, inserted.titulo, inserted.fecha_inicio
                  FROM dbo.calendario_evento AS evento
                  INNER JOIN eventos_pendientes AS pendiente ON pendiente.evento_id = evento.evento_id;";
               command.Parameters.AddWithValue("@minutosAviso", ReminderMinutes);
               command.Parameters.AddWithValue("@minutosReclamo", ClaimLeaseMinutes);

               using (SqlDataReader reader = command.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     int areaId = reader["area_id"] != DBNull.Value ? Convert.ToInt32(reader["area_id"]) : 0;
                     events.Add(new DueCalendarEvent
                     {
                        Id = Convert.ToInt32(reader["evento_id"]),
                        UserId = Convert.ToInt32(reader["usuario_id"]),
                        AreaId = areaId,
                        Title = Convert.ToString(reader["titulo"]),
                        Start = Convert.ToDateTime(reader["fecha_inicio"])
                     });
                  }
               }
            }
         }

         return events;
      }

      private void MarkReminderSent(int eventId)
      {
         UpdateReminderState(eventId, "E", true);
      }

      private void ReleaseReminder(int eventId)
      {
         UpdateReminderState(eventId, "P", false);
      }

      private void UpdateReminderState(int eventId, string state, bool sent)
      {
         using (var connection = new SqlConnection(CalendarDatabase.GetConnectionString(configuration)))
         {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
               command.CommandText = @"
                  UPDATE dbo.calendario_evento
                  SET recordatorio_estado = @estado,
                      recordatorio_reclamado_hasta = NULL,
                      recordatorio_enviado_en = CASE WHEN @enviado = 1 THEN GETDATE() ELSE NULL END
                  WHERE evento_id = @id AND recordatorio_estado = 'R';";
               command.Parameters.AddWithValue("@estado", state);
               command.Parameters.AddWithValue("@enviado", sent);
               command.Parameters.AddWithValue("@id", eventId);
               command.ExecuteNonQuery();
            }
         }
      }

      private (List<int> memberIds, string areaNombre) ObtenerMiembrosArea(int areaId)
      {
         var members = new List<int>();
         string areaNombre = null;

         if (areaId <= 0)
         {
            return (members, areaNombre);
         }

         try
         {
            using (var connection = new SqlConnection(CalendarDatabase.GetConnectionString(configuration)))
            {
               connection.Open();
               using (var command = connection.CreateCommand())
               {
                  command.CommandText = @"
                     SELECT cua.usuario_id, a.nombre AS area_nombre
                     FROM dbo.calendario_usuario_area cua
                     INNER JOIN dbo.calendario_area a ON cua.area_id = a.area_id
                     INNER JOIN dbo.usuario u ON cua.usuario_id = u.usuario_id
                     WHERE cua.area_id = @areaId AND a.estado = 'A' AND u.estado = 'A';";
                  command.Parameters.AddWithValue("@areaId", areaId);

                  using (var reader = command.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        members.Add(Convert.ToInt32(reader["usuario_id"]));
                        if (string.IsNullOrEmpty(areaNombre) && reader["area_nombre"] != DBNull.Value)
                        {
                           areaNombre = reader["area_nombre"].ToString().Trim();
                        }
                     }
                  }
               }

               if (string.IsNullOrEmpty(areaNombre))
               {
                  using (var cmdName = connection.CreateCommand())
                  {
                     cmdName.CommandText = "SELECT nombre FROM dbo.calendario_area WHERE area_id = @areaId;";
                     cmdName.Parameters.AddWithValue("@areaId", areaId);
                     object res = cmdName.ExecuteScalar();
                     if (res != null && res != DBNull.Value)
                     {
                        areaNombre = res.ToString().Trim();
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            logger.LogError(ex, "Error al consultar los miembros del área {AreaId} para el recordatorio.", areaId);
         }

         return (members, areaNombre);
      }

      private sealed class DueCalendarEvent
      {
         public int Id { get; set; }
         public int UserId { get; set; }
         public int AreaId { get; set; }
         public string Title { get; set; }
         public DateTime Start { get; set; }
      }
   }
}
