using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface INotificacionPushBusiness
	{
		NotificacionPushDTO GetById(int id);
		List<NotificacionPushDTO> Search(NotificacionPushDTO dto);
		NotificacionPushDTO SingleSearch(NotificacionPushDTO dto);
		int Insert(NotificacionPushDTO dto);
		int Update(NotificacionPushDTO dto);
		void Delete(NotificacionPushDTO dto);
	}
}
