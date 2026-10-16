using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoAutorizacionModuloProxy
	{
		TipoAutorizacionModuloDTO GetById(TipoAutorizacionModuloDTO dto);
		List<TipoAutorizacionModuloDTO> Search(TipoAutorizacionModuloDTO dto);
		TipoAutorizacionModuloDTO SingleSearch(TipoAutorizacionModuloDTO dto);
		int Insert(TipoAutorizacionModuloDTO dto);
		int Update(TipoAutorizacionModuloDTO dto);
		void Delete(TipoAutorizacionModuloDTO dto);
	}
}
