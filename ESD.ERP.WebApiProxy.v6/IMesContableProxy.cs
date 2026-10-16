using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMesContableProxy
	{
		MesContableDTO GetById(MesContableDTO dto);
		List<MesContableDTO> Search(MesContableDTO dto);
		MesContableDTO SingleSearch(MesContableDTO dto);
		int Insert(MesContableDTO dto);
		int Update(MesContableDTO dto);
		void Delete(MesContableDTO dto);
		MesContableDTO ObtenerPorCodigo(MesContableDTO dto);
	}
}
