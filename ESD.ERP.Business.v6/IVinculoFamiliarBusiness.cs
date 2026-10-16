using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IVinculoFamiliarBusiness
	{
		VinculoFamiliarDTO GetById(int id);
		List<VinculoFamiliarDTO> Search(VinculoFamiliarDTO dto);
		VinculoFamiliarDTO SingleSearch(VinculoFamiliarDTO dto);
		int Insert(VinculoFamiliarDTO dto);
		int Update(VinculoFamiliarDTO dto);
		void Delete(VinculoFamiliarDTO dto);
	}
}
