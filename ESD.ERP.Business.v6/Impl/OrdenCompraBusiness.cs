using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OrdenCompraBusiness : IOrdenCompraBusiness
	{
		private readonly IOrdenCompraRepository ordenCompraRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IOrdenCompraProductoRepository ordenCompraProductoRepository;
		private readonly IOrdenCompraEntregaRepository ordenCompraEntregaRepository;
		private readonly IEstadoOrdenCompraRepository estadoOrdenCompraRepository;
		private readonly IPedidoCompraRepository pedidoCompraRepository;
		private readonly IPedidoCompraProductoRepository pedidoCompraProductoRepository;
		private readonly IEstadoPedidoCompraRepository estadoPedidoCompraRepository;
		private readonly IEstadoAprobacionRepository estadoAprobacionRepository;
		private readonly IProveedorListaPrecioRepository proveedorListaPrecioRepository;
		private readonly IProveedorListaPrecioProductoRepository proveedorListaPrecioProductoRepository;
		public OrdenCompraBusiness(
			IOrdenCompraRepository ordenCompraRepository,
			ITablaBusiness tablaBusiness,
			IOrdenCompraProductoRepository ordenCompraProductoRepository,
			IOrdenCompraEntregaRepository ordenCompraEntregaRepository,
			IEstadoOrdenCompraRepository estadoOrdenCompraRepository,
			IPedidoCompraRepository pedidoCompraRepository,
			IPedidoCompraProductoRepository pedidoCompraProductoRepository,
			IEstadoPedidoCompraRepository estadoPedidoCompraRepository,
			IEstadoAprobacionRepository estadoAprobacionRepository,
			IProveedorListaPrecioRepository proveedorListaPrecioRepository,
			IProveedorListaPrecioProductoRepository proveedorListaPrecioProductoRepository
			)
		{
			this.ordenCompraRepository = ordenCompraRepository;
			this.tablaBusiness = tablaBusiness;
			this.ordenCompraProductoRepository = ordenCompraProductoRepository;
			this.ordenCompraEntregaRepository = ordenCompraEntregaRepository;
			this.estadoOrdenCompraRepository = estadoOrdenCompraRepository;
			this.pedidoCompraRepository = pedidoCompraRepository;
			this.pedidoCompraProductoRepository = pedidoCompraProductoRepository;
			this.estadoPedidoCompraRepository = estadoPedidoCompraRepository;
			this.estadoAprobacionRepository = estadoAprobacionRepository;
			this.proveedorListaPrecioRepository = proveedorListaPrecioRepository;
			this.proveedorListaPrecioProductoRepository = proveedorListaPrecioProductoRepository;
		}
		public OrdenCompraDTO GetById(int id)
		{
			try
			{
				OrdenCompra ordenCompra = ordenCompraRepository.GetById(id);

				OrdenCompraDTO ordenCompraDTO = Mapper.Map<OrdenCompraDTO>(ordenCompra);

				return ordenCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenCompraDTO> Search(OrdenCompraDTO dto)
		{
			try
			{
				List<OrdenCompra> ordenCompraList = ordenCompraRepository.Search(Mapper.Map<OrdenCompra>(dto));

				List<OrdenCompraDTO> ordenCompraDTOList = Mapper.Map<List<OrdenCompraDTO>>(ordenCompraList);

				return ordenCompraDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenCompraDTO SingleSearch(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = ordenCompraRepository.SingleSearch(Mapper.Map<OrdenCompra>(dto));

				OrdenCompraDTO ordenCompraDTO = Mapper.Map<OrdenCompraDTO>(ordenCompra);

				return ordenCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = Mapper.Map<OrdenCompra>(dto);

				string tipoOrden = string.Empty;
				if (ordenCompra.TipoProducto.Codigo == TipoProductoConstante.PRODUCTO)
					tipoOrden = TablaConstante.ORDEN_COMPRA;
				else
					tipoOrden = TablaConstante.ORDEN_SERVICIO;

				int newId = 0;
				using (TransactionScope tx = new())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new()
               {
						Tabla = tipoOrden,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							PeriodoEmpresaSedeId = ordenCompra.PeriodoEmpresaSedeId
						},
						UsuarioAccionId = ordenCompra.UsuarioCreacionId
					};
					tablaCorrelativoDTO.UsarSedeCentral = true;

					ordenCompra.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = ordenCompraRepository.Insert(ordenCompra);

               if (ordenCompra.PedidoCompraId.HasValue)
               {

						ProveedorListaPrecio proveedorListaPrecio = proveedorListaPrecioRepository.SingleSearch(new ProveedorListaPrecio() { 
							ProveedorId = ordenCompra.ProveedorId,
							MonedaId = ordenCompra.MonedaId,
							Estado = EstadoConstante.ACTIVO
						});

						List<PedidoCompraProducto> pedidoCompraProductoList = pedidoCompraProductoRepository.Search(new PedidoCompraProducto()
						{
							PedidoCompraId = dto.PedidoCompraId.Value,
							StockEnProveedor = SiNoConstante.S,
							Estado = EstadoConstante.ACTIVO
						});

						List<ProveedorListaPrecioProducto> proveedorListaPrecioProductoList = new();

						if(proveedorListaPrecio != null)
							proveedorListaPrecioProductoList = proveedorListaPrecioProductoRepository.Search(
								new ProveedorListaPrecioProducto(){
									ProveedorListaPrecioId = proveedorListaPrecio.EntityId,
									ProductoIdList = pedidoCompraProductoList.Select(x => x.ProductoId.Value).ToList(),
									Estado = EstadoConstante.ACTIVO
								}
							);

						foreach (PedidoCompraProducto pedidoCompraProducto in pedidoCompraProductoList)
                  {

							OrdenCompraProducto ordenCompraProducto = new()
							{
								OrdenCompraId = newId,
								ProductoId = pedidoCompraProducto.ProductoId.Value,
								Descripcion = string.Empty,
								Cantidad = pedidoCompraProducto.Cantidad,
								EsAdicional = SiNoConstante.N,
								UsuarioCreacionId = dto.UsuarioCreacionId
							};

							ProveedorListaPrecioProducto proveedorListaPrecioProducto = proveedorListaPrecioProductoList.Where(x => x.ProductoId ==
								ordenCompraProducto.ProductoId).FirstOrDefault();

							if(proveedorListaPrecioProducto != null)
                     {
								ordenCompraProducto.ValorUnitario = proveedorListaPrecioProducto.ValorUnitario;
								ordenCompraProducto.PrecioUnitario = proveedorListaPrecioProducto.PrecioUnitario;

								CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO() { 
									PrecioUnitario = ordenCompraProducto.PrecioUnitario,
									Cantidad = ordenCompraProducto.Cantidad,
									TipoIGV = proveedorListaPrecioProducto.Producto.TipoIgv.Codigo,
									ImpuestoTasa = ordenCompra.ImpuestoTasa.Tasa
								}, 4);

								ordenCompraProducto.MontoBruto = calculadorVentaItemDTO.MontoBruto;
								ordenCompraProducto.MontoImpuesto = calculadorVentaItemDTO.MontoImpuesto;
								ordenCompraProducto.MontoTotal = calculadorVentaItemDTO.MontoTotal;
							}

							ordenCompraProductoRepository.Insert(ordenCompraProducto);
						}

						EstadoPedidoCompra estadoPedidoCompra = estadoPedidoCompraRepository.ObtenerPorCodigo(EstadoPedidoCompraConstante.CON_ORDEN_DE_COMPRA);

						pedidoCompraRepository.ActualizarEstadoPedido(new PedidoCompra() { 
							PedidoCompraId = dto.PedidoCompraId.Value,
							EstadoPedidoCompraId = estadoPedidoCompra.EntityId,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});

						ordenCompraRepository.ActualizarMonto(new OrdenCompra()
						{
							OrdenCompraId = newId,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});
					}

					dto.OrdenCompraId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = Mapper.Map<OrdenCompra>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = ordenCompraRepository.Update(ordenCompra);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = ordenCompraRepository.GetById(dto.OrdenCompraId);

				EstadoOrdenCompra estadoOrdenCompra = estadoOrdenCompraRepository.ObtenerPorCodigo(EstadoOrdenCompraConstante.ANULADA);
				ordenCompra.EstadoOrdenCompraId = estadoOrdenCompra.EstadoOrdenCompraId;

				using (TransactionScope tx = new())
				{
					ordenCompraRepository.Delete(ordenCompra);

					ordenCompraProductoRepository.EliminarPorOrdenCompraId(new OrdenCompraProducto() { 
						OrdenCompraId = ordenCompra.EntityId,
						UsuarioCreacionId = ordenCompra.UsuarioCreacionId
					});

					ordenCompraEntregaRepository.EliminarPorOrdenCompraId(new OrdenCompraEntrega()
					{
						OrdenCompraProducto = new OrdenCompraProducto()
                  {
							OrdenCompraId = ordenCompra.EntityId,
						},
						UsuarioCreacionId = ordenCompra.UsuarioCreacionId
					});

					if (ordenCompra.PedidoCompraId.HasValue)
					{
						EstadoPedidoCompra estadoPedidoCompra = estadoPedidoCompraRepository.ObtenerPorCodigo(EstadoPedidoCompraConstante.PEDIDO);

						pedidoCompraRepository.ActualizarEstadoPedido(new PedidoCompra()
						{
							PedidoCompraId = ordenCompra.PedidoCompraId.Value,
							EstadoPedidoCompraId = estadoPedidoCompra.EntityId,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});
					}
					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void SolicitarAutorizacion(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = Mapper.Map<OrdenCompra>(dto);

				List<OrdenCompraProducto> ordenCompraProductoList = ordenCompraProductoRepository.Search(new OrdenCompraProducto()
				{
					OrdenCompraId = dto.OrdenCompraId,
					Estado = EstadoConstante.ACTIVO
				});

				if (ordenCompraProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_orden_compra_sin_items"));

				int totalSinCantidad = ordenCompraProductoList.Where(x => x.Cantidad == decimal.Zero).Count();
				if (totalSinCantidad > 0)
					throw new Exception(SR.GetString("EX_orden_compra_items_cantidad_cero"));

				int totalSinPrecio = ordenCompraProductoList.Where(x => x.PrecioUnitario == decimal.Zero).Count();
				if (totalSinPrecio > 0)
					throw new Exception(SR.GetString("EX_orden_compra_items_precio_cero"));

				EstadoOrdenCompra estadoOrdenCompra = null;
				string esModificacion = SiNoConstante.N;
				OrdenCompra ordenCompraTmp = ordenCompraRepository.GetById(dto.Id);

            if (ordenCompraTmp.EstadoOrdenCompraAnteriorId.HasValue)
            {
					estadoOrdenCompra = new() { EstadoOrdenCompraId = ordenCompraTmp.EstadoOrdenCompraAnteriorId.Value };
					esModificacion = SiNoConstante.S;
				}
				else
					estadoOrdenCompra = estadoOrdenCompraRepository.ObtenerPorCodigo(EstadoOrdenCompraConstante.ORDEN_COMPRA);

				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);
				
				using (TransactionScope tx = new())
				{
					ordenCompra.EsModificacion = esModificacion;
					ordenCompra.EstadoOrdenCompraAnteriorId = null;
					ordenCompra.EstadoOrdenCompraId = estadoOrdenCompra.EntityId;
					ordenCompraRepository.Update(ordenCompra);

					ordenCompraRepository.ActualizarEstadoAprobacion(new OrdenCompra()
					{
						OrdenCompraId = dto.OrdenCompraId,
						EstadoAprobacionId = estadoAprobacion.EntityId,
						FechaAprobacion = null
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void SolicitarModificacion(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = Mapper.Map<OrdenCompra>(dto);

				List<OrdenCompraProducto> ordenCompraProductoList = ordenCompraProductoRepository.Search(new OrdenCompraProducto()
				{
					OrdenCompraId = dto.OrdenCompraId,
					Estado = EstadoConstante.ACTIVO
				});

				if (ordenCompraProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_orden_compra_sin_items"));

				int totalSinCantidad = ordenCompraProductoList.Where(x => x.Cantidad == decimal.Zero).Count();
				if (totalSinCantidad > 0)
					throw new Exception(SR.GetString("EX_orden_compra_items_cantidad_cero"));

				int totalSinPrecio = ordenCompraProductoList.Where(x => x.PrecioUnitario == decimal.Zero).Count();
				if (totalSinPrecio > 0)
					throw new Exception(SR.GetString("EX_orden_compra_items_precio_cero"));

				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);
				EstadoOrdenCompra estadoOrdenCompra = estadoOrdenCompraRepository.ObtenerPorCodigo(EstadoOrdenCompraConstante.MODIFICACION);

				using (TransactionScope tx = new())
				{
					ordenCompraRepository.ActualizarEstado(new OrdenCompra()
					{
						OrdenCompraId = dto.OrdenCompraId,
						EstadoOrdenCompraAnteriorId = ordenCompra.EstadoOrdenCompraId,
						EstadoOrdenCompraId = estadoOrdenCompra.EstadoOrdenCompraId,
						UsuarioCreacionId = dto.UsuarioCreacionId,
					});

					ordenCompraRepository.ActualizarEstadoAprobacion(new OrdenCompra()
					{
						OrdenCompraId = dto.OrdenCompraId,
						EstadoAprobacionId = estadoAprobacion.EntityId,
						FechaAprobacion = null
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Aprobar(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = ordenCompraRepository.GetById(dto.Id);

				EstadoAprobacion estadoAprobacion = null;

				if(ordenCompra.EstadoOrdenCompra.Codigo != EstadoOrdenCompraConstante.MODIFICACION)
					estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);
				{
					using (TransactionScope tx = new())
					{

						ordenCompraRepository.ActualizarEstadoAprobacion(new OrdenCompra()
						{
							OrdenCompraId = dto.OrdenCompraId,
							EstadoAprobacionId = estadoAprobacion?.EntityId,
							FechaAprobacion = DateTime.Now
						});

						ordenCompraRepository.ActualizarProductos(new OrdenCompra()
						{
							OrdenCompraId = dto.OrdenCompraId

						});
						tx.Complete();
					}	                  
				}
            }
			catch (Exception)
			{
				throw;
			}
		}
		public void Desaprobar(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = ordenCompraRepository.GetById(dto.Id);

				EstadoAprobacion estadoAprobacion = null;

				if (ordenCompra.EstadoOrdenCompra.Codigo == EstadoOrdenCompraConstante.MODIFICACION)
					estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);
				else
					estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.DESAPROBADO);

				using (TransactionScope tx = new())
				{
					if (ordenCompra.EstadoOrdenCompra.Codigo == EstadoOrdenCompraConstante.MODIFICACION)
               {
						ordenCompraRepository.ActualizarEstado(new OrdenCompra()
						{
							OrdenCompraId = dto.OrdenCompraId,
							EstadoOrdenCompraAnteriorId = null,
							EstadoOrdenCompraId = ordenCompra.EstadoOrdenCompraAnteriorId.Value,
							UsuarioCreacionId = dto.UsuarioCreacionId,
						});
					}

					ordenCompraRepository.ActualizarEstadoAprobacion(new OrdenCompra()
					{
						OrdenCompraId = dto.OrdenCompraId,
						EstadoAprobacionId = estadoAprobacion.EntityId,
						FechaAprobacion = DateTime.Now
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void CambiarEstado(OrdenCompraDTO dto)
		{
			try
			{
				OrdenCompra ordenCompra = Mapper.Map<OrdenCompra>(dto);

				using (TransactionScope tx = new())
				{
					ordenCompraRepository.ActualizarEstado(ordenCompra);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
