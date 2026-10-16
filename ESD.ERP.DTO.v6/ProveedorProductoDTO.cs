using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProveedorProductoDTO : DTOBase
	{
		public override int Id => ProveedorProductoId;
		public int ProveedorProductoId { get; set; }
		public int ProveedorId { get; set; }
		public int ProductoId { get; set; }
		public string CodigoReferencial { get; set; }
		public decimal PesoNeto { get; set; }
		public decimal PesoBruto { get; set; }
		public int MasterPack { get; set; }
		public decimal Volumen { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
	}
}
