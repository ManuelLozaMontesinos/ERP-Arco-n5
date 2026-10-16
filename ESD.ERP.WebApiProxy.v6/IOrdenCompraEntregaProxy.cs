using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOrdenCompraEntregaProxy
	{
		OrdenCompraEntregaDTO GetById(OrdenCompraEntregaDTO dto);
		List<OrdenCompraEntregaDTO> Search(OrdenCompraEntregaDTO dto);
		OrdenCompraEntregaDTO SingleSearch(OrdenCompraEntregaDTO dto);
		int Insert(OrdenCompraEntregaDTO dto);
		int Update(OrdenCompraEntregaDTO dto);
		void Delete(OrdenCompraEntregaDTO dto);
		void AgregarDesdeOrdenCompraProducto(OrdenCompraEntregaDTO dto);
	}
}
