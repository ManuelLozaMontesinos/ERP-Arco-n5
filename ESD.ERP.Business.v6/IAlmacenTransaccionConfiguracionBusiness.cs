using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IAlmacenTransaccionConfiguracionBusiness
	{
		AlmacenTransaccionConfiguracionDTO GetById(int id);
		List<AlmacenTransaccionConfiguracionDTO> Search(AlmacenTransaccionConfiguracionDTO dto);
		AlmacenTransaccionConfiguracionDTO SingleSearch(AlmacenTransaccionConfiguracionDTO dto);
		int Insert(AlmacenTransaccionConfiguracionDTO dto);
		int Update(AlmacenTransaccionConfiguracionDTO dto);
		void Delete(AlmacenTransaccionConfiguracionDTO dto);
		List<AlmacenTransaccionConfiguracionDTO> ListarPorPorAlmacenTransaccionId(AlmacenTransaccionConfiguracionDTO dto);
	}
}
