using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IComboProductoBusiness
	{
		ComboProductoDTO GetById(int id);
		List<ComboProductoDTO> Search(ComboProductoDTO dto);
		ComboProductoDTO SingleSearch(ComboProductoDTO dto);
		int Insert(ComboProductoDTO dto);
		int Update(ComboProductoDTO dto);
		void Delete(ComboProductoDTO dto);
	}
}
