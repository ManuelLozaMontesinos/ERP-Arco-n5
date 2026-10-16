using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IGuiaRemisionProductoProxy
	{
		GuiaRemisionProductoDTO GetById(GuiaRemisionProductoDTO dto);
		List<GuiaRemisionProductoDTO> Search(GuiaRemisionProductoDTO dto);
		GuiaRemisionProductoDTO SingleSearch(GuiaRemisionProductoDTO dto);
		int Insert(GuiaRemisionProductoDTO dto);
		int Update(GuiaRemisionProductoDTO dto);
		void Delete(GuiaRemisionProductoDTO dto);
	}
}
