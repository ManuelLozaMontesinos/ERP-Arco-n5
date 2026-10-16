using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProcesoIntegracionBusiness
	{
		ProcesoIntegracionDTO GetById(int id);
		List<ProcesoIntegracionDTO> Search(ProcesoIntegracionDTO dto);
		ProcesoIntegracionDTO SingleSearch(ProcesoIntegracionDTO dto);
		int Insert(ProcesoIntegracionDTO dto);
		int Update(ProcesoIntegracionDTO dto);
		void Delete(ProcesoIntegracionDTO dto);
		void EjecutarJob(ProcesoIntegracionDTO dto);
	}
}
