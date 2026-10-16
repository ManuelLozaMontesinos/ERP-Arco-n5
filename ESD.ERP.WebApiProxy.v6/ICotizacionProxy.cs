using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICotizacionProxy
	{
		CotizacionDTO GetById(CotizacionDTO dto);
		List<CotizacionDTO> Search(CotizacionDTO dto);

        CotizacionDTO SingleSearch(CotizacionDTO dto);
		int Insert(CotizacionDTO dto);
		int Update(CotizacionDTO dto);
		void Delete(CotizacionDTO dto);
		int GenerarComprobante(CotizacionDTO dto);
		void GenerarPedido(CotizacionDTO dto);
		void CambiarEstado(CotizacionDTO dto);
		int GenerarOrdenProduccion(CotizacionDTO dto);
		int GenerarOrdenProduccionParcial(CotizacionDTO dto);
		decimal CortesGratisParcial(CotizacionDTO dto);
    }
}
