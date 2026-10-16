using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProveedorListaPrecioProductoBusiness
	{
		ProveedorListaPrecioProductoDTO GetById(int id);
		List<ProveedorListaPrecioProductoDTO> Search(ProveedorListaPrecioProductoDTO dto);
		ProveedorListaPrecioProductoDTO SingleSearch(ProveedorListaPrecioProductoDTO dto);
		int Insert(ProveedorListaPrecioProductoDTO dto);
		int Update(ProveedorListaPrecioProductoDTO dto);
		void Delete(ProveedorListaPrecioProductoDTO dto);
      void ActualizarLote(List<ProveedorListaPrecioProductoDTO> list);
   }
}
