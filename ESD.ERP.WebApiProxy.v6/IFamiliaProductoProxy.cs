using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IFamiliaProductoProxy
	{
		FamiliaProductoDTO GetById(FamiliaProductoDTO dto);
		List<FamiliaProductoDTO> Search(FamiliaProductoDTO dto);
		FamiliaProductoDTO SingleSearch(FamiliaProductoDTO dto);
		int Insert(FamiliaProductoDTO dto);
		int Update(FamiliaProductoDTO dto);
		void Delete(FamiliaProductoDTO dto);
	}
}
