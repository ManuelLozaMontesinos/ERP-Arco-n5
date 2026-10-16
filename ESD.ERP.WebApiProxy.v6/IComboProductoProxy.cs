using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IComboProductoProxy
	{
		ComboProductoDTO GetById(ComboProductoDTO dto);
		List<ComboProductoDTO> Search(ComboProductoDTO dto);
		ComboProductoDTO SingleSearch(ComboProductoDTO dto);
		int Insert(ComboProductoDTO dto);
		int Update(ComboProductoDTO dto);
		void Delete(ComboProductoDTO dto);
	}
}
