using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IGuiaRemisionProductoRepository : IGenericRepository<GuiaRemisionProducto>
	{
		void EliminarPorGuiaRemisionId(GuiaRemisionProducto entity);
		void ActualizarLote(List<GuiaRemisionProducto> list);
	}
}
