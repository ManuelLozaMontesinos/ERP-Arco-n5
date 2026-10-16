using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalSolicitudAccesoBusiness
	{
		PersonalSolicitudAccesoDTO GetById(int id);
		List<PersonalSolicitudAccesoDTO> Search(PersonalSolicitudAccesoDTO dto);
		PersonalSolicitudAccesoDTO SingleSearch(PersonalSolicitudAccesoDTO dto);
		int Insert(PersonalSolicitudAccesoDTO dto);
		int Update(PersonalSolicitudAccesoDTO dto);
		void Delete(PersonalSolicitudAccesoDTO dto);
		void Aprobar(PersonalSolicitudAccesoDTO dto);
		void Desaprobar(PersonalSolicitudAccesoDTO dto);

	}
}
