using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRemuneracionMinimaVitalBusiness
	{
		RemuneracionMinimaVitalDTO GetById(int id);
		List<RemuneracionMinimaVitalDTO> Search(RemuneracionMinimaVitalDTO dto);
		RemuneracionMinimaVitalDTO SingleSearch(RemuneracionMinimaVitalDTO dto);
		int Insert(RemuneracionMinimaVitalDTO dto);
		int Update(RemuneracionMinimaVitalDTO dto);
		void Delete(RemuneracionMinimaVitalDTO dto);
	}
}
