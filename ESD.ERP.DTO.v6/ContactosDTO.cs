using System;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
    public class ContactosDTO : DTOBase
    {
        public override int Id => ContactosId;
        public int ContactosId { get; set; }
        public int ProveedorId {  get; set; }
        public int PuestoId { get; set; }
        public string NombreC {  get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int TipoDocumentoIdentidadId { get; set; }
        public string NumeroDocumentoIdentidadC { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public ProveedorDTO ProveedorDTO { get; set; }
        public TipoDocumentoIdentidadDTO TipoDocumentoIdentidadDTO { get; set; }
        public PuestoDTO PuestoDTO { get; set; }
    }
}