using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IComprobanteProxy
	{
		ComprobanteDTO GetById(ComprobanteDTO dto);
		List<ComprobanteDTO> Search(ComprobanteDTO dto);
		ComprobanteDTO SingleSearch(ComprobanteDTO dto);
		int Insert(ComprobanteDTO dto);
		int Update(ComprobanteDTO dto);
		void Delete(ComprobanteDTO dto);
		void AplicarTasaPorMedioPago(ComprobanteDTO dto);
		void Emitir(ComprobanteDTO dto);
		void Cerrar(ComprobanteDTO dto);
      void ActualizarEmisionElectronica(ComprobanteDTO dto);
		List<ComprobanteDTO> ListarDocumentoEmitido(ComprobanteDTO dto);
		void ActualizarEstadoSunat(ComprobanteDTO dto);
		List<ComprobanteDTO> ListarResumenEstadoSunat(ComprobanteDTO dto);
		void ActualizarPorComprobanteElectronicoIdList(ComprobanteDTO dto);
		void EmitirCanje(ComprobanteDTO dto);
		void EliminarCanje(ComprobanteDTO dto);
		void ActualizarDetraccion(ComprobanteDTO dto);
		void ActualizarTipoComprobante(ComprobanteDTO dto);
		int AgregarComprobanteAnticipo(ComprobanteDTO dto);
		int ActualizarComprobanteAnticipo(ComprobanteDTO dto);
		void AnularComprobanteAnticipo(ComprobanteDTO dto);
		string ListarCobranzaGrid(ComprobanteDTO dto);
		List<ComprobanteDTO> BuscarDespacho(ComprobanteDTO dto);
      void CerrarDespacho(ComprobanteDTO dto);
		List<ComprobanteDTO> BuscarPorEmpresaSede(ComprobanteDTO dto);
	}
}
