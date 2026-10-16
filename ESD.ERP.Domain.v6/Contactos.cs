using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
    [Table("contactos")]
    public class Contactos : BaseEntity
    {
        public override int EntityId => ContactosId;
        [Key]
        [Column("contactos_id", Order = 1)]
        public int ContactosId { get; set; }
       
        [Column("proveedor_id")]
        public int ProveedorId { get; set; }

        [Column("puesto_id")]
        public int PuestoId { get; set; }

        [Column("nombre")]
        public string NombreC { get; set; }

        [Column("apellido_pat")]
        public string ApellidoPaterno { get; set; }

        [Column("apellido_mat")]
        public string ApellidoMaterno { get; set; }

        [Column("tipo_doc_iden_id")]
        public int TipoDocumentoIdentidadId { get; set; }

        [Column("num_doc_iden")]
        public string NumeroDocumentoIdentidadC { get; set; }

        [Column("telefono")]
        public string Telefono { get; set; }

        [Column("correo")]
        public string Correo { get; set; }

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
        public Proveedor Proveedor { get; set; }
        public TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }
        public Puesto Puesto {  get; set; }
    }
}