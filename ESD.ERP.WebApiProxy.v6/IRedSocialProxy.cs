using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRedSocialProxy
	{
		RedSocialDTO GetById(RedSocialDTO dto);
		List<RedSocialDTO> Search(RedSocialDTO dto);
		RedSocialDTO SingleSearch(RedSocialDTO dto);
		int Insert(RedSocialDTO dto);
		int Update(RedSocialDTO dto);
		void Delete(RedSocialDTO dto);
	}
}
