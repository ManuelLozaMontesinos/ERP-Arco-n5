using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPlanillaProcesoHoraBusiness
	{
		PlanillaProcesoHoraDTO GetById(int id);
		List<PlanillaProcesoHoraDTO> Search(PlanillaProcesoHoraDTO dto);
		PlanillaProcesoHoraDTO SingleSearch(PlanillaProcesoHoraDTO dto);
		int Insert(PlanillaProcesoHoraDTO dto);
		int Update(PlanillaProcesoHoraDTO dto);
		void Delete(PlanillaProcesoHoraDTO dto);
		List<dynamic> ListarHoraPivot(PlanillaProcesoHoraDTO dto);
		void ActualizarLote(List<PlanillaProcesoHoraDTO> list);
		void ActualizarLotePorPersonal(List<PlanillaProcesoHoraDTO> list);
		void ActualizarListaPersonalId(PlanillaProcesoHoraDTO dto);
		void ActualizarDesdeMarcacion(PlanillaProcesoHoraDTO dto);
	}
}
