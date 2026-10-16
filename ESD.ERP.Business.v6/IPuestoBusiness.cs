using System.Collections.Generic;
using ESD.ERP.Business;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
    public interface IPuestoBusiness
    {
        PuestoDTO GetById(int id);
        List<PuestoDTO> Search(PuestoDTO dto);
        PuestoDTO SingleSearch(PuestoDTO dto);
        int Insert(PuestoDTO dto);
        int Update(PuestoDTO dto);
        void Delete(PuestoDTO dto);
    }
}