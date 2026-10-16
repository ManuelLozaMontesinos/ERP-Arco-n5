using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOrdenProduccionProxy
	{
		OrdenProduccionDTO GetById(OrdenProduccionDTO dto);
		List<OrdenProduccionDTO> Search(OrdenProduccionDTO dto);
		OrdenProduccionDTO SingleSearch(OrdenProduccionDTO dto);
		int Insert(OrdenProduccionDTO dto);
		int Update(OrdenProduccionDTO dto);
		void Delete(OrdenProduccionDTO dto);
		void CerrarDespacho(OrdenProduccionDTO dto);
		void CerrarInsumo(OrdenProduccionDTO dto);
		void CambiarEstado(OrdenProduccionDTO dto);
		void AsignarMaquina(OrdenProduccionDTO dto);
    }
}
