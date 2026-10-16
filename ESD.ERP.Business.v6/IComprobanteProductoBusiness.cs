using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IComprobanteProductoBusiness
	{
		ComprobanteProductoDTO GetById(int id);
		List<ComprobanteProductoDTO> Search(ComprobanteProductoDTO dto);
		ComprobanteProductoDTO SingleSearch(ComprobanteProductoDTO dto);
		int Insert(ComprobanteProductoDTO dto);
		int Update(ComprobanteProductoDTO dto);
		void Delete(ComprobanteProductoDTO dto);
      void AgregarCotizacionProducto(ComprobanteProductoDTO dto);
		List<ComprobanteProductoDTO> ListarPendienteCanje(ComprobanteProductoDTO dto);
		List<ComprobanteProductoDTO> ListarPorIdList(ComprobanteProductoDTO dto);
      void AgregarComprobanteProductoCanjeSelectivo(ComprobanteProductoDTO dto);
		void AgregarComprobanteProductoCanje(ComprobanteProductoDTO dto);
		int ActualizarCanje(ComprobanteProductoDTO dto);
		void EliminarCanje(ComprobanteProductoDTO dto);
		void ActualizarCantidadEntregada(ComprobanteProductoDTO dto);
		void AgregarComprobanteProductoEntrega(ComprobanteProductoDTO dto);
	}
}
