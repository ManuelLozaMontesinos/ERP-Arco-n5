using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICotizacionProductoBusiness
	{
		CotizacionProductoDTO GetById(int id);
		List<CotizacionProductoDTO> Search(CotizacionProductoDTO dto);
		List<CotizacionProductoDTO> ProductosParcial(CotizacionProductoDTO dto);
        List<CotizacionProductoDTO> CortesGratis(CotizacionProductoDTO dto);
        List<CotizacionProductoDTO> CortesGratisIlo(CotizacionProductoDTO dto);
        CotizacionProductoDTO SingleSearch(CotizacionProductoDTO dto);
		int Insert(CotizacionProductoDTO dto);
		int Update(CotizacionProductoDTO dto);
		void Delete(CotizacionProductoDTO dto);
		void ActualizarReserva(CotizacionProductoDTO cotizacionProductoDTO);
		void ActualizarReservaCombo(CotizacionProductoDTO cotizacionProductoDTO);
		void ActualizarSinSaldo(CotizacionProductoDTO cotizacionProductoDTO);
		void ActualizarSaldoProduccion(CotizacionProductoDTO dto);
    }
}