using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
    public class CortesGratisDTO : DTOBase
    {
        public override int Id => CortesGratisId;
        public int CortesGratisId { get; set; }
        public int ProductoId { get; set; }
        public int EmpresaSedeId { get; set; }
        public decimal Cantidad { get; set; }
        public string Estado { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        
        public ProductoDTO ProductoDTO{ get; set; }
        public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
    }
}