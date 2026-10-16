using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IPlanillaProcesoHoraRepository : IGenericRepository<PlanillaProcesoHora>
	{
		List<dynamic> ListarHoraPivot(PlanillaProcesoHora entity);
		void ActualizarLote(List<PlanillaProcesoHora> list);
		void ActualizarLotePorPersonal(List<PlanillaProcesoHora> list);
		void ActualizarListaPersonalId(PlanillaProcesoHora entity);
		void ActualizarDesdeMarcacion(PlanillaProcesoHora entity);
	}
}
