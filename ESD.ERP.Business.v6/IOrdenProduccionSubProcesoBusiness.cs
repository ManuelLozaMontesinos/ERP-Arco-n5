using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenProduccionSubProcesoBusiness
	{
		OrdenProduccionSubProcesoDTO GetById(int id);
		List<OrdenProduccionSubProcesoDTO> Search(OrdenProduccionSubProcesoDTO dto);
		OrdenProduccionSubProcesoDTO SingleSearch(OrdenProduccionSubProcesoDTO dto);
		int Insert(OrdenProduccionSubProcesoDTO dto);
		int Update(OrdenProduccionSubProcesoDTO dto);
		void Delete(OrdenProduccionSubProcesoDTO dto);
		void ActualizarInicio(OrdenProduccionSubProcesoDTO dto);
		void ActualizarFin(OrdenProduccionSubProcesoDTO dto);
		void Maquina(OrdenProduccionSubProcesoDTO dto);
    }
}
