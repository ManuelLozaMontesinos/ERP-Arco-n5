using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITransportistaProxy
	{
		TransportistaDTO GetById(TransportistaDTO dto);
		List<TransportistaDTO> Search(TransportistaDTO dto);
		TransportistaDTO SingleSearch(TransportistaDTO dto);
		int Insert(TransportistaDTO dto);
		int Update(TransportistaDTO dto);
		void Delete(TransportistaDTO dto);
	}
}
