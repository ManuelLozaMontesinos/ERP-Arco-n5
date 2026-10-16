using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IGuiaRemisionBusiness
	{
		GuiaRemisionDTO GetById(int id);
		List<GuiaRemisionDTO> Search(GuiaRemisionDTO dto);
		GuiaRemisionDTO SingleSearch(GuiaRemisionDTO dto);
		int Insert(GuiaRemisionDTO dto);
		int Update(GuiaRemisionDTO dto);
		void Delete(GuiaRemisionDTO dto);
      void GenerarDesdeComprobante(GuiaRemisionDTO dto);
		void Emitir(GuiaRemisionDTO dto);
		void EmitirSunat(GuiaRemisionDTO dto);
		void SolicitarModificacion(GuiaRemisionDTO dto);
      void Aprobar(GuiaRemisionDTO dto);
      void Desaprobar(GuiaRemisionDTO dto);
		void ActualizarEstado(GuiaRemisionDTO dto);
		void ActualizarEmisionElectronica(GuiaRemisionDTO dto);
		void ActualizarPeso(GuiaRemisionDTO dto);

    }
}
