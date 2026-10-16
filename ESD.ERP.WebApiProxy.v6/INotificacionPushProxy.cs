using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface INotificacionPushProxy
	{
		NotificacionPushDTO GetById(NotificacionPushDTO dto);
		List<NotificacionPushDTO> Search(NotificacionPushDTO dto);
		NotificacionPushDTO SingleSearch(NotificacionPushDTO dto);
		int Insert(NotificacionPushDTO dto);
		int Update(NotificacionPushDTO dto);
		void Delete(NotificacionPushDTO dto);
	}
}