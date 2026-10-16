using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IAlmacenSerieBusiness
	{
		AlmacenSerieDTO GetById(int id);
		List<AlmacenSerieDTO> Search(AlmacenSerieDTO dto);
		AlmacenSerieDTO SingleSearch(AlmacenSerieDTO dto);
		int Insert(AlmacenSerieDTO dto);
		int Update(AlmacenSerieDTO dto);
		void Delete(AlmacenSerieDTO dto);
		void ActualizarListaDocumentoComercialSerieId(AlmacenSerieDTO dto);
	}
}
