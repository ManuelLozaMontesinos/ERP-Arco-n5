using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoAccionAuditoriaProxy
	{
		TipoAccionAuditoriaDTO GetById(TipoAccionAuditoriaDTO dto);
		List<TipoAccionAuditoriaDTO> Search(TipoAccionAuditoriaDTO dto);
		TipoAccionAuditoriaDTO SingleSearch(TipoAccionAuditoriaDTO dto);
		int Insert(TipoAccionAuditoriaDTO dto);
		int Update(TipoAccionAuditoriaDTO dto);
		void Delete(TipoAccionAuditoriaDTO dto);
		TipoAccionAuditoriaDTO ObtenerPorCodigo(TipoAccionAuditoriaDTO dto);
	}
}