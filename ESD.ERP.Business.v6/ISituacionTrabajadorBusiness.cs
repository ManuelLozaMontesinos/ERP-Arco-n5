using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISituacionTrabajadorBusiness
	{
		SituacionTrabajadorDTO GetById(int id);
		List<SituacionTrabajadorDTO> Search(SituacionTrabajadorDTO dto);
		SituacionTrabajadorDTO SingleSearch(SituacionTrabajadorDTO dto);
		int Insert(SituacionTrabajadorDTO dto);
		int Update(SituacionTrabajadorDTO dto);
		void Delete(SituacionTrabajadorDTO dto);
		SituacionTrabajadorDTO ObtenerPorCodigo(string codigo);
	}
}
