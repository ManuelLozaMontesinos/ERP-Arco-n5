using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRemuneracionMinimaVitalProxy
	{
		RemuneracionMinimaVitalDTO GetById(RemuneracionMinimaVitalDTO dto);
		List<RemuneracionMinimaVitalDTO> Search(RemuneracionMinimaVitalDTO dto);
		RemuneracionMinimaVitalDTO SingleSearch(RemuneracionMinimaVitalDTO dto);
		int Insert(RemuneracionMinimaVitalDTO dto);
		int Update(RemuneracionMinimaVitalDTO dto);
		void Delete(RemuneracionMinimaVitalDTO dto);
	}
}
