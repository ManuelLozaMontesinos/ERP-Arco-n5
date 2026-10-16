using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("empresa_cta")]
	public class EmpresaCuentaBancaria : BaseEntity
	{
		public override int EntityId => EmpresaCuentaBancariaId;
		[Key]
		[Column("empresa_cta_id", Order = 1)]
		public int EmpresaCuentaBancariaId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("banco_id")]
		public int BancoId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("cuenta")]
		public string Cuenta { get; set; }

		[Column("cta_interbanca")]
		public string CuentaInterbancaria { get; set; }

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

		public Empresa Empresa { get; set; }
		public Banco Banco { get; set; }
		public Moneda Moneda { get; set; }
	}
}
