using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IModuloBusiness
	{
		ModuloDTO GetById(int id);
		List<ModuloDTO> Search(ModuloDTO dto);
		ModuloDTO SingleSearch(ModuloDTO dto);
		int Insert(ModuloDTO dto);
		int Update(ModuloDTO dto);
		void Delete(ModuloDTO dto);
	}
}
