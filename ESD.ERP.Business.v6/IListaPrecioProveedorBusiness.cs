using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
    public interface IListaPrecioProveedorBusiness
    {
        ListaPrecioProveedorDTO GetById(int id);
        List<ListaPrecioProveedorDTO> Search(ListaPrecioProveedorDTO dto);
        ListaPrecioProveedorDTO SingleSearch(ListaPrecioProveedorDTO dto);
        int Insert(ListaPrecioProveedorDTO dto);
        int Update(ListaPrecioProveedorDTO dto);
        void Delete(ListaPrecioProveedorDTO dto);
        List<ListaPrecioProveedorDTO> ListarPorProveedorId(ListaPrecioProveedorDTO dto);
    }
}
