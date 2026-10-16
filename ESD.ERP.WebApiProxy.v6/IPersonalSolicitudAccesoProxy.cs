using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalSolicitudAccesoProxy
	{
		PersonalSolicitudAccesoDTO GetById(PersonalSolicitudAccesoDTO dto);
		List<PersonalSolicitudAccesoDTO> Search(PersonalSolicitudAccesoDTO dto);
		PersonalSolicitudAccesoDTO SingleSearch(PersonalSolicitudAccesoDTO dto);
		int Insert(PersonalSolicitudAccesoDTO dto);
		int Update(PersonalSolicitudAccesoDTO dto);
		void Delete(PersonalSolicitudAccesoDTO dto);
		void Aprobar(PersonalSolicitudAccesoDTO dto);
		void Desaprobar(PersonalSolicitudAccesoDTO dto);
	}
}
