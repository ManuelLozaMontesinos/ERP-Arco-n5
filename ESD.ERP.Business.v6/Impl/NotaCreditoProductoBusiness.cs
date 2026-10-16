using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class NotaCreditoProductoBusiness : INotaCreditoProductoBusiness
	{
		private readonly INotaCreditoProductoRepository notaCreditoProductoRepository;
		private readonly INotaCreditoRepository notaCreditoRepository;
		private readonly IComprobanteProductoRepository comprobanteProductoRepository;

		public NotaCreditoProductoBusiness(
			INotaCreditoProductoRepository notaCreditoProductoRepository,
			INotaCreditoRepository notaCreditoRepository,
			IComprobanteProductoRepository comprobanteProductoRepository
			)
		{
			this.notaCreditoProductoRepository = notaCreditoProductoRepository;
			this.notaCreditoRepository = notaCreditoRepository;
			this.comprobanteProductoRepository = comprobanteProductoRepository;
		}
		public NotaCreditoProductoDTO GetById(int id)
		{
			try
			{
				NotaCreditoProducto notaCreditoProducto = notaCreditoProductoRepository.GetById(id);

				NotaCreditoProductoDTO notaCreditoProductoDTO = Mapper.Map<NotaCreditoProductoDTO>(notaCreditoProducto);

				return notaCreditoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<NotaCreditoProductoDTO> Search(NotaCreditoProductoDTO dto)
		{
			try
			{
				List<NotaCreditoProducto> notaCreditoProductoList = notaCreditoProductoRepository.Search(Mapper.Map<NotaCreditoProducto>(dto));

				List<NotaCreditoProductoDTO> notaCreditoProductoDTOList = Mapper.Map<List<NotaCreditoProductoDTO>>(notaCreditoProductoList);

				return notaCreditoProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public NotaCreditoProductoDTO SingleSearch(NotaCreditoProductoDTO dto)
		{
			try
			{
				NotaCreditoProducto notaCreditoProducto = notaCreditoProductoRepository.SingleSearch(Mapper.Map<NotaCreditoProducto>(dto));

				NotaCreditoProductoDTO notaCreditoProductoDTO = Mapper.Map<NotaCreditoProductoDTO>(notaCreditoProducto);

				return notaCreditoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(NotaCreditoProductoDTO dto)
		{
			try
			{
				NotaCreditoProducto notaCreditoProducto = Mapper.Map<NotaCreditoProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = notaCreditoProductoRepository.Insert(notaCreditoProducto);

					dto.NotaCreditoProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(NotaCreditoProductoDTO dto)
		{
			try
			{

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					decimal cantidadAnterior = decimal.Zero;
					NotaCreditoProducto notaCreditoProducto = notaCreditoProductoRepository.GetById(dto.Id);

					cantidadAnterior = notaCreditoProducto.Cantidad;

					ComprobanteProducto comprobanteProducto = comprobanteProductoRepository.GetById(notaCreditoProducto.ComprobanteProductoId.Value);

					id = notaCreditoProductoRepository.Update(Mapper.Map<NotaCreditoProducto>(dto));

					comprobanteProducto.CantidadNotaCredito = (comprobanteProducto.CantidadNotaCredito - cantidadAnterior) + dto.Cantidad;
					comprobanteProductoRepository.ActualizarCantidadNotaCredito(comprobanteProducto);

					notaCreditoRepository.ActualizarMonto(new NotaCredito()
					{
						NotaCreditoId = dto.NotaCreditoId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(NotaCreditoProductoDTO dto)
		{
			try
			{
				using (TransactionScope tx = new TransactionScope())
				{
					NotaCreditoProducto notaCreditoProducto = notaCreditoProductoRepository.GetById(dto.Id);

					ComprobanteProducto comprobanteProducto = comprobanteProductoRepository.GetById(notaCreditoProducto.ComprobanteProductoId.Value);

					notaCreditoProductoRepository.Delete(Mapper.Map<NotaCreditoProducto>(dto));

					comprobanteProducto.CantidadNotaCredito = comprobanteProducto.CantidadNotaCredito - notaCreditoProducto.Cantidad;
					comprobanteProductoRepository.ActualizarCantidadNotaCredito(comprobanteProducto);

					notaCreditoRepository.ActualizarMonto(new NotaCredito()
					{
						NotaCreditoId = dto.NotaCreditoId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void AgregarComprobanteProducto(NotaCreditoProductoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = notaCreditoRepository.GetById(dto.NotaCreditoId);

				using (TransactionScope tx = new TransactionScope())
				{
					List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
					{
						ComprobanteId = dto.NotaCreditoDTO.ComprobanteId,
						ComprobanteProductoIdList = dto.ComprobanteProductoIdList
					});

					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
					{
						NotaCreditoProducto notaCreditoProducto = new NotaCreditoProducto()
						{
							NotaCreditoId = dto.NotaCreditoId,
							ComprobanteProductoId = comprobanteProducto.ComprobanteProductoId,
							Descripcion = comprobanteProducto.Descripcion,
							Cantidad = comprobanteProducto.Cantidad - comprobanteProducto.CantidadNotaCredito,
							PrecioUnitario = comprobanteProducto.PrecioUnitario,
							MontoBruto = comprobanteProducto.MontoBruto,
							MontoDescuento1 = comprobanteProducto.MontoDescuento1,
							MontoDescuento2 = comprobanteProducto.MontoDescuento2,
							MontoNeto = comprobanteProducto.MontoNeto,
							MontoIgv = comprobanteProducto.MontoIgv,
							MontoIsc = comprobanteProducto.MontoIsc,
							MontoTotal = comprobanteProducto.MontoTotal,
							TipoAfectacionIgvId = comprobanteProducto.TipoAfectacionIgvId,
							Estado = EstadoConstante.ACTIVO,
							UsuarioCreacionId = dto.UsuarioCreacionId
						};

						CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
						{
							PrecioUnitario = comprobanteProducto.PrecioUnitario,
							Cantidad = comprobanteProducto.Cantidad,
							TipoIGV = comprobanteProducto.TipoAfectacionIgv.TipoIgv.Codigo,
							ImpuestoTasa = notaCredito.ImpuestoTasa.Tasa,
						});

						notaCreditoProducto.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
						notaCreditoProducto.MontoBruto = calculadorVentaItemDTO.MontoBruto;
						notaCreditoProducto.MontoNeto = calculadorVentaItemDTO.MontoNeto;
						notaCreditoProducto.MontoIgv = calculadorVentaItemDTO.MontoImpuesto;
						notaCreditoProducto.MontoTotal = calculadorVentaItemDTO.MontoTotal;

						notaCreditoProductoRepository.Insert(notaCreditoProducto);

                        comprobanteProducto.CantidadNotaCredito += notaCreditoProducto.Cantidad;
                        comprobanteProductoRepository.ActualizarCantidadNotaCredito(comprobanteProducto);
												
					}

					notaCreditoRepository.ActualizarMonto(new NotaCredito()
					{
						NotaCreditoId = dto.NotaCreditoId,
						UsuarioCreacionId = dto.UsuarioCreacionId
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
