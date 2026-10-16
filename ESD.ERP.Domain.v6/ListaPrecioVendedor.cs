using ESD.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESD.ERP.Domain
{
    [Table("lista_p_vendedor")]
    public class ListaPrecioVendedor : BaseEntity
    {
        public override int EntityId => ListaPrecioVendedorId;
        [Key]
        [Column("lista_p_vendedor_id", Order = 1)]
        public int ListaPrecioVendedorId { get; set; }

        [Column("lista_precio_id")]
        public int ListaPrecioId { get; set; }

        [Column("vendedor_id")]
        public int VendedorId { get; set; }

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

        public ListaPrecio ListaPrecio { get; set; }
        public Vendedor Vendedor{ get; set; }
        public List<int> ListaPrecioIdList { get; set; }

    }
}
