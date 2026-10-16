using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProveedorRedSocialProxy
	{
		ProveedorRedSocialDTO GetById(ProveedorRedSocialDTO dto);
		List<ProveedorRedSocialDTO> Search(ProveedorRedSocialDTO dto);
		ProveedorRedSocialDTO SingleSearch(ProveedorRedSocialDTO dto);
		int Insert(ProveedorRedSocialDTO dto);
		int Update(ProveedorRedSocialDTO dto);
		void Delete(ProveedorRedSocialDTO dto);
	}
}
