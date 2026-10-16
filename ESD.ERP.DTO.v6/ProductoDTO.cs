using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProductoDTO : DTOBase
	{
		public override int Id => ProductoId;
		public int ProductoId { get; set; }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
		public string CodigoReferencial { get; set; }
		public int FamiliaProductoId { get; set; }
		public int UnidadMedidaId { get; set; }
		public int TipoIgvId { get; set; }
		public decimal PesoNeto { get; set; }
		public decimal PesoBruto { get; set; }
		public decimal ValorReposicion { get; set; }
		public decimal Volumen { get; set; }
		public string TienePresentacionVenta { get; set; }
		public string EsOperacion { get; set; }
		public string EsPerecible { get; set; }
		public string DetalleRequerido { get; set; }
		public int MasterPack { get; set; }
		public int? ColorId { get; set; }
		public int EstadoProductoId { get; set; }
		public int EstadoComercialProductoId { get; set; }
		public string Estado { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public FamiliaProductoDTO FamiliaProductoDTO { get; set; }
		public UnidadMedidaDTO UnidadMedidaDTO { get; set; }
		public TipoIgvDTO TipoIgvDTO { get; set; }
		public ColorDTO ColorDTO { get; set; }
		public EstadoProductoDTO EstadoProductoDTO { get; set; }
		public EstadoComercialProductoDTO EstadoComercialProductoDTO { get; set; }
		public int Periodo { get; set; }
		public int CortesGratis {  get; set; }
		public List<string> ProductoList { get; set; }
}
}
