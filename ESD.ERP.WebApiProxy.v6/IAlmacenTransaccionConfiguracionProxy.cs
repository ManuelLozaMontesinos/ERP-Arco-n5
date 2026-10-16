using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IAlmacenTransaccionConfiguracionProxy
	{
		AlmacenTransaccionConfiguracionDTO GetById(AlmacenTransaccionConfiguracionDTO dto);
		List<AlmacenTransaccionConfiguracionDTO> Search(AlmacenTransaccionConfiguracionDTO dto);
		AlmacenTransaccionConfiguracionDTO SingleSearch(AlmacenTransaccionConfiguracionDTO dto);
		int Insert(AlmacenTransaccionConfiguracionDTO dto);
		int Update(AlmacenTransaccionConfiguracionDTO dto);
		void Delete(AlmacenTransaccionConfiguracionDTO dto);
		List<AlmacenTransaccionConfiguracionDTO> ListarPorPorAlmacenTransaccionId(AlmacenTransaccionConfiguracionDTO dto);
	}
}
