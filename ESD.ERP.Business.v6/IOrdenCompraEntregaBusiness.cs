using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenCompraEntregaBusiness
	{
		OrdenCompraEntregaDTO GetById(int id);
		List<OrdenCompraEntregaDTO> Search(OrdenCompraEntregaDTO dto);
		OrdenCompraEntregaDTO SingleSearch(OrdenCompraEntregaDTO dto);
		int Insert(OrdenCompraEntregaDTO dto);
		int Update(OrdenCompraEntregaDTO dto);
		void Delete(OrdenCompraEntregaDTO dto);
		void AgregarDesdeOrdenCompraProducto(OrdenCompraEntregaDTO dto);
	}
}
