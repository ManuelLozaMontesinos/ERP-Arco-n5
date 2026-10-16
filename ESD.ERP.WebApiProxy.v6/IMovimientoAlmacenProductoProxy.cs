using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMovimientoAlmacenProductoProxy
	{
		MovimientoAlmacenProductoDTO GetById(MovimientoAlmacenProductoDTO dto);
		List<MovimientoAlmacenProductoDTO> Search(MovimientoAlmacenProductoDTO dto);
		MovimientoAlmacenProductoDTO SingleSearch(MovimientoAlmacenProductoDTO dto);
		int Insert(MovimientoAlmacenProductoDTO dto);
		int Update(MovimientoAlmacenProductoDTO dto);
		void Delete(MovimientoAlmacenProductoDTO dto);
		void AgregarDesdeReserva(MovimientoAlmacenProductoDTO dto);
		void ActualizarProductoReclasificacion(MovimientoAlmacenProductoDTO dto);
      void AgregarOrdenCompraProducto(MovimientoAlmacenProductoDTO dto);
   }
}
