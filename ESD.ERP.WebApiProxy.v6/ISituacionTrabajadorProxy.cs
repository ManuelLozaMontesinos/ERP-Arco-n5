using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISituacionTrabajadorProxy
	{
		SituacionTrabajadorDTO GetById(SituacionTrabajadorDTO dto);
		List<SituacionTrabajadorDTO> Search(SituacionTrabajadorDTO dto);
		SituacionTrabajadorDTO SingleSearch(SituacionTrabajadorDTO dto);
		int Insert(SituacionTrabajadorDTO dto);
		int Update(SituacionTrabajadorDTO dto);
		void Delete(SituacionTrabajadorDTO dto);
		SituacionTrabajadorDTO ObtenerPorCodigo(SituacionTrabajadorDTO dto);
	}
}
