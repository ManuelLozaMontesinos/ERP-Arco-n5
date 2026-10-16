using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoViaProxy
	{
		TipoViaDTO GetById(TipoViaDTO dto);
		List<TipoViaDTO> Search(TipoViaDTO dto);
		TipoViaDTO SingleSearch(TipoViaDTO dto);
		int Insert(TipoViaDTO dto);
		int Update(TipoViaDTO dto);
		void Delete(TipoViaDTO dto);
		TipoViaDTO ObtenerPorCodigo(TipoViaDTO dto);
	}
}
