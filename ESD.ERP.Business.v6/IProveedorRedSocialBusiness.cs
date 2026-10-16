using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProveedorRedSocialBusiness
	{
		ProveedorRedSocialDTO GetById(int id);
		List<ProveedorRedSocialDTO> Search(ProveedorRedSocialDTO dto);
		ProveedorRedSocialDTO SingleSearch(ProveedorRedSocialDTO dto);
		int Insert(ProveedorRedSocialDTO dto);
		int Update(ProveedorRedSocialDTO dto);
		void Delete(ProveedorRedSocialDTO dto);
	}
}
