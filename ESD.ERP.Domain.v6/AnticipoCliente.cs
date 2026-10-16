using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("anticipo_cliente")]
	public class AnticipoCliente : BaseEntity
	{
		public override int EntityId => AnticipoClienteId;
		[Key]
		[Column("anticipo_cliente_id", Order = 1)]
		public int AnticipoClienteId { get; set; }

		[Column("empresa_sede_id")]
		public int EmpresaSedeId { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("tipo_car_abo_id")]
		public int TipoCargoAbonoId { get; set; }

		[Column("numero_opera")]
		public string NumeroOperacion { get; set; }
		[Column("secuencia")]
		public int Secuencia { get; set; }
		[Column("numero_grupo")]
		public string NumeroGrupo { get; set; }
		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("vendedor_id")]
		public int VendedorId { get; set; }

		[Column("medio_pago_id")]
		public int MedioPagoId { get; set; }

		[Column("banco_id")]
		public int? BancoId { get; set; }

		[Column("fecha_ref")]
		public DateTime? FechaReferencia { get; set; }

		[Column("numero_ref")]
		public string NumeroReferencia { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

		[Column("monto_aplica")]
		public decimal MontoAplicado { get; set; }

		[Column("tipo_ope_antic_id")]
		public int TipoOperacionAnticipoId { get; set; }

		[Column("tipo_doc_com_id")]
		public int? TipoDocumentoComercialId { get; set; }

		[Column("serie")]
		public string Serie { get; set; }

		[Column("comprobante_id")]
		public int? ComprobanteId { get; set; }

		[Column("cotiza_id")]
		public int? CotizacionId { get; set; }

		[Column("anticipo_cli_p_id")]
		public int? AnticipoClientePadreId { get; set; }
		[Column("extornado")]
		public string Extornado { get; set; }
		[Column("caja_id")]
		public int? CajaId { get; set; }
		[Column("observa")]
		public string Observacion { get; set; }
		[Column("estado_compro_id")]
		public int EstadoComprobanteId { get; set; }
		[Column("reg_en_coti")]
		public string RegistradoEnCotizacion { get; set; }
		[Column("estado")]
		public string Estado { get; set; }
		[Column("fecha_baja")]
		public DateTime? FechaBaja { get; set; }
		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public EmpresaSede EmpresaSede { get; set; }
		public Proveedor Proveedor { get; set; }
		public TipoCargoAbono TipoCargoAbono { get; set; }
		public Moneda Moneda { get; set; }
		public Vendedor Vendedor { get; set; }
		public MedioPago MedioPago { get; set; }
		public Banco Banco { get; set; }
		public TipoOperacionAnticipo TipoOperacionAnticipo { get; set; }
		public Comprobante Comprobante { get; set; }
		public Cotizacion Cotizacion { get; set; }
		public AnticipoCliente AnticipoClientePadre { get; set; }
		public EstadoComprobante EstadoComprobante { get; set; }
		public TipoDocumentoComercial TipoDocumentoComercial { get; set; }
		public Caja Caja { get; set; }

		public List<string> TipoOperacionAnticipoList { get; set; }
		public string ConSaldo { get; set; }
		public string NullCotizacionId { get; set; }
		public string NullFechaBaja { get; set; }
		public List<int> AnticipoClienteIdList { get; set; }
	}
}