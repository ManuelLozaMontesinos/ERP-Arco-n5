using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICuadroCostoBusiness
	{
		CuadroCostoDTO GetById(int id);
		List<CuadroCostoDTO> Search(CuadroCostoDTO dto);
		CuadroCostoDTO SingleSearch(CuadroCostoDTO dto);
		int Insert(CuadroCostoDTO dto);
		int Update(CuadroCostoDTO dto);
		void Delete(CuadroCostoDTO dto);
	}
}
