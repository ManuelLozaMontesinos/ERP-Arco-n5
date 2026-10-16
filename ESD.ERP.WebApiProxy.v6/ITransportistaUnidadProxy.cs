using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITransportistaUnidadProxy
	{
		TransportistaUnidadDTO GetById(TransportistaUnidadDTO dto);
		List<TransportistaUnidadDTO> Search(TransportistaUnidadDTO dto);
		TransportistaUnidadDTO SingleSearch(TransportistaUnidadDTO dto);
		int Insert(TransportistaUnidadDTO dto);
		int Update(TransportistaUnidadDTO dto);
		void Delete(TransportistaUnidadDTO dto);
	}
}
