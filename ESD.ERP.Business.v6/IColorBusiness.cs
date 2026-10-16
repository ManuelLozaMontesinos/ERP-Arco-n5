using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IColorBusiness
	{
		ColorDTO GetById(int id);
		List<ColorDTO> Search(ColorDTO dto);
		ColorDTO SingleSearch(ColorDTO dto);
		int Insert(ColorDTO dto);
		int Update(ColorDTO dto);
		void Delete(ColorDTO dto);
	}
}
