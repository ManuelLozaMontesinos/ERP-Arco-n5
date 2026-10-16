using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoDocumentoComercialBusiness
	{
		TipoDocumentoComercialDTO GetById(int id);
		List<TipoDocumentoComercialDTO> Search(TipoDocumentoComercialDTO dto);
		TipoDocumentoComercialDTO SingleSearch(TipoDocumentoComercialDTO dto);
		int Insert(TipoDocumentoComercialDTO dto);
		int Update(TipoDocumentoComercialDTO dto);
		void Delete(TipoDocumentoComercialDTO dto);
		TipoDocumentoComercialDTO ObtenerPorCodigo(string codigo);
	}
}
