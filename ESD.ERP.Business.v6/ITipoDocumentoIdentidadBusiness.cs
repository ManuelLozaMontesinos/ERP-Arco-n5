using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoDocumentoIdentidadBusiness
	{
		TipoDocumentoIdentidadDTO GetById(int id);
		List<TipoDocumentoIdentidadDTO> Search(TipoDocumentoIdentidadDTO dto);
		TipoDocumentoIdentidadDTO SingleSearch(TipoDocumentoIdentidadDTO dto);
		int Insert(TipoDocumentoIdentidadDTO dto);
		int Update(TipoDocumentoIdentidadDTO dto);
		void Delete(TipoDocumentoIdentidadDTO dto);
		TipoDocumentoIdentidadDTO ObtenerPorCodigo(string codigo);
	}
}