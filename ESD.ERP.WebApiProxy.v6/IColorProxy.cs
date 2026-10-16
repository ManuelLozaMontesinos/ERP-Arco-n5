using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IColorProxy
	{
		ColorDTO GetById(ColorDTO dto);
		List<ColorDTO> Search(ColorDTO dto);
		ColorDTO SingleSearch(ColorDTO dto);
		int Insert(ColorDTO dto);
		int Update(ColorDTO dto);
		void Delete(ColorDTO dto);
	}
}
