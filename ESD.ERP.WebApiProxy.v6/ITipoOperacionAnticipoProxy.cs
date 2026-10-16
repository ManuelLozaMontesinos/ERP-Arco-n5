using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoOperacionAnticipoProxy
	{
		TipoOperacionAnticipoDTO GetById(TipoOperacionAnticipoDTO dto);
		List<TipoOperacionAnticipoDTO> Search(TipoOperacionAnticipoDTO dto);
		TipoOperacionAnticipoDTO SingleSearch(TipoOperacionAnticipoDTO dto);
		int Insert(TipoOperacionAnticipoDTO dto);
		int Update(TipoOperacionAnticipoDTO dto);
		void Delete(TipoOperacionAnticipoDTO dto);
		TipoOperacionAnticipoDTO ObtenerPorCodigo(TipoOperacionAnticipoDTO dto);
	}
}
