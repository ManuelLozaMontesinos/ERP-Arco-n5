using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
    public interface IListaPrecioVendedorRepository: IGenericRepository<ListaPrecioVendedor>
    {
        void ActualizarListaPrecioVendedorId(ListaPrecioVendedor entity);        
    }
}