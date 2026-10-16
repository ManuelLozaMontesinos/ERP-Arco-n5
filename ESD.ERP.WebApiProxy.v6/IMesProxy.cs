using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMesProxy
	{
		MesDTO GetById(MesDTO dto);
		List<MesDTO> Search(MesDTO dto);
		MesDTO SingleSearch(MesDTO dto);
		int Insert(MesDTO dto);
		int Update(MesDTO dto);
		void Delete(MesDTO dto);
		MesDTO ObtenerPorCodigo(MesDTO dto);
	}
}
