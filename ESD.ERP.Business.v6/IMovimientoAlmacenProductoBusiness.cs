using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMovimientoAlmacenProductoBusiness
	{
		MovimientoAlmacenProductoDTO GetById(int id);
		List<MovimientoAlmacenProductoDTO> Search(MovimientoAlmacenProductoDTO dto);
		MovimientoAlmacenProductoDTO SingleSearch(MovimientoAlmacenProductoDTO dto);
		int Insert(MovimientoAlmacenProductoDTO dto);
		int Update(MovimientoAlmacenProductoDTO dto);
		void Delete(MovimientoAlmacenProductoDTO dto);
		void Delete(List<MovimientoAlmacenProductoDTO> list);
		void AgregarDesdeReserva(MovimientoAlmacenProductoDTO dto);
		void ActualizarSaldoTransferenciaSalida(MovimientoAlmacenProductoDTO dto);
		void ActualizarProductoReclasificacion(MovimientoAlmacenProductoDTO dto);
		void ActualizarSaldoAprobado(MovimientoAlmacenProductoDTO dto);
      void AgregarOrdenCompraProducto(MovimientoAlmacenProductoDTO dto);
   }
}
