using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoViaBusiness
	{
		TipoViaDTO GetById(int id);
		List<TipoViaDTO> Search(TipoViaDTO dto);
		TipoViaDTO SingleSearch(TipoViaDTO dto);
		int Insert(TipoViaDTO dto);
		int Update(TipoViaDTO dto);
		void Delete(TipoViaDTO dto);
		TipoViaDTO ObtenerPorCodigo(string codigo);
	}
}
