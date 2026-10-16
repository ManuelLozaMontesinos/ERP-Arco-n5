using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IAlmacenSerieProxy
	{
		AlmacenSerieDTO GetById(AlmacenSerieDTO dto);
		List<AlmacenSerieDTO> Search(AlmacenSerieDTO dto);
		AlmacenSerieDTO SingleSearch(AlmacenSerieDTO dto);
		int Insert(AlmacenSerieDTO dto);
		int Update(AlmacenSerieDTO dto);
		void Delete(AlmacenSerieDTO dto);
		void ActualizarListaDocumentoComercialSerieId(AlmacenSerieDTO dto);
	}
}
