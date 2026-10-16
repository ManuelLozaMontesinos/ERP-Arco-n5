using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDocumentoComercialSerieProxy
	{
		DocumentoComercialSerieDTO GetById(DocumentoComercialSerieDTO dto);
		List<DocumentoComercialSerieDTO> Search(DocumentoComercialSerieDTO dto);
		DocumentoComercialSerieDTO SingleSearch(DocumentoComercialSerieDTO dto);
		int Insert(DocumentoComercialSerieDTO dto);
		int Update(DocumentoComercialSerieDTO dto);
		void Delete(DocumentoComercialSerieDTO dto);
	}
}
