using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ComprobanteAutorizacionBusiness : IComprobanteAutorizacionBusiness
	{
		private readonly IComprobanteAutorizacionRepository comprobanteAutorizacionRepository;
		private readonly IComprobanteProductoRepository comprobanteProductoRepository;
		private readonly IComprobanteRepository comprobanteRepository;
		private readonly IEstadoAprobacionRepository estadoAprobacionRepository;
		public ComprobanteAutorizacionBusiness(
			IComprobanteAutorizacionRepository comprobanteAutorizacionRepository,
			IComprobanteProductoRepository comprobanteProductoRepository,
			IComprobanteRepository comprobanteRepository,
		IEstadoAprobacionRepository estadoAprobacionRepository
			)
		{
			this.comprobanteAutorizacionRepository = comprobanteAutorizacionRepository;
			this.comprobanteProductoRepository = comprobanteProductoRepository;
			this.comprobanteRepository = comprobanteRepository;
			this.estadoAprobacionRepository = estadoAprobacionRepository;
		}
		public ComprobanteAutorizacionDTO GetById(int id)
		{
			try
			{
				ComprobanteAutorizacion comprobanteAutorizacion = comprobanteAutorizacionRepository.GetById(id);

				ComprobanteAutorizacionDTO comprobanteAutorizacionDTO = Mapper.Map<ComprobanteAutorizacionDTO>(comprobanteAutorizacion);

				return comprobanteAutorizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteAutorizacionDTO> Search(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				List<ComprobanteAutorizacion> comprobanteAutorizacionList = comprobanteAutorizacionRepository.Search(Mapper.Map<ComprobanteAutorizacion>(dto));

				List<ComprobanteAutorizacionDTO> comprobanteAutorizacionDTOList = Mapper.Map<List<ComprobanteAutorizacionDTO>>(comprobanteAutorizacionList);

				return comprobanteAutorizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ComprobanteAutorizacionDTO SingleSearch(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				ComprobanteAutorizacion comprobanteAutorizacion = comprobanteAutorizacionRepository.SingleSearch(Mapper.Map<ComprobanteAutorizacion>(dto));

				ComprobanteAutorizacionDTO comprobanteAutorizacionDTO = Mapper.Map<ComprobanteAutorizacionDTO>(comprobanteAutorizacion);

				return comprobanteAutorizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				ComprobanteAutorizacion comprobanteAutorizacion = Mapper.Map<ComprobanteAutorizacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = comprobanteAutorizacionRepository.Insert(comprobanteAutorizacion);

					dto.ComprobanteAutorizacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				ComprobanteAutorizacion comprobanteAutorizacion = Mapper.Map<ComprobanteAutorizacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = comprobanteAutorizacionRepository.Update(comprobanteAutorizacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				ComprobanteAutorizacion comprobanteAutorizacion = Mapper.Map<ComprobanteAutorizacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteAutorizacionRepository.Delete(comprobanteAutorizacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteAutorizacionDTO> ListarPorTipoAutorizacionIdList(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				List<ComprobanteAutorizacion> comprobanteAutorizacionList = comprobanteAutorizacionRepository.
					ListarPorTipoAutorizacionIdList(Mapper.Map<ComprobanteAutorizacion>(dto));

				List<ComprobanteAutorizacionDTO> comprobanteAutorizacionDTOList = Mapper.Map<List<ComprobanteAutorizacionDTO>>(comprobanteAutorizacionList);

				return comprobanteAutorizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Solicitar(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				List<ComprobanteAutorizacion> comprobanteAutorizacionList = Mapper.Map<List<ComprobanteAutorizacion>>(dto.ComprobanteAutorizacionDTOList);

				using (TransactionScope tx = new TransactionScope())
				{

					EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);

					foreach (ComprobanteAutorizacion comprobanteAutorizacion in comprobanteAutorizacionList)
					{
						if (comprobanteAutorizacion.ComprobanteAutorizacionId != 0)
						{
							comprobanteAutorizacionRepository.Delete(comprobanteAutorizacion);

							comprobanteAutorizacion.UsuarioId = null;
							comprobanteAutorizacion.FechaAutorizacion = null;
							comprobanteAutorizacion.Estado = EstadoConstante.ACTIVO;
						}

						comprobanteAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
						comprobanteAutorizacionRepository.Insert(comprobanteAutorizacion);
					}

					if (dto.ComprobanteProductoDTOList != null)
					{
						foreach (ComprobanteProductoDTO comprobanteProductoDTO in dto.ComprobanteProductoDTOList)
						{
							ComprobanteProducto comprobanteProducto = comprobanteProductoRepository.GetById(comprobanteProductoDTO.Id);
							comprobanteProducto.SolicitaAutorizacionDescuento = comprobanteProductoDTO.SolicitaAutorizacionDescuento;
							comprobanteProductoRepository.ActualizarSolicitaAutorizacionDescuento(comprobanteProducto);
						}
					}

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Aprobar(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				ComprobanteAutorizacion comprobanteAutorizacion = comprobanteAutorizacionRepository.GetById(dto.ComprobanteAutorizacionId);
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);

				comprobanteAutorizacion.UsuarioId = dto.UsuarioCreacionId;
				comprobanteAutorizacion.FechaAutorizacion = DateTime.Now;
				comprobanteAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
				comprobanteAutorizacion.UsuarioCreacionId = dto.UsuarioCreacionId;

				List<ComprobanteProducto> comprobanteProductoList = null;
				if (comprobanteAutorizacion.TipoAutorizacion.Codigo == TipoAutorizacionConstante.DSCTO_EN_PRECIO_UNITARIO)
				{
					comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
					{
						ComprobanteId = comprobanteAutorizacion.ComprobanteId,
						SolicitaAutorizacionDescuento = SiNoConstante.S,
						NullRegistroComboId = SiNoConstante.S,
						Estado = EstadoConstante.ACTIVO
					});

					foreach (ComprobanteProductoDTO comprobanteProductoDTO in dto.ComprobanteProductoDTOList)
					{
						ComprobanteProducto comprobanteProducto = comprobanteProductoList.Where(x => x.ComprobanteProductoId == comprobanteProductoDTO.Id)
							.FirstOrDefault();
						if (comprobanteProducto == null)
							continue;

						CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
						{
							PrecioUnitario = comprobanteProductoDTO.PrecioAutorizado,
							Cantidad = comprobanteProducto.Cantidad,
							TipoIGV = comprobanteProducto.TipoAfectacionIgv.TipoIgv.Codigo,
							ImpuestoTasa = comprobanteProducto.Comprobante.ImpuestoTasa.Tasa
						});

						comprobanteProducto.CantidadBloqueada = SiNoConstante.S;
						comprobanteProducto.SolicitaAutorizacionDescuento = SiNoConstante.N;
						comprobanteProducto.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
						comprobanteProducto.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
						comprobanteProducto.MontoBruto = calculadorVentaItemDTO.MontoBruto;
						comprobanteProducto.MontoNeto = calculadorVentaItemDTO.MontoNeto;
						comprobanteProducto.MontoIgv = calculadorVentaItemDTO.MontoImpuesto;
						comprobanteProducto.MontoTotal = calculadorVentaItemDTO.MontoTotal;
						comprobanteProducto.MontoDetraccion = calculadorVentaItemDTO.MontoDetraccion;
					}

				}

				using (TransactionScope tx = new())
				{

					comprobanteAutorizacionRepository.Update(comprobanteAutorizacion);

					if (comprobanteProductoList != null)
					{
						comprobanteProductoRepository.ActualizarPrecioMontoLote(comprobanteProductoList);

						comprobanteRepository.ActualizarMonto(new Comprobante() { ComprobanteId = comprobanteAutorizacion.ComprobanteId });
					}

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Desaprobar(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				ComprobanteAutorizacion comprobanteAutorizacion = comprobanteAutorizacionRepository.GetById(dto.ComprobanteAutorizacionId);
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.DESAPROBADO);

				comprobanteAutorizacion.UsuarioId = dto.UsuarioCreacionId;
				comprobanteAutorizacion.FechaAutorizacion = DateTime.Now;
				comprobanteAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
				comprobanteAutorizacion.UsuarioCreacionId = dto.UsuarioCreacionId;

				using (TransactionScope tx = new TransactionScope())
				{

					comprobanteAutorizacionRepository.Update(comprobanteAutorizacion);

					if (comprobanteAutorizacion.TipoAutorizacion.Codigo == TipoAutorizacionConstante.DSCTO_EN_PRECIO_UNITARIO)
					{
						List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
						{
							ComprobanteId = comprobanteAutorizacion.ComprobanteId,
							SolicitaAutorizacionDescuento = SiNoConstante.S,
							NullRegistroComboId = SiNoConstante.S,
							Estado = EstadoConstante.ACTIVO
						});

						foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
						{
							comprobanteProducto.SolicitaAutorizacionDescuento = SiNoConstante.N;

							comprobanteProductoRepository.ActualizarSolicitaAutorizacionDescuento(comprobanteProducto);
						}
					}

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Cancelar(ComprobanteAutorizacionDTO dto)
		{
			try
			{
				List<ComprobanteAutorizacion> comprobanteAutorizacionList = Mapper.Map<List<ComprobanteAutorizacion>>(dto.ComprobanteAutorizacionDTOList);

				using (TransactionScope tx = new TransactionScope())
				{

					EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.CANCELADO);

					foreach (ComprobanteAutorizacion comprobanteAutorizacion in comprobanteAutorizacionList)
					{

						comprobanteAutorizacion.FechaAutorizacion = DateTime.Now;
						comprobanteAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
						comprobanteAutorizacion.Estado = EstadoConstante.INACTIVO;
						comprobanteAutorizacionRepository.Update(comprobanteAutorizacion);

						if (comprobanteAutorizacion.TipoAutorizacion.Codigo == TipoAutorizacionConstante.DSCTO_EN_PRECIO_UNITARIO)
						{
							List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
							{
								ComprobanteId = comprobanteAutorizacion.ComprobanteId,
								SolicitaAutorizacionDescuento = SiNoConstante.S,
								NullRegistroComboId = SiNoConstante.S,
								Estado = EstadoConstante.ACTIVO
							});

							foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
							{
								comprobanteProducto.SolicitaAutorizacionDescuento = SiNoConstante.N;

								comprobanteProductoRepository.ActualizarSolicitaAutorizacionDescuento(comprobanteProducto);
							}
						}
					}

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
