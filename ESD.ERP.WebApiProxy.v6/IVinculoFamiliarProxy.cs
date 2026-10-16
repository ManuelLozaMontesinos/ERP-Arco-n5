using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IVinculoFamiliarProxy
	{
		VinculoFamiliarDTO GetById(VinculoFamiliarDTO dto);
		List<VinculoFamiliarDTO> Search(VinculoFamiliarDTO dto);
		VinculoFamiliarDTO SingleSearch(VinculoFamiliarDTO dto);
		int Insert(VinculoFamiliarDTO dto);
		int Update(VinculoFamiliarDTO dto);
		void Delete(VinculoFamiliarDTO dto);
	}
}
