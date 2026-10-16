using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoPrestamoBusiness
	{
		TipoPrestamoDTO GetById(int id);
		List<TipoPrestamoDTO> Search(TipoPrestamoDTO dto);
		TipoPrestamoDTO SingleSearch(TipoPrestamoDTO dto);
		int Insert(TipoPrestamoDTO dto);
		int Update(TipoPrestamoDTO dto);
		void Delete(TipoPrestamoDTO dto);
	}
}
