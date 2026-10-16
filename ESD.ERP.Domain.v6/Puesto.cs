using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
    [Table("Puesto")]
    public class Puesto : BaseEntity
    {
        public override int EntityId => PuestoId;
        [Key]
        [Column("puesto_id", Order = 1)]
        public int PuestoId { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

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
        
        public Contactos Contactos {  get; set; }
    }
}