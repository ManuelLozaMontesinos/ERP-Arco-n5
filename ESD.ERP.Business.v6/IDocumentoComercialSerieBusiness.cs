using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IDocumentoComercialSerieBusiness
	{
		DocumentoComercialSerieDTO GetById(int id);
		List<DocumentoComercialSerieDTO> Search(DocumentoComercialSerieDTO dto);
		DocumentoComercialSerieDTO SingleSearch(DocumentoComercialSerieDTO dto);
		int Insert(DocumentoComercialSerieDTO dto);
		int Update(DocumentoComercialSerieDTO dto);
		void Delete(DocumentoComercialSerieDTO dto);
		string ObtenerCorrelativo(DocumentoComercialSerieDTO dto);
		string FormatearNumero(DocumentoComercialSerieDTO dto);
	}
}
