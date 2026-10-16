using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoTrabajadorProxy
	{
		TipoTrabajadorDTO GetById(TipoTrabajadorDTO dto);
		List<TipoTrabajadorDTO> Search(TipoTrabajadorDTO dto);
		TipoTrabajadorDTO SingleSearch(TipoTrabajadorDTO dto);
		int Insert(TipoTrabajadorDTO dto);
		int Update(TipoTrabajadorDTO dto);
		void Delete(TipoTrabajadorDTO dto);
		TipoTrabajadorDTO ObtenerPorCodigo(TipoTrabajadorDTO dto);
	}
}
