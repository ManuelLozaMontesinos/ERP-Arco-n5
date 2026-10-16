using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoCargoAbonoBusiness
	{
		TipoCargoAbonoDTO GetById(int id);
		List<TipoCargoAbonoDTO> Search(TipoCargoAbonoDTO dto);
		TipoCargoAbonoDTO SingleSearch(TipoCargoAbonoDTO dto);
		int Insert(TipoCargoAbonoDTO dto);
		int Update(TipoCargoAbonoDTO dto);
		void Delete(TipoCargoAbonoDTO dto);
		TipoCargoAbonoDTO ObtenerPorCodigo(string codigo);
	}
}
