using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
    [Table("cortes_gratis")]
    public class CortesGratis : BaseEntity
    {
        public override int EntityId => CortesGratisId;
        [Key]
        [Column("cortes_gratis_id", Order = 1)]
        public int CortesGratisId { get; set; }

        [Column("producto_id")]
        public int ProductoId{ get; set; }

        [Column("empresa_sede_id")]
        public int EmpresaSedeId { get; set; }

        [Column("cantidad")]
        public decimal Cantidad { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        [Column("usuario_creacion_id")]
        public int UsuarioCreacionId { get; set; }
        
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("usuario_modifica_id")]
        public int? UsuarioModificacionId { get; set; }

        [Column("fecha_modifica")]
        public DateTime? FechaModificacion { get; set; }
                        
        public Producto Producto{ get; set; }
        public EmpresaSede EmpresaSede { get; set; }
    }
}