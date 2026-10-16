using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class DocumentoComercialSerieDTO : DTOBase
	{
		public override int Id => DocumentoComercialSerieId;
		public int DocumentoComercialSerieId { get; set; }
		public int EmpresaSedeId { get; set; }
		public int TipoDocumentoComercialId { get; set; }
		public string Serie { get; set; }
		public int Correlativo { get; set; }
		public string EsContingencia { get; set; }
		public string EnvioEnLinea { get; set; }
		public string Formato { get; set; }
		public short MaximoItemImpresion { get; set; }
		public string ArchivoFormato { get; set; }
		public int? TipoDocumentoComercialRelacionadoId { get; set; }
		public int? AlmacenId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
		public TipoDocumentoComercialDTO TipoDocumentoComercialDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public string AlmacenIdNotNull { get; set; }
	}
}
