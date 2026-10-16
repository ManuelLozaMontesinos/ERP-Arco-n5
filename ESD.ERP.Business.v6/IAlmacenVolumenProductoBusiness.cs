using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IAlmacenVolumenProductoBusiness
	{
		AlmacenVolumenProductoDTO GetById(int id);
		List<AlmacenVolumenProductoDTO> Search(AlmacenVolumenProductoDTO dto);
		AlmacenVolumenProductoDTO SingleSearch(AlmacenVolumenProductoDTO dto);
		int Insert(AlmacenVolumenProductoDTO dto);
		int Update(AlmacenVolumenProductoDTO dto);
		void Delete(AlmacenVolumenProductoDTO dto);
		List<AlmacenVolumenProductoDTO> ListarPorSedeId(AlmacenVolumenProductoDTO dto);
		void ActualizarLote(List<AlmacenVolumenProductoDTO> list);
		List<dynamic> ListarStockMinimo(AlmacenVolumenProductoDTO dto);
	}
}
