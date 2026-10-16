using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOcupacionBusiness
	{
		OcupacionDTO GetById(int id);
		List<OcupacionDTO> Search(OcupacionDTO dto);
		OcupacionDTO SingleSearch(OcupacionDTO dto);
		int Insert(OcupacionDTO dto);
		int Update(OcupacionDTO dto);
		void Delete(OcupacionDTO dto);
		OcupacionDTO ObtenerPorCodigo(string codigo);
	}
}
