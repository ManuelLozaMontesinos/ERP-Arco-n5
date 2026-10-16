using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenProduccionBusiness
	{
		OrdenProduccionDTO GetById(int id);
		List<OrdenProduccionDTO> Search(OrdenProduccionDTO dto);
		OrdenProduccionDTO SingleSearch(OrdenProduccionDTO dto);
		int Insert(OrdenProduccionDTO dto);
		int Update(OrdenProduccionDTO dto);
		void Delete(OrdenProduccionDTO dto);
		void CerrarDespacho(OrdenProduccionDTO dto);
		void EliminarSubProcesos(OrdenProduccionDTO dto);
		void ActualizarFecha(OrdenProduccionDTO dto);
		void CerrarInsumo(OrdenProduccionDTO dto);
		void CambiarEstado(OrdenProduccionDTO dto);
		void AsignarMaquina(OrdenProduccionDTO dto);
    }
}
