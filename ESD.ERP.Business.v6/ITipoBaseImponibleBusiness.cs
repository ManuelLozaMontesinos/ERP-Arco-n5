using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoBaseImponibleBusiness
	{
		TipoBaseImponibleDTO GetById(int id);
		List<TipoBaseImponibleDTO> Search(TipoBaseImponibleDTO dto);
		TipoBaseImponibleDTO SingleSearch(TipoBaseImponibleDTO dto);
		int Insert(TipoBaseImponibleDTO dto);
		int Update(TipoBaseImponibleDTO dto);
		void Delete(TipoBaseImponibleDTO dto);
	}
}
