using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPlanillaProcesoHoraProxy
	{
		PlanillaProcesoHoraDTO GetById(PlanillaProcesoHoraDTO dto);
		List<PlanillaProcesoHoraDTO> Search(PlanillaProcesoHoraDTO dto);
		PlanillaProcesoHoraDTO SingleSearch(PlanillaProcesoHoraDTO dto);
		int Insert(PlanillaProcesoHoraDTO dto);
		int Update(PlanillaProcesoHoraDTO dto);
		void Delete(PlanillaProcesoHoraDTO dto);
		string ListarHoraGrid(PlanillaProcesoHoraDTO dto);
		void ActualizarLote(List<PlanillaProcesoHoraDTO> list);
		void ActualizarLotePorPersonal(List<PlanillaProcesoHoraDTO> list);
		void ActualizarListaPersonalId(PlanillaProcesoHoraDTO dto);
		void ActualizarDesdeMarcacion(PlanillaProcesoHoraDTO dto);
	}
}
