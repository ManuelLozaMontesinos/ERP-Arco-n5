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
	public class PedidoCompraBusiness : IPedidoCompraBusiness
	{
		private readonly IPedidoCompraRepository pedidoCompraRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IPedidoCompraProductoRepository pedidoCompraProductoRepository;
		private readonly IEstadoAprobacionRepository estadoAprobacionRepository;
		private readonly IEstadoPedidoCompraRepository estadoPedidoCompraRepository;

		public PedidoCompraBusiness(
			IPedidoCompraRepository pedidoCompraRepository,
			ITablaBusiness tablaBusiness,
			IPedidoCompraProductoRepository pedidoCompraProductoRepository,
			IEstadoAprobacionRepository estadoAprobacionRepository,
			IEstadoPedidoCompraRepository estadoPedidoCompraRepository
			)
		{
			this.pedidoCompraRepository = pedidoCompraRepository;
			this.tablaBusiness = tablaBusiness;
			this.pedidoCompraProductoRepository = pedidoCompraProductoRepository;
			this.estadoAprobacionRepository = estadoAprobacionRepository;
			this.estadoPedidoCompraRepository = estadoPedidoCompraRepository;
		}
		public PedidoCompraDTO GetById(int id)
		{
			try
			{
				PedidoCompra pedidoCompra = pedidoCompraRepository.GetById(id);

				PedidoCompraDTO pedidoCompraDTO = Mapper.Map<PedidoCompraDTO>(pedidoCompra);

				return pedidoCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PedidoCompraDTO> Search(PedidoCompraDTO dto)
		{
			try
			{
				List<PedidoCompra> pedidoCompraList = pedidoCompraRepository.Search(Mapper.Map<PedidoCompra>(dto));

				List<PedidoCompraDTO> pedidoCompraDTOList = Mapper.Map<List<PedidoCompraDTO>>(pedidoCompraList);

				return pedidoCompraDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PedidoCompraDTO SingleSearch(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = pedidoCompraRepository.SingleSearch(Mapper.Map<PedidoCompra>(dto));

				PedidoCompraDTO pedidoCompraDTO = Mapper.Map<PedidoCompraDTO>(pedidoCompra);

				return pedidoCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = Mapper.Map<PedidoCompra>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					pedidoCompra.Codigo = obtenerCorrelativo(dto);
					newId = pedidoCompraRepository.Insert(pedidoCompra);

					dto.PedidoCompraId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = Mapper.Map<PedidoCompra>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = pedidoCompraRepository.Update(pedidoCompra);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = Mapper.Map<PedidoCompra>(dto);

				using (TransactionScope tx = new())
				{
					pedidoCompraRepository.Delete(pedidoCompra);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public int AgregarDesdeStockMinimo(PedidoCompraDTO dto)
      {
			try
			{
				PedidoCompra pedidoCompra = Mapper.Map<PedidoCompra>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					pedidoCompra.Codigo = obtenerCorrelativo(dto);
					newId = pedidoCompraRepository.Insert(pedidoCompra);

					dto.PedidoCompraId = newId;

					List<PedidoCompraProducto> pedidoCompraProductoList = Mapper.Map<List<PedidoCompraProducto>>(dto.PedidoCompraProductoDTOList);

					foreach (PedidoCompraProducto pedidoCompraProducto in pedidoCompraProductoList)
               {
						pedidoCompraProducto.PedidoCompraId = newId;
						pedidoCompraProducto.EsAdicional = SiNoConstante.N;
						pedidoCompraProducto.Estado = EstadoConstante.ACTIVO;
						pedidoCompraProducto.UsuarioCreacionId = dto.UsuarioCreacionId;
					}

					pedidoCompraProductoRepository.ActualizarLote(pedidoCompraProductoList);

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		private string obtenerCorrelativo(PedidoCompraDTO dto)
      {
			TablaCorrelativoDTO tablaCorrelativoDTO = new()
			{
				Tabla = TablaConstante.PEDIDO_COMPRA,
				PeriodoEmpresaSedeDTO = dto.PeriodoEmpresaSedeDTO,
				UsuarioAccionId = dto.UsuarioCreacionId,
				UsarSedeCentral = true
			};

			return tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);
		}
		public void SolicitarAutorizacion(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = Mapper.Map<PedidoCompra>(dto);

				List<PedidoCompraProducto> pedidoCompraProductoList = pedidoCompraProductoRepository.Search(new PedidoCompraProducto()
				{
					PedidoCompraId = dto.PedidoCompraId,
					Estado = EstadoConstante.ACTIVO
				});

				if (pedidoCompraProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_pedido_compra_sin_items"));

				int totalSinCantidad = pedidoCompraProductoList.Where(x => x.Cantidad == decimal.Zero).Count();
				if(totalSinCantidad != 0)
            {
					if (pedidoCompraProductoList.Count == totalSinCantidad)
						throw new Exception(SR.GetString("EX_pedido_compra_items_cantidad_cero"));
				}

				EstadoPedidoCompra estadoPedidoCompra = null;
				string esModificacion = SiNoConstante.N;
				PedidoCompra pedidoCompraTmp = pedidoCompraRepository.GetById(dto.Id);

				if (pedidoCompraTmp.EstadoPedidoCompraAnteriorId.HasValue)
				{
					estadoPedidoCompra = new() { EstadoPedidoCompraId = pedidoCompraTmp.EstadoPedidoCompraAnteriorId.Value };
					esModificacion = SiNoConstante.S;
				}
				else
					estadoPedidoCompra = estadoPedidoCompraRepository.ObtenerPorCodigo(EstadoPedidoCompraConstante.PEDIDO);

				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);

				using (TransactionScope tx = new())
				{
					pedidoCompra.EsModificacion = esModificacion;
					pedidoCompra.EstadoPedidoCompraAnteriorId = null;
					pedidoCompra.EstadoPedidoCompraId = estadoPedidoCompra.EntityId;
					pedidoCompraRepository.Update(pedidoCompra);

					pedidoCompraRepository.ActualizarEstadoAprobacion(new PedidoCompra()
					{
						PedidoCompraId = dto.PedidoCompraId,
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
		public void Aprobar(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = pedidoCompraRepository.GetById(dto.Id);

				EstadoAprobacion estadoAprobacion = null;

				if (pedidoCompra.EstadoPedidoCompra.Codigo != EstadoPedidoCompraConstante.MODIFICACION)
					estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);

				using (TransactionScope tx = new())
				{

					pedidoCompraRepository.ActualizarEstadoAprobacion(new PedidoCompra()
					{
						PedidoCompraId = dto.PedidoCompraId,
						EstadoAprobacionId = estadoAprobacion?.EntityId,
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
		public void Desaprobar(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = pedidoCompraRepository.GetById(dto.Id);

				EstadoAprobacion estadoAprobacion = null;

				if (pedidoCompra.EstadoPedidoCompra.Codigo == EstadoPedidoCompraConstante.MODIFICACION)
					estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);
				else
					estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.DESAPROBADO);

				using (TransactionScope tx = new())
				{
					if (pedidoCompra.EstadoPedidoCompra.Codigo == EstadoPedidoCompraConstante.MODIFICACION)
					{
						pedidoCompraRepository.ActualizarEstadoPedido(new PedidoCompra()
						{
							PedidoCompraId = dto.PedidoCompraId,
							EstadoPedidoCompraAnteriorId = null,
							EstadoPedidoCompraId = pedidoCompra.EstadoPedidoCompraAnteriorId.Value,
							UsuarioCreacionId = dto.UsuarioCreacionId,
						});
					}

					pedidoCompraRepository.ActualizarEstadoAprobacion(new PedidoCompra()
					{
						PedidoCompraId = dto.PedidoCompraId,
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
		public void SolicitarModificacion(PedidoCompraDTO dto)
		{
			try
			{
				PedidoCompra pedidoCompra = Mapper.Map<PedidoCompra>(dto);

				List<PedidoCompraProducto> pedidoCompraProductoList = pedidoCompraProductoRepository.Search(new PedidoCompraProducto()
				{
					PedidoCompraId = dto.PedidoCompraId,
					Estado = EstadoConstante.ACTIVO
				});

				if (pedidoCompraProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_pedido_compra_sin_items"));

				int totalSinCantidad = pedidoCompraProductoList.Where(x => x.Cantidad == decimal.Zero).Count();
				if (totalSinCantidad != 0)
				{
					if (pedidoCompraProductoList.Count == totalSinCantidad)
						throw new Exception(SR.GetString("EX_pedido_compra_items_cantidad_cero"));
				}

				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);
				EstadoPedidoCompra estadoPedidoCompra = estadoPedidoCompraRepository.ObtenerPorCodigo(EstadoPedidoCompraConstante.MODIFICACION);

				using (TransactionScope tx = new())
				{
					pedidoCompraRepository.ActualizarEstadoPedido(new PedidoCompra()
					{
						PedidoCompraId = dto.PedidoCompraId,
						EstadoPedidoCompraAnteriorId = pedidoCompra.EstadoPedidoCompraId,
						EstadoPedidoCompraId = estadoPedidoCompra.EstadoPedidoCompraId,
						UsuarioCreacionId = dto.UsuarioCreacionId,
					});

					pedidoCompraRepository.ActualizarEstadoAprobacion(new PedidoCompra()
					{
						PedidoCompraId = dto.PedidoCompraId,
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
	}
}
