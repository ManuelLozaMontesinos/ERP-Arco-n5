using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("lista_precio")]
	public class ListaPrecio : BaseEntity
	{
		public override int EntityId => ListaPrecioId;
		[Key]
		[Column("lista_precio_id", Order = 1)]
		public int ListaPrecioId { get; set; }

		[Column("empresa_sede_id")]
		public int EmpresaSedeId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

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

		public EmpresaSede EmpresaSede { get; set; }
		public ImpuestoTasa ImpuestoTasa { get; set; }
		public Moneda Moneda { get; set; }
	}
}