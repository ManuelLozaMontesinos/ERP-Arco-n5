using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TransaccionAlmacenDTO : DTOBase
	{
		public override int Id => TransaccionAlmacenId;
		public int TransaccionAlmacenId { get; set; }
		public int EmpresaId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public int? TipoMovimientoAlmacenId { get; set; }
		public int OrigenRegistroId { get; set; }
		public string EsConfigurable { get; set; }
		public string EsCompra { get; set; }
		public string TieneMantenimiento { get; set; }
		public int? TipoOperacionId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoMovimientoAlmacenDTO TipoMovimientoAlmacenDTO { get; set; }
		public OrigenRegistroDTO OrigenRegistroDTO { get; set; }
		public TipoOperacionDTO TipoOperacionDTO { get; set; }

		public int NotTipoMovimientoAlmacenId { get; set; }
		public List<string> NotTransaccionAlmacenList { get; set; }
	}
}
