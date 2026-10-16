using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrigenRegistroBusiness
	{
		OrigenRegistroDTO GetById(int id);
		List<OrigenRegistroDTO> Search(OrigenRegistroDTO dto);
		OrigenRegistroDTO SingleSearch(OrigenRegistroDTO dto);
		int Insert(OrigenRegistroDTO dto);
		int Update(OrigenRegistroDTO dto);
		void Delete(OrigenRegistroDTO dto);
		OrigenRegistroDTO ObtenerPorCodigo(string codigo);
	}
}
