using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoPrestamoProxy
	{
		TipoPrestamoDTO GetById(TipoPrestamoDTO dto);
		List<TipoPrestamoDTO> Search(TipoPrestamoDTO dto);
		TipoPrestamoDTO SingleSearch(TipoPrestamoDTO dto);
		int Insert(TipoPrestamoDTO dto);
		int Update(TipoPrestamoDTO dto);
		void Delete(TipoPrestamoDTO dto);
	}
}
