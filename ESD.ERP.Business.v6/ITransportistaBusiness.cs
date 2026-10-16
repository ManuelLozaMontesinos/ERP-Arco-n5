using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITransportistaBusiness
	{
		TransportistaDTO GetById(int id);
		List<TransportistaDTO> Search(TransportistaDTO dto);
		TransportistaDTO SingleSearch(TransportistaDTO dto);
		int Insert(TransportistaDTO dto);
		int Update(TransportistaDTO dto);
		void Delete(TransportistaDTO dto);
	}
}
