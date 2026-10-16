using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
    public class FamiliaProductoDTO : DTOBase
    {
        public override int Id => FamiliaProductoId;
        public int FamiliaProductoId { get; set; }
        public int ClaseProductoId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public short Correlativo { get; set; }
        public string Estado { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public ClaseProductoDTO ClaseProductoDTO { get; set; }
        public int NotFamiliaProductoId { get; set; }
        public int Periodo { get; set; }
    }
}