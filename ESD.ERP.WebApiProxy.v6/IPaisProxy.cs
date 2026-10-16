using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPaisProxy
	{
		PaisDTO GetById(PaisDTO dto);
		List<PaisDTO> Search(PaisDTO dto);
		PaisDTO SingleSearch(PaisDTO dto);
		int Insert(PaisDTO dto);
		int Update(PaisDTO dto);
		void Delete(PaisDTO dto);
		PaisDTO ObtenerPorCodigo(PaisDTO dto);
	}
}
