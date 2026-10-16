using System.Collections.Generic;
using ESD.ERP.Business;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
    public interface IContactosBusiness
    {
        ContactosDTO GetById(int id);
        List<ContactosDTO> Search(ContactosDTO dto);
        ContactosDTO SingleSearch(ContactosDTO dto);
        int Insert(ContactosDTO dto);
        int Update(ContactosDTO dto);
        void Delete(ContactosDTO dto);
    }
}