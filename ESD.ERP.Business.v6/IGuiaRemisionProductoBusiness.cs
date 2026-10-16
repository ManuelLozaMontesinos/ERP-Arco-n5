using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IGuiaRemisionProductoBusiness
	{
		GuiaRemisionProductoDTO GetById(int id);
		List<GuiaRemisionProductoDTO> Search(GuiaRemisionProductoDTO dto);
		GuiaRemisionProductoDTO SingleSearch(GuiaRemisionProductoDTO dto);
		int Insert(GuiaRemisionProductoDTO dto);
		int Update(GuiaRemisionProductoDTO dto);
		void Delete(GuiaRemisionProductoDTO dto);
		void EliminarPorGuiaRemisionId(GuiaRemisionProductoDTO dto);
	}
}
