using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoAutorizacionModuloBusiness
	{
		TipoAutorizacionModuloDTO GetById(int id);
		List<TipoAutorizacionModuloDTO> Search(TipoAutorizacionModuloDTO dto);
		TipoAutorizacionModuloDTO SingleSearch(TipoAutorizacionModuloDTO dto);
		int Insert(TipoAutorizacionModuloDTO dto);
		int Update(TipoAutorizacionModuloDTO dto);
		void Delete(TipoAutorizacionModuloDTO dto);
	}
}
