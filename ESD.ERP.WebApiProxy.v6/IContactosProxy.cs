using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
    public interface IContactosProxy
    {
        ContactosDTO GetById(ContactosDTO dto);
        List<ContactosDTO> Search(ContactosDTO dto);
        ContactosDTO SingleSearch(ContactosDTO dto);
        int Insert(ContactosDTO dto);
        int Update(ContactosDTO dto);
        void Delete (ContactosDTO dto);
    }
}
