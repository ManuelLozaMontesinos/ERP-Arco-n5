using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
    public interface IListaPrecioVendedorProxy
    {
        ListaPrecioVendedorDTO GetById(ListaPrecioVendedorDTO dto);
        List<ListaPrecioVendedorDTO> Search(ListaPrecioVendedorDTO dto);
        ListaPrecioVendedorDTO SingleSearch(ListaPrecioVendedorDTO dto);
        int Insert(ListaPrecioVendedorDTO dto);
        int Update(ListaPrecioVendedorDTO dto);
        void Delete(ListaPrecioVendedorDTO dto);
        void ActualizarListaPrecioVendedorId(ListaPrecioVendedorDTO dto);
    }
}
