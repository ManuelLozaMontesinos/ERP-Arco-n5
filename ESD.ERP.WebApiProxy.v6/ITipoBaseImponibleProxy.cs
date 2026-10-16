using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoBaseImponibleProxy
	{
		TipoBaseImponibleDTO GetById(TipoBaseImponibleDTO dto);
		List<TipoBaseImponibleDTO> Search(TipoBaseImponibleDTO dto);
		TipoBaseImponibleDTO SingleSearch(TipoBaseImponibleDTO dto);
		int Insert(TipoBaseImponibleDTO dto);
		int Update(TipoBaseImponibleDTO dto);
		void Delete(TipoBaseImponibleDTO dto);
	}
}
