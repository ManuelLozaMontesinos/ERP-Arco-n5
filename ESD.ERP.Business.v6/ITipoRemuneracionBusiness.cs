using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoRemuneracionBusiness
	{
		TipoRemuneracionDTO GetById(int id);
		List<TipoRemuneracionDTO> Search(TipoRemuneracionDTO dto);
		TipoRemuneracionDTO SingleSearch(TipoRemuneracionDTO dto);
		int Insert(TipoRemuneracionDTO dto);
		int Update(TipoRemuneracionDTO dto);
		void Delete(TipoRemuneracionDTO dto);
	}
}
