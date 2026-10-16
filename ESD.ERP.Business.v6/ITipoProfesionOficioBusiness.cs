using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoProfesionOficioBusiness
	{
		TipoProfesionOficioDTO GetById(int id);
		List<TipoProfesionOficioDTO> Search(TipoProfesionOficioDTO dto);
		TipoProfesionOficioDTO SingleSearch(TipoProfesionOficioDTO dto);
		int Insert(TipoProfesionOficioDTO dto);
		int Update(TipoProfesionOficioDTO dto);
		void Delete(TipoProfesionOficioDTO dto);
	}
}
