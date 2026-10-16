using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISectorProxy
	{
		SectorDTO GetById(SectorDTO dto);
		List<SectorDTO> Search(SectorDTO dto);
		SectorDTO SingleSearch(SectorDTO dto);
		int Insert(SectorDTO dto);
		int Update(SectorDTO dto);
		void Delete(SectorDTO dto);
	}
}
