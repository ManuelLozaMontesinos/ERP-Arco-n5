using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITransportistaConductorProxy
	{
		TransportistaConductorDTO GetById(TransportistaConductorDTO dto);
		List<TransportistaConductorDTO> Search(TransportistaConductorDTO dto);
		TransportistaConductorDTO SingleSearch(TransportistaConductorDTO dto);
		int Insert(TransportistaConductorDTO dto);
		int Update(TransportistaConductorDTO dto);
		void Delete(TransportistaConductorDTO dto);
	}
}
