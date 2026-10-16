using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICotizacionBusiness
	{
		CotizacionDTO GetById(int id);
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
