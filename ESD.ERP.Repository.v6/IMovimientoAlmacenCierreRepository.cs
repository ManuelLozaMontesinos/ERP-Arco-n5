using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IMovimientoAlmacenCierreRepository : IGenericRepository<MovimientoAlmacenCierre>
	{
		void Reprocesar(MovimientoAlmacenCierre entity);
   }
}
