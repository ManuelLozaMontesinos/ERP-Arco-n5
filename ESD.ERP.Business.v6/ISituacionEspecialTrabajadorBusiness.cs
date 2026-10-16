using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISituacionEspecialTrabajadorBusiness
	{
		SituacionEspecialTrabajadorDTO GetById(int id);
		List<SituacionEspecialTrabajadorDTO> Search(SituacionEspecialTrabajadorDTO dto);
		SituacionEspecialTrabajadorDTO SingleSearch(SituacionEspecialTrabajadorDTO dto);
		int Insert(SituacionEspecialTrabajadorDTO dto);
		int Update(SituacionEspecialTrabajadorDTO dto);
		void Delete(SituacionEspecialTrabajadorDTO dto);
		SituacionEspecialTrabajadorDTO ObtenerPorCodigo(string codigo);
	}
}
