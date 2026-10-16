using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;
using ESD.ERP.Common;

namespace ESD.ERP.DTO
{
	public class AnticipoClienteDTO : DTOBase
	{
		public override int Id => AnticipoClienteId;
		public int AnticipoClienteId { get; set; }
		public int EmpresaSedeId { get; set; }
		public int ProveedorId { get; set; }
		public DateTime Fecha { get; set; }
		public int TipoCargoAbonoId { get; set; }
		public string NumeroOperacion { get; set; }
		public int Secuencia { get; set; }
		public string NumeroGrupo { get; set; }
		public int MonedaId { get; set; }
		public int VendedorId { get; set; }
		public int MedioPagoId { get; set; }
		public int? BancoId { get; set; }
		public DateTime? FechaReferencia { get; set; }
		public string NumeroReferencia { get; set; }
		public decimal Monto { get; set; }
		public decimal MontoAplicado { get; set; }
		public decimal MontoSaldo { get { return Monto - MontoAplicado; } }
		public int TipoOperacionAnticipoId { get; set; }
		public int? TipoDocumentoComercialId { get; set; }
		public string Serie { get; set; }
		public int? ComprobanteId { get; set; }
		public int? CotizacionId { get; set; }
		public int? AnticipoClientePadreId { get; set; }
		public string Extornado { get; set; }
		public int? CajaId { get; set; }
		public string Observacion { get; set; }
		public int EstadoComprobanteId { get; set; }
		public string PermiteDividirMonto
      {
         get {
				if (CotizacionId == null)
					return MontoSaldo > decimal.Zero ? SiNoConstante.S : SiNoConstante.N;

				return (MontoAplicado > decimal.Zero && MontoSaldo > decimal.Zero) ? SiNoConstante.S : SiNoConstante.N;
			}
      }
		public string RegistradoEnCotizacion { get; set; }
		public string Estado { get; set; }
		public DateTime? FechaBaja { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public TipoCargoAbonoDTO TipoCargoAbonoDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public VendedorDTO VendedorDTO { get; set; }
		public MedioPagoDTO MedioPagoDTO { get; set; }
		public BancoDTO BancoDTO { get; set; }
		public TipoOperacionAnticipoDTO TipoOperacionAnticipoDTO { get; set; }
		public ComprobanteDTO ComprobanteDTO { get; set; }
		public CotizacionDTO CotizacionDTO { get; set; }
		public AnticipoClienteDTO AnticipoClientePadreDTO { get; set; }
		public EstadoComprobanteDTO EstadoComprobanteDTO { get; set; }
		public TipoDocumentoComercialDTO TipoDocumentoComercialDTO { get; set; }
		public CajaDTO CajaDTO { get; set; }
		public List<string> TipoOperacionAnticipoList { get; set; }
		public string ConSaldo { get; set; }
		public string NullCotizacionId { get; set; }
		public string NullFechaBaja { get; set; }
		public List<int> AnticipoClienteIdList { get; set; }
	}
}
