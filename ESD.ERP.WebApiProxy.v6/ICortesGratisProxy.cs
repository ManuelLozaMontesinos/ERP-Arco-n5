using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
    public interface ICortesGratisProxy
    {
        CortesGratisDTO GetById(CortesGratisDTO dto);
        List<CortesGratisDTO> Search(CortesGratisDTO dto);
        CortesGratisDTO SingleSearch(CortesGratisDTO dto);
        int Insert(CortesGratisDTO dto);
        int Update(CortesGratisDTO dto);
    }
}
