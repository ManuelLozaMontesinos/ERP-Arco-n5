using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICuadroCostoProxy
	{
		List<CuadroCostoDTO> Search(CuadroCostoDTO dto);
		CuadroCostoDTO SingleSearch(CuadroCostoDTO dto);
	}
}
