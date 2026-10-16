using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMesContableBusiness
	{
		MesContableDTO GetById(int id);
		List<MesContableDTO> Search(MesContableDTO dto);
		MesContableDTO SingleSearch(MesContableDTO dto);
		int Insert(MesContableDTO dto);
		int Update(MesContableDTO dto);
		void Delete(MesContableDTO dto);
		MesContableDTO ObtenerPorCodigo(string codigo);
	}
}
