using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPaisBusiness
	{
		PaisDTO GetById(int id);
		List<PaisDTO> Search(PaisDTO dto);
		PaisDTO SingleSearch(PaisDTO dto);
		int Insert(PaisDTO dto);
		int Update(PaisDTO dto);
		void Delete(PaisDTO dto);
		PaisDTO ObtenerPorCodigo(string codigo);
	}
}
