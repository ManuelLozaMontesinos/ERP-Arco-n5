using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface INotaCreditoProxy
	{
		NotaCreditoDTO GetById(NotaCreditoDTO dto);
		List<NotaCreditoDTO> Search(NotaCreditoDTO dto);
		NotaCreditoDTO SingleSearch(NotaCreditoDTO dto);
		int Insert(NotaCreditoDTO dto);
		int Update(NotaCreditoDTO dto);
		void Delete(NotaCreditoDTO dto);
		void Cerrar(NotaCreditoDTO dto);
		void Emitir(NotaCreditoDTO dto);
		void ActualizarEmisionElectronica(NotaCreditoDTO dto);
		void ActualizarEstadoSunat(NotaCreditoDTO dto);
		void ActualizarPorComprobanteElectronicoIdList(NotaCreditoDTO dto);
		void EmitirInterno(NotaCreditoDTO dto);
   }
}
