using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISubProcesoProduccionPersonalBusiness
	{
		SubProcesoProduccionPersonalDTO GetById(int id);
		List<SubProcesoProduccionPersonalDTO> Search(SubProcesoProduccionPersonalDTO dto);
		SubProcesoProduccionPersonalDTO SingleSearch(SubProcesoProduccionPersonalDTO dto);
		int Insert(SubProcesoProduccionPersonalDTO dto);
		int Update(SubProcesoProduccionPersonalDTO dto);
		void Delete(SubProcesoProduccionPersonalDTO dto);
	}
}
