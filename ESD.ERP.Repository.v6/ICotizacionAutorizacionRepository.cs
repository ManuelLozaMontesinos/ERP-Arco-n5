using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface ICotizacionAutorizacionRepository : IGenericRepository<CotizacionAutorizacion>
	{
		List<CotizacionAutorizacion> ListarPorTipoAutorizacionIdList(CotizacionAutorizacion entity);
		void UpdateAuto(CotizacionAutorizacion entity);

    }
}
