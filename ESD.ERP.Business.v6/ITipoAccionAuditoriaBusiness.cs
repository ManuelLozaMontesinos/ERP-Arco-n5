using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoAccionAuditoriaBusiness
	{
		TipoAccionAuditoriaDTO GetById(int id);
		List<TipoAccionAuditoriaDTO> Search(TipoAccionAuditoriaDTO dto);
		TipoAccionAuditoriaDTO SingleSearch(TipoAccionAuditoriaDTO dto);
		int Insert(TipoAccionAuditoriaDTO dto);
		int Update(TipoAccionAuditoriaDTO dto);
		void Delete(TipoAccionAuditoriaDTO dto);
		TipoAccionAuditoriaDTO ObtenerPorCodigo(string codigo);
	}
}
