using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPrestamoPersonalBusiness
	{
		PrestamoPersonalDTO GetById(int id);
		List<PrestamoPersonalDTO> Search(PrestamoPersonalDTO dto);
		PrestamoPersonalDTO SingleSearch(PrestamoPersonalDTO dto);
		int Insert(PrestamoPersonalDTO dto);
		int Update(PrestamoPersonalDTO dto);
		void Delete(PrestamoPersonalDTO dto);
	}
}
