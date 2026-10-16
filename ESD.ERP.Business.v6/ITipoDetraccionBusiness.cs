using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoDetraccionBusiness
	{
		TipoDetraccionDTO GetById(int id);
		List<TipoDetraccionDTO> Search(TipoDetraccionDTO dto);
		TipoDetraccionDTO SingleSearch(TipoDetraccionDTO dto);
		int Insert(TipoDetraccionDTO dto);
		int Update(TipoDetraccionDTO dto);
		void Delete(TipoDetraccionDTO dto);
		TipoDetraccionDTO ObtenerPorCodigo(string codigo);
	}
}
