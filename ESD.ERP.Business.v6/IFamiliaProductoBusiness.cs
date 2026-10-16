using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IFamiliaProductoBusiness
	{
		FamiliaProductoDTO GetById(int id);
		List<FamiliaProductoDTO> Search(FamiliaProductoDTO dto);
		FamiliaProductoDTO SingleSearch(FamiliaProductoDTO dto);
		int Insert(FamiliaProductoDTO dto);
		int Update(FamiliaProductoDTO dto);
		void Delete(FamiliaProductoDTO dto);
	}
}
