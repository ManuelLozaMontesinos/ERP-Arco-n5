using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOpcionModuloProxy
	{
		OpcionModuloDTO GetById(OpcionModuloDTO dto);
		List<OpcionModuloDTO> Search(OpcionModuloDTO dto);
		OpcionModuloDTO SingleSearch(OpcionModuloDTO dto);
		int Insert(OpcionModuloDTO dto);
		int Update(OpcionModuloDTO dto);
		void Delete(OpcionModuloDTO dto);
	}
}
