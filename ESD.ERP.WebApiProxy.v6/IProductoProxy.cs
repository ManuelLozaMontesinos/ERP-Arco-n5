using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
    public interface IProductoProxy
    {
        ProductoDTO GetById(ProductoDTO dto);
        List<ProductoDTO> Search(ProductoDTO dto);
        ProductoDTO SingleSearch(ProductoDTO dto);
        int Insert(ProductoDTO dto);
        int Update(ProductoDTO dto);
        void Delete(ProductoDTO dto);
        void ActualizarPesoMasterPack(ProductoDTO dto);
    }
}
