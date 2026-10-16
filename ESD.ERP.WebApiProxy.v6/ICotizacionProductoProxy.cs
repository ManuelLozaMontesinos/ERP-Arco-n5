using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICotizacionProductoProxy
	{
		CotizacionProductoDTO GetById(CotizacionProductoDTO dto);
		List<CotizacionProductoDTO> Search(CotizacionProductoDTO dto);
		List<CotizacionProductoDTO> ProductosParcial(CotizacionProductoDTO dto);
        List<CotizacionProductoDTO> CortesGratis(CotizacionProductoDTO dto);
        List<CotizacionProductoDTO> CortesGratisIlo(CotizacionProductoDTO dto);
        CotizacionProductoDTO SingleSearch(CotizacionProductoDTO dto);
		int Insert(CotizacionProductoDTO dto);
		int Update(CotizacionProductoDTO dto);
		void Delete(CotizacionProductoDTO dto);
		void ActualizarSinSaldo(CotizacionProductoDTO dto);

    }
}
