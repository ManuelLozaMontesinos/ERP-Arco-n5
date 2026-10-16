using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IComprobanteAutorizacionBusiness
	{
		ComprobanteAutorizacionDTO GetById(int id);
		List<ComprobanteAutorizacionDTO> Search(ComprobanteAutorizacionDTO dto);
		ComprobanteAutorizacionDTO SingleSearch(ComprobanteAutorizacionDTO dto);
		int Insert(ComprobanteAutorizacionDTO dto);
		int Update(ComprobanteAutorizacionDTO dto);
		void Delete(ComprobanteAutorizacionDTO dto);
		List<ComprobanteAutorizacionDTO> ListarPorTipoAutorizacionIdList(ComprobanteAutorizacionDTO dto);
		void Solicitar(ComprobanteAutorizacionDTO dto);
		void Aprobar(ComprobanteAutorizacionDTO dto);
		void Desaprobar(ComprobanteAutorizacionDTO dto);
		void Cancelar(ComprobanteAutorizacionDTO dto);
	}
}
