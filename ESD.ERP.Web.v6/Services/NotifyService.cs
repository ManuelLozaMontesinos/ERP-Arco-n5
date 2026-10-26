using ESD.Core;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESD.ERP.Web.Services
{
   public interface INotifyService
   {
      void SendNotification(NotificacionPushDTO notificacionPushDTO);
      Task SendCalendarReminderAsync(int usuarioId, string tituloEvento, DateTime fechaInicio, string areaNombre = null);
      Task SendCalendarReminderToUsersAsync(IEnumerable<int> usuarioIds, string tituloEvento, DateTime fechaInicio, string areaNombre = null);
   }
   public class NotifyService: INotifyService
   {
      private readonly IHubContext<NotifyHub> hubContext;
      private readonly INotificacionPushProxy notificacionPushProxy;
      private readonly ILogger<NotifyService> logger;
      private readonly IConfiguration configuration;

      public NotifyService(
         IHubContext<NotifyHub> hubContext,
         INotificacionPushProxy notificacionPushProxy,
         ILogger<NotifyService> logger,
         IConfiguration configuration
         )
      {
         this.hubContext = hubContext;
         this.notificacionPushProxy = notificacionPushProxy;
         this.logger = logger;
         this.configuration = configuration;
      }

      public void SendNotification(NotificacionPushDTO notificacionPushDTO)
      {
         notificacionPushProxy.Insert(notificacionPushDTO);

         List<NotificacionPushDTO> notificacionPushDTOList = notificacionPushProxy.Search(new NotificacionPushDTO()
         {
            OrderBy = "fecha_reg desc",
            Fecha = DateTime.Today,
            UsuarioId = notificacionPushDTO.UsuarioId
         });

         object data = new
         {
            Total = notificacionPushDTOList.Count,
            List = from f in notificacionPushDTOList.AsEnumerable()
                   select new
                   {
                      f.NotificacionPushId,
                      f.Titulo,
                      f.Descripcion,
                      Fecha = string.Format(StringFormatConstant.DATE_TIME, f.FechaRegistro)
                   }
         };

         JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

         hubContext.Clients.User(notificacionPushDTO.UsuarioId.ToString()).SendAsync("refreshNotifications", notificacionPushDTO.UsuarioId,
            JsonSerializer.Serialize(data, options));
      }

      public async Task SendCalendarReminderAsync(int usuarioId, string tituloEvento, DateTime fechaInicio, string areaNombre = null)
      {
         await SendCalendarReminderToUsersAsync(new[] { usuarioId }, tituloEvento, fechaInicio, areaNombre);
      }

      public async Task SendCalendarReminderToUsersAsync(IEnumerable<int> usuarioIds, string tituloEvento, DateTime fechaInicio, string areaNombre = null)
      {
         if (usuarioIds == null) return;
         var ids = usuarioIds.Distinct().Where(id => id > 0).ToList();
         if (ids.Count == 0) return;

         string tituloNotificacion = string.IsNullOrWhiteSpace(areaNombre)
            ? ("Próximo evento: " + tituloEvento)
            : ($"[{areaNombre}] " + tituloEvento);

         if (tituloNotificacion.Length > 100)
         {
            tituloNotificacion = tituloNotificacion.Substring(0, 100);
         }

         string descripcionNotificacion = string.IsNullOrWhiteSpace(areaNombre)
            ? ("El evento inicia el " + fechaInicio.ToString("dd/MM/yyyy HH:mm") + ".")
            : ($"Evento de {areaNombre} inicia el " + fechaInicio.ToString("dd/MM/yyyy HH:mm") + ".");

         string alertMessage = string.IsNullOrWhiteSpace(areaNombre)
            ? "Tienes un evento próximo que inicia en 15 minutos o menos. Revisa tus notificaciones."
            : $"Tienes un evento próximo del área de {areaNombre} que inicia en 15 minutos o menos. Revisa tus notificaciones.";

         using (var connection = new SqlConnection(CalendarDatabase.GetConnectionString(configuration)))
         {
            connection.Open();
            foreach (int uid in ids)
            {
               try
               {
                  using (var command = connection.CreateCommand())
                  {
                     command.CommandText = "sp_notifi_push_insertar";
                     command.CommandType = CommandType.StoredProcedure;
                     command.Parameters.Add("@titulo", SqlDbType.VarChar, 100).Value = tituloNotificacion;
                     command.Parameters.Add("@descripcion", SqlDbType.VarChar, 400).Value = descripcionNotificacion;
                     command.Parameters.Add("@usuarioId", SqlDbType.Int).Value = uid;
                     SqlParameter newId = command.Parameters.Add("@newID", SqlDbType.Int);
                     newId.Direction = ParameterDirection.Output;
                     command.ExecuteNonQuery();
                  }
               }
               catch (Exception ex)
               {
                  logger.LogError(ex, "Error al registrar la notificación push de calendario para el usuario {UserId}.", uid);
               }
            }
         }

         foreach (int uid in ids)
         {
            try
            {
               await hubContext.Clients.User(uid.ToString()).SendAsync("calendarReminder", alertMessage);
               await hubContext.Clients.User(uid.ToString()).SendAsync("refreshNotificationList");
            }
            catch (Exception ex)
            {
               logger.LogError(ex, "El recordatorio del calendario se guardó, pero no se pudo solicitar la actualización en tiempo real para el usuario {UserId}.", uid);
            }
         }
      }
   }
}
