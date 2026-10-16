using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOrigenRegistroProxy
	{
		OrigenRegistroDTO GetById(OrigenRegistroDTO dto);
		List<OrigenRegistroDTO> Search(OrigenRegistroDTO dto);
		OrigenRegistroDTO SingleSearch(OrigenRegistroDTO dto);
		int Insert(OrigenRegistroDTO dto);
		int Update(OrigenRegistroDTO dto);
		void Delete(OrigenRegistroDTO dto);
		OrigenRegistroDTO ObtenerPorCodigo(OrigenRegistroDTO dto);
	}
}
