using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IAlmacenTransaccionConfiguracionRepository : IGenericRepository<AlmacenTransaccionConfiguracion>
	{
		List<AlmacenTransaccionConfiguracion> ListarPorPorAlmacenTransaccionId(AlmacenTransaccionConfiguracion entity);
	}
}
