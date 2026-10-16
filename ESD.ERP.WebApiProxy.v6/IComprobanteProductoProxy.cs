using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IComprobanteProductoProxy
	{
		ComprobanteProductoDTO GetById(ComprobanteProductoDTO dto);
		List<ComprobanteProductoDTO> Search(ComprobanteProductoDTO dto);
		ComprobanteProductoDTO SingleSearch(ComprobanteProductoDTO dto);
		int Insert(ComprobanteProductoDTO dto);
		int Update(ComprobanteProductoDTO dto);
		void Delete(ComprobanteProductoDTO dto);
      void AgregarCotizacionProducto(ComprobanteProductoDTO dto);
		void AgregarComprobanteProductoCanje(ComprobanteProductoDTO dto);
		void AgregarComprobanteProductoCanjeSelectivo(ComprobanteProductoDTO dto);
		List<ComprobanteProductoDTO> ListarPendienteCanje(ComprobanteProductoDTO dto);
		List<ComprobanteProductoDTO> ListarPorIdList(ComprobanteProductoDTO dto);
		int ActualizarCanje(ComprobanteProductoDTO dto);
		void EliminarCanje(ComprobanteProductoDTO dto);
      void AgregarComprobanteProductoEntrega(ComprobanteProductoDTO comprobanteDTO);
   }
}
