using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMesBusiness
	{
		MesDTO GetById(int id);
		List<MesDTO> Search(MesDTO dto);
		MesDTO SingleSearch(MesDTO dto);
		int Insert(MesDTO dto);
		int Update(MesDTO dto);
		void Delete(MesDTO dto);
		MesDTO ObtenerPorCodigo(string codigo);
	}
}
