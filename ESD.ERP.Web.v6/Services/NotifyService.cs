using ESD.Core;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Services
{
   public interface INotifyService
   {
      void SendNotification(NotificacionPushDTO notificacionPushDTO);
   }
   public class NotifyService: INotifyService
   {
      private readonly IHubContext<NotifyHub> hubContext;
      private readonly INotificacionPushProxy notificacionPushProxy;

      public NotifyService(
         IHubContext<NotifyHub> hubContext,
         INotificacionPushProxy notificacionPushProxy
         )
      {
         this.hubContext = hubContext;
         this.notificacionPushProxy = notificacionPushProxy;
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
   }
}
