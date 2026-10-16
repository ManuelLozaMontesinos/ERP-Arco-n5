using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoProfesionOficioProxy
	{
		TipoProfesionOficioDTO GetById(TipoProfesionOficioDTO dto);
		List<TipoProfesionOficioDTO> Search(TipoProfesionOficioDTO dto);
		TipoProfesionOficioDTO SingleSearch(TipoProfesionOficioDTO dto);
		int Insert(TipoProfesionOficioDTO dto);
		int Update(TipoProfesionOficioDTO dto);
		void Delete(TipoProfesionOficioDTO dto);
	}
}
