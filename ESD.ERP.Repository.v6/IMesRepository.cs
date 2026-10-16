using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
   public interface IMesRepository : IGenericRepository<Mes>
   {
      Mes ObtenerPorCodigo(string codigo);
   }
}
