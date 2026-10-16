using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoCargoAbonoProxy
	{
		TipoCargoAbonoDTO GetById(TipoCargoAbonoDTO dto);
		List<TipoCargoAbonoDTO> Search(TipoCargoAbonoDTO dto);
		TipoCargoAbonoDTO SingleSearch(TipoCargoAbonoDTO dto);
		int Insert(TipoCargoAbonoDTO dto);
		int Update(TipoCargoAbonoDTO dto);
		void Delete(TipoCargoAbonoDTO dto);
		TipoCargoAbonoDTO ObtenerPorCodigo(TipoCargoAbonoDTO dto);
	}
}
