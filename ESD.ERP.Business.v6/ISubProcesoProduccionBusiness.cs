using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISubProcesoProduccionBusiness
	{
		SubProcesoProduccionDTO GetById(int id);
		List<SubProcesoProduccionDTO> Search(SubProcesoProduccionDTO dto);
		SubProcesoProduccionDTO SingleSearch(SubProcesoProduccionDTO dto);
		int Insert(SubProcesoProduccionDTO dto);
		int Update(SubProcesoProduccionDTO dto);
		void Delete(SubProcesoProduccionDTO dto);
	}
}
