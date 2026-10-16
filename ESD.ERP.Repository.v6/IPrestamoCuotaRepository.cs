using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IPrestamoCuotaRepository : IGenericRepository<PrestamoCuota>
	{
		void BulkMerge(List<PrestamoCuota> list);
	}
}
