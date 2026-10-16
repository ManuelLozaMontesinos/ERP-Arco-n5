using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IAlmacenVolumenProductoProxy
	{
		AlmacenVolumenProductoDTO GetById(AlmacenVolumenProductoDTO dto);
		List<AlmacenVolumenProductoDTO> Search(AlmacenVolumenProductoDTO dto);
		AlmacenVolumenProductoDTO SingleSearch(AlmacenVolumenProductoDTO dto);
		int Insert(AlmacenVolumenProductoDTO dto);
		int Update(AlmacenVolumenProductoDTO dto);
		void Delete(AlmacenVolumenProductoDTO dto);
		List<AlmacenVolumenProductoDTO> ListarPorSedeId(AlmacenVolumenProductoDTO dto);
		void ActualizarLote(List<AlmacenVolumenProductoDTO> list);
		string ListarStockMinimoGrid(AlmacenVolumenProductoDTO dto);
	}
}
