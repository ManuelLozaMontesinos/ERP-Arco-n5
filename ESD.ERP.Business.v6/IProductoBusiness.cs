using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProductoBusiness
	{
		ProductoDTO GetById(int id);
		List<ProductoDTO> Search(ProductoDTO dto);
		ProductoDTO SingleSearch(ProductoDTO dto);
		int Insert(ProductoDTO dto);
		int Update(ProductoDTO dto);
		void Delete(ProductoDTO dto);
		void ActualizarPesoMasterPack(ProductoDTO dto);
		void ActualizarEstado(ProductoDTO dto);
	}
}
