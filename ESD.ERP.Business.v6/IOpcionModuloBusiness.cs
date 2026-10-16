using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOpcionModuloBusiness
	{
		OpcionModuloDTO GetById(int id);
		List<OpcionModuloDTO> Search(OpcionModuloDTO dto);
		OpcionModuloDTO SingleSearch(OpcionModuloDTO dto);
		int Insert(OpcionModuloDTO dto);
		int Update(OpcionModuloDTO dto);
		void Delete(OpcionModuloDTO dto);
	}
}
