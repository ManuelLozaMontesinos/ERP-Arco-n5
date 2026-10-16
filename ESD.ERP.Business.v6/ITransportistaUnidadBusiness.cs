using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITransportistaUnidadBusiness
	{
		TransportistaUnidadDTO GetById(int id);
		List<TransportistaUnidadDTO> Search(TransportistaUnidadDTO dto);
		TransportistaUnidadDTO SingleSearch(TransportistaUnidadDTO dto);
		int Insert(TransportistaUnidadDTO dto);
		int Update(TransportistaUnidadDTO dto);
		void Delete(TransportistaUnidadDTO dto);
	}
}
