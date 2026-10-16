using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IAlmacenVolumenProductoRepository : IGenericRepository<AlmacenVolumenProducto>
	{
		List<AlmacenVolumenProducto> ListarPorSedeId(AlmacenVolumenProducto entity);
      void ActualizarLote(List<AlmacenVolumenProducto> list);
		List<dynamic> ListarStockMinimo(AlmacenVolumenProducto entity);

	}
}
