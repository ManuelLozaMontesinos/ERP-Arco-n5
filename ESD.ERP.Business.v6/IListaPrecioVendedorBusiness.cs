using ESD.ERP.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ESD.ERP.Business
{
    public interface IListaPrecioVendedorBusiness
    {
        ListaPrecioVendedorDTO GetById(int id);
        List<ListaPrecioVendedorDTO> Search(ListaPrecioVendedorDTO dto);
        ListaPrecioVendedorDTO SingleSearch(ListaPrecioVendedorDTO dto);
        int Insert(ListaPrecioVendedorDTO dto);
        int Update(ListaPrecioVendedorDTO dto);
        void Delete(ListaPrecioVendedorDTO dto);
        void ActualizarListaPrecioVendedorId(ListaPrecioVendedorDTO dto);
    }
}
