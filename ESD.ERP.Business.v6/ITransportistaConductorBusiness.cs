using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITransportistaConductorBusiness
	{
		TransportistaConductorDTO GetById(int id);
		List<TransportistaConductorDTO> Search(TransportistaConductorDTO dto);
		TransportistaConductorDTO SingleSearch(TransportistaConductorDTO dto);
		int Insert(TransportistaConductorDTO dto);
		int Update(TransportistaConductorDTO dto);
		void Delete(TransportistaConductorDTO dto);
	}
}
