using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoNotaCreditoBusiness
	{
		TipoNotaCreditoDTO GetById(int id);
		List<TipoNotaCreditoDTO> Search(TipoNotaCreditoDTO dto);
		TipoNotaCreditoDTO SingleSearch(TipoNotaCreditoDTO dto);
		int Insert(TipoNotaCreditoDTO dto);
		int Update(TipoNotaCreditoDTO dto);
		void Delete(TipoNotaCreditoDTO dto);
	}
}
