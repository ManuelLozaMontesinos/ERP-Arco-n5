using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoDocumentoVinculoFamiliarBusiness
	{
		TipoDocumentoVinculoFamiliarDTO GetById(int id);
		List<TipoDocumentoVinculoFamiliarDTO> Search(TipoDocumentoVinculoFamiliarDTO dto);
		TipoDocumentoVinculoFamiliarDTO SingleSearch(TipoDocumentoVinculoFamiliarDTO dto);
		int Insert(TipoDocumentoVinculoFamiliarDTO dto);
		int Update(TipoDocumentoVinculoFamiliarDTO dto);
		void Delete(TipoDocumentoVinculoFamiliarDTO dto);
	}
}
