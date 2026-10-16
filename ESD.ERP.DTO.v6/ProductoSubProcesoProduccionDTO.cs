using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProductoSubProcesoProduccionDTO : DTOBase
	{
		public override int Id => ProductoSubProcesoProduccionId;
		public int ProductoSubProcesoProduccionId { get; set; }
		public int ProductoId { get; set; }
		public int SubProcesoProduccionId { get; set; }
		public int Orden { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public SubProcesoProduccionDTO SubProcesoProduccionDTO { get; set; }
		public List<int> ProductoIdList { get; set; }
	}
}
