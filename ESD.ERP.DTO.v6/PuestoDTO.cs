using System;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
    public class PuestoDTO : DTOBase
    {
        public override int Id => PuestoId;
        public int PuestoId { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        
        public ContactosDTO ContactosDTO { get; set; }
    }
}