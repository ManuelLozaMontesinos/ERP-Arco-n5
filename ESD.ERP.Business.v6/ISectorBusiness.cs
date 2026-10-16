using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISectorBusiness
	{
		SectorDTO GetById(int id);
		List<SectorDTO> Search(SectorDTO dto);
		SectorDTO SingleSearch(SectorDTO dto);
		int Insert(SectorDTO dto);
		int Update(SectorDTO dto);
		void Delete(SectorDTO dto);
	}
}
