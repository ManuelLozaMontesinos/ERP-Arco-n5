using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IComprobanteAutorizacionRepository : IGenericRepository<ComprobanteAutorizacion>
	{
		List<ComprobanteAutorizacion> ListarPorTipoAutorizacionIdList(ComprobanteAutorizacion entity);
	}
}
