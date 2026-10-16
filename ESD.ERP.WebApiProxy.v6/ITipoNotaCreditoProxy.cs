using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoNotaCreditoProxy
	{
		TipoNotaCreditoDTO GetById(TipoNotaCreditoDTO dto);
		List<TipoNotaCreditoDTO> Search(TipoNotaCreditoDTO dto);
		TipoNotaCreditoDTO SingleSearch(TipoNotaCreditoDTO dto);
		int Insert(TipoNotaCreditoDTO dto);
		int Update(TipoNotaCreditoDTO dto);
		void Delete(TipoNotaCreditoDTO dto);
	}
}
