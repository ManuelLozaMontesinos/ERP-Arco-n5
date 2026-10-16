using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoDocumentoVinculoFamiliarProxy
	{
		TipoDocumentoVinculoFamiliarDTO GetById(TipoDocumentoVinculoFamiliarDTO dto);
		List<TipoDocumentoVinculoFamiliarDTO> Search(TipoDocumentoVinculoFamiliarDTO dto);
		TipoDocumentoVinculoFamiliarDTO SingleSearch(TipoDocumentoVinculoFamiliarDTO dto);
		int Insert(TipoDocumentoVinculoFamiliarDTO dto);
		int Update(TipoDocumentoVinculoFamiliarDTO dto);
		void Delete(TipoDocumentoVinculoFamiliarDTO dto);
	}
}
