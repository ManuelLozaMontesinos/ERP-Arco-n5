using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
    [Table("familia_producto")]
    public class FamiliaProducto : BaseEntity
    {
        public override int EntityId => FamiliaProductoId;
        [Key]
        [Column("familia_producto_id", Order = 1)]
        public int FamiliaProductoId { get; set; }

        [Column("clase_producto_id")]
        public int ClaseProductoId { get; set; }

        [Column("familia_producto")]
        public string Codigo { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("correla")]
        public short Correlativo { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        [Column("fecha_alta")]
        public DateTime FechaAlta { get; set; }

        [Column("fecha_baja")]
        public DateTime? FechaBaja { get; set; }

        [Column("usuario_creacion_id")]
        public int UsuarioCreacionId { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("usuario_modifica_id")]
        public int? UsuarioModificacionId { get; set; }

        [Column("fecha_modifica")]
        public DateTime? FechaModificacion { get; set; }

        public ClaseProducto ClaseProducto { get; set; }
        public int NotFamiliaProductoId { get; set; }
        public int Periodo { get; set; }
    }
}