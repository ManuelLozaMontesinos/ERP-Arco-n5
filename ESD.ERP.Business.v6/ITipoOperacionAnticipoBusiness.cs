using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoOperacionAnticipoBusiness
	{
		TipoOperacionAnticipoDTO GetById(int id);
		List<TipoOperacionAnticipoDTO> Search(TipoOperacionAnticipoDTO dto);
		TipoOperacionAnticipoDTO SingleSearch(TipoOperacionAnticipoDTO dto);
		int Insert(TipoOperacionAnticipoDTO dto);
		int Update(TipoOperacionAnticipoDTO dto);
		void Delete(TipoOperacionAnticipoDTO dto);
		TipoOperacionAnticipoDTO ObtenerPorCodigo(string codigo);
	}
}
