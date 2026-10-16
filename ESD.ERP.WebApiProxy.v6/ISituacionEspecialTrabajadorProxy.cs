using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISituacionEspecialTrabajadorProxy
	{
		SituacionEspecialTrabajadorDTO GetById(SituacionEspecialTrabajadorDTO dto);
		List<SituacionEspecialTrabajadorDTO> Search(SituacionEspecialTrabajadorDTO dto);
		SituacionEspecialTrabajadorDTO SingleSearch(SituacionEspecialTrabajadorDTO dto);
		int Insert(SituacionEspecialTrabajadorDTO dto);
		int Update(SituacionEspecialTrabajadorDTO dto);
		void Delete(SituacionEspecialTrabajadorDTO dto);
		SituacionEspecialTrabajadorDTO ObtenerPorCodigo(SituacionEspecialTrabajadorDTO dto);
	}
}
