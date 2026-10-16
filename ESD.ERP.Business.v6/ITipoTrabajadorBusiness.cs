using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoTrabajadorBusiness
	{
		TipoTrabajadorDTO GetById(int id);
		List<TipoTrabajadorDTO> Search(TipoTrabajadorDTO dto);
		TipoTrabajadorDTO SingleSearch(TipoTrabajadorDTO dto);
		int Insert(TipoTrabajadorDTO dto);
		int Update(TipoTrabajadorDTO dto);
		void Delete(TipoTrabajadorDTO dto);
		TipoTrabajadorDTO ObtenerPorCodigo(string codigo);
	}
}
