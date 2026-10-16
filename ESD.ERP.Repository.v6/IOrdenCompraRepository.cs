using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
   public interface IOrdenCompraRepository : IGenericRepository<OrdenCompra>
   {
      void ActualizarEstado(OrdenCompra entity);
      void ActualizarMonto(OrdenCompra entity);
      void ActualizarEstadoAprobacion(OrdenCompra entity);
      void ActualizarProductos(OrdenCompra entity);
    }
}