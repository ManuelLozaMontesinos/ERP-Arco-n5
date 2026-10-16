using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IRolMenuRepository : IGenericRepository<RolMenu>
	{
		List<RolMenu> ListarPorRolId(RolMenu entity);
		void ActualizarLote(RolMenu entity);
	}
}
