using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
   public interface IEstadoOrdenCompraBusiness
   {
      EstadoOrdenCompraDTO GetById(int id);
      List<EstadoOrdenCompraDTO> Search(EstadoOrdenCompraDTO dto);
      EstadoOrdenCompraDTO SingleSearch(EstadoOrdenCompraDTO dto);
      int Insert(EstadoOrdenCompraDTO dto);
      int Update(EstadoOrdenCompraDTO dto);
      void Delete(EstadoOrdenCompraDTO dto);
      EstadoOrdenCompraDTO ObtenerPorCodigo(string codigo);
   }
}
