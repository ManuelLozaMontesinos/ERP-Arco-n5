using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoDocumentoComercialProxy
	{
		TipoDocumentoComercialDTO GetById(TipoDocumentoComercialDTO dto);
		List<TipoDocumentoComercialDTO> Search(TipoDocumentoComercialDTO dto);
		TipoDocumentoComercialDTO SingleSearch(TipoDocumentoComercialDTO dto);
		int Insert(TipoDocumentoComercialDTO dto);
		int Update(TipoDocumentoComercialDTO dto);
		void Delete(TipoDocumentoComercialDTO dto);
		TipoDocumentoComercialDTO ObtenerPorCodigo(TipoDocumentoComercialDTO dto);
	}
}
