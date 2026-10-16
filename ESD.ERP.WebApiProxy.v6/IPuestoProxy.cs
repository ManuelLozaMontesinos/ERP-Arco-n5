using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
    public interface IPuestoProxy
    {
        PuestoDTO GetById(PuestoDTO dto);
        List<PuestoDTO> Search(PuestoDTO dto);
        PuestoDTO SingleSearch(PuestoDTO dto);
        int Insert(PuestoDTO dto);
        int Update(PuestoDTO dto);
    }
}
