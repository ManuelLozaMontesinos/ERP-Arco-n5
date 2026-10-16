using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICotizacionAutorizacionProxy
	{
		CotizacionAutorizacionDTO GetById(CotizacionAutorizacionDTO dto);
		List<CotizacionAutorizacionDTO> Search(CotizacionAutorizacionDTO dto);
		CotizacionAutorizacionDTO SingleSearch(CotizacionAutorizacionDTO dto);
		int Insert(CotizacionAutorizacionDTO dto);
		int Update(CotizacionAutorizacionDTO dto);
		void Delete(CotizacionAutorizacionDTO dto);
		List<CotizacionAutorizacionDTO> ListarPorTipoAutorizacionIdList(CotizacionAutorizacionDTO dto);
		void Solicitar(CotizacionAutorizacionDTO dto);
		void Aprobar(CotizacionAutorizacionDTO dto);
		void Desaprobar(CotizacionAutorizacionDTO dto);
		void Cancelar(CotizacionAutorizacionDTO dto);
	}
}
