using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProcesoProduccionBusiness
	{
		ProcesoProduccionDTO GetById(int id);
		List<ProcesoProduccionDTO> Search(ProcesoProduccionDTO dto);
		ProcesoProduccionDTO SingleSearch(ProcesoProduccionDTO dto);
		int Insert(ProcesoProduccionDTO dto);
		int Update(ProcesoProduccionDTO dto);
		void Delete(ProcesoProduccionDTO dto);
	}
}
