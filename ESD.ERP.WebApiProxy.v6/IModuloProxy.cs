using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IModuloProxy
	{
		ModuloDTO GetById(ModuloDTO dto);
		List<ModuloDTO> Search(ModuloDTO dto);
		ModuloDTO SingleSearch(ModuloDTO dto);
		int Insert(ModuloDTO dto);
		int Update(ModuloDTO dto);
		void Delete(ModuloDTO dto);
	}
}
