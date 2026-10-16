using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoDocumentoComercialDTO : DTOBase
	{
		public override int Id => TipoDocumentoComercialId;
		public int TipoDocumentoComercialId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Abreviatura { get; set; }
		public string CodigoSunat { get; set; }
		public string EsInterno { get; set; }
		public string EsComprobantePago { get; set; }
		public string EsElectronico { get; set; }
		public string EsCanjeable { get; set; }
		public decimal MontoMaximoSinProveedor { get; set; }
		public string ValidarProveedor { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public List<string> TipoDocumentoComercialList { get; set; }
	}
}
