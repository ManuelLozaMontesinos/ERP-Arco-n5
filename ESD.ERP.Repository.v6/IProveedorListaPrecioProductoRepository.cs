using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
   public interface IProveedorListaPrecioProductoRepository : IGenericRepository<ProveedorListaPrecioProducto>
   {
      void ActualizarLote(List<ProveedorListaPrecioProducto> list);
      void EliminarPorProveedorIdProductoId(ProveedorListaPrecioProducto entity);
   }
}
