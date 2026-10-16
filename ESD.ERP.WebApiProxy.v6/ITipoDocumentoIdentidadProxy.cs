using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoDocumentoIdentidadProxy
	{
		TipoDocumentoIdentidadDTO GetById(TipoDocumentoIdentidadDTO dto);
		List<TipoDocumentoIdentidadDTO> Search(TipoDocumentoIdentidadDTO dto);
		TipoDocumentoIdentidadDTO SingleSearch(TipoDocumentoIdentidadDTO dto);
		int Insert(TipoDocumentoIdentidadDTO dto);
		int Update(TipoDocumentoIdentidadDTO dto);
		void Delete(TipoDocumentoIdentidadDTO dto);
		TipoDocumentoIdentidadDTO ObtenerPorCodigo(TipoDocumentoIdentidadDTO dto);
	}
}
