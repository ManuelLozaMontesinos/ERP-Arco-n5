using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPrestamoPersonalProxy
	{
		PrestamoPersonalDTO GetById(PrestamoPersonalDTO dto);
		List<PrestamoPersonalDTO> Search(PrestamoPersonalDTO dto);
		PrestamoPersonalDTO SingleSearch(PrestamoPersonalDTO dto);
		int Insert(PrestamoPersonalDTO dto);
		int Update(PrestamoPersonalDTO dto);
		void Delete(PrestamoPersonalDTO dto);
	}
}
