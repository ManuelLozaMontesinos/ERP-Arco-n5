using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
   [Table("usuario")]
   public class Usuario : BaseEntity
   {
      public override int EntityId => UsuarioId;
      [Key]
      [Column("usuario_id", Order = 1)]
      public int UsuarioId { get; set; }

      [Column("empresa_id")]
      public int? EmpresaId { get; set; }

      [Column("usuario_logon")]
      public string UsuarioLogon { get; set; }

      [Column("nombre_completo")]
      public string NombreCompleto { get; set; }

      [Column("password")]
      public string Password { get; set; }

      [Column("clave_publica")]
      public byte[] ClavePublica { get; set; }

      [Column("cambiar_psw_proximo")]
      public string CambiarContrasenaProximo { get; set; }

      [Column("no_puede_cambiar_psw")]
      public string NoPuedeCambiarContrasena { get; set; }

      [Column("no_expira")]
      public string NoExpira { get; set; }

      [Column("fecha_expira")]
      public DateTime? FechaExpiracion { get; set; }

      [Column("email")]
      public string Email { get; set; }

      [Column("email_alt")]
      public string EmailAlterno { get; set; }

      [Column("codigo_recupera_psw")]
      public string CodigoRecuperacionContrasena { get; set; }

      [Column("tiene_horario")]
      public string TieneHorario { get; set; }

      [Column("origen_registro_id")]
      public int OrigenRegistroId { get; set; }

      [Column("estado")]
      public string Estado { get; set; }

      public OrigenRegistro OrigenRegistro { get; set; }
   }
}
