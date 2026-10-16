using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("prove_lista_p")]
	public class ProveedorListaPrecio : BaseEntity
	{
		public override int EntityId => ProveedorListaPrecioId;
		[Key]
		[Column("prove_lista_p_id", Order = 1)]
		public int ProveedorListaPrecioId { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("impuesto_tasa_id")]
		public int ImpuestoTasaId { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

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
		public ImpuestoTasa ImpuestoTasa { get; set; }
		public Moneda Moneda { get; set; }
	}
}
