using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRedSocialBusiness
	{
		RedSocialDTO GetById(int id);
		List<RedSocialDTO> Search(RedSocialDTO dto);
		RedSocialDTO SingleSearch(RedSocialDTO dto);
		int Insert(RedSocialDTO dto);
		int Update(RedSocialDTO dto);
		void Delete(RedSocialDTO dto);
	}
}
