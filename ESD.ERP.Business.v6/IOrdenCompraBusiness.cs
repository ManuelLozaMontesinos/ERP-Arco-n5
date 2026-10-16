using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenCompraBusiness
	{
		OrdenCompraDTO GetById(int id);
		List<OrdenCompraDTO> Search(OrdenCompraDTO dto);
		OrdenCompraDTO SingleSearch(OrdenCompraDTO dto);
		int Insert(OrdenCompraDTO dto);
		int Update(OrdenCompraDTO dto);
		void Delete(OrdenCompraDTO dto);
		void SolicitarAutorizacion(OrdenCompraDTO dto);
		void SolicitarModificacion(OrdenCompraDTO dto);
		void Aprobar(OrdenCompraDTO dto);
		void Desaprobar(OrdenCompraDTO dto);
		void CambiarEstado(OrdenCompraDTO dto);
	}
}
