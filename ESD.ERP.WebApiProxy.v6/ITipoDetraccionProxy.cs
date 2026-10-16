using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoDetraccionProxy
	{
		TipoDetraccionDTO GetById(TipoDetraccionDTO dto);
		List<TipoDetraccionDTO> Search(TipoDetraccionDTO dto);
		TipoDetraccionDTO SingleSearch(TipoDetraccionDTO dto);
		int Insert(TipoDetraccionDTO dto);
		int Update(TipoDetraccionDTO dto);
		void Delete(TipoDetraccionDTO dto);
		TipoDetraccionDTO ObtenerPorCodigo(TipoDetraccionDTO dto);
	}
}
