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
	public class CotizacionAutorizacionBusiness : ICotizacionAutorizacionBusiness
	{
		private readonly ICotizacionAutorizacionRepository cotizacionAutorizacionRepository;
		private readonly ICotizacionProductoRepository cotizacionProductoRepository;
		private readonly ICotizacionRepository cotizacionRepository;
		private readonly IEstadoAprobacionRepository estadoAprobacionRepository;
		public CotizacionAutorizacionBusiness(
			ICotizacionAutorizacionRepository cotizacionAutorizacionRepository,
			ICotizacionProductoRepository cotizacionProductoRepository,
			ICotizacionRepository cotizacionRepository,
		IEstadoAprobacionRepository estadoAprobacionRepository
			)
		{
			this.cotizacionAutorizacionRepository = cotizacionAutorizacionRepository;
			this.cotizacionProductoRepository = cotizacionProductoRepository;
			this.cotizacionRepository = cotizacionRepository;
			this.estadoAprobacionRepository = estadoAprobacionRepository;
		}
		public CotizacionAutorizacionDTO GetById(int id)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = cotizacionAutorizacionRepository.GetById(id);

				CotizacionAutorizacionDTO cotizacionAutorizacionDTO = Mapper.Map<CotizacionAutorizacionDTO>(cotizacionAutorizacion);

				return cotizacionAutorizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CotizacionAutorizacionDTO> Search(CotizacionAutorizacionDTO dto)
		{
			try
			{
				List<CotizacionAutorizacion> cotizacionAutorizacionList = cotizacionAutorizacionRepository.Search(Mapper.Map<CotizacionAutorizacion>(dto));

				List<CotizacionAutorizacionDTO> cotizacionAutorizacionDTOList = Mapper.Map<List<CotizacionAutorizacionDTO>>(cotizacionAutorizacionList);

				return cotizacionAutorizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CotizacionAutorizacionDTO SingleSearch(CotizacionAutorizacionDTO dto)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = cotizacionAutorizacionRepository.SingleSearch(Mapper.Map<CotizacionAutorizacion>(dto));

				CotizacionAutorizacionDTO cotizacionAutorizacionDTO = Mapper.Map<CotizacionAutorizacionDTO>(cotizacionAutorizacion);

				return cotizacionAutorizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CotizacionAutorizacionDTO dto)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = Mapper.Map<CotizacionAutorizacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = cotizacionAutorizacionRepository.Insert(cotizacionAutorizacion);

					dto.CotizacionAutorizacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CotizacionAutorizacionDTO dto)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = Mapper.Map<CotizacionAutorizacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = cotizacionAutorizacionRepository.Update(cotizacionAutorizacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CotizacionAutorizacionDTO dto)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = Mapper.Map<CotizacionAutorizacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					cotizacionAutorizacionRepository.Delete(cotizacionAutorizacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void UpdateAuto(CotizacionAutorizacionDTO dto)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = Mapper.Map<CotizacionAutorizacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					cotizacionAutorizacionRepository.UpdateAuto(new CotizacionAutorizacion()
					{
						CotizacionId = dto.CotizacionId
					});

					tx.Complete();
                }

			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<CotizacionAutorizacionDTO> ListarPorTipoAutorizacionIdList(CotizacionAutorizacionDTO dto)
      {
			try
			{
				List<CotizacionAutorizacion> cotizacionAutorizacionList = cotizacionAutorizacionRepository.
					ListarPorTipoAutorizacionIdList(Mapper.Map<CotizacionAutorizacion>(dto));

				List<CotizacionAutorizacionDTO> cotizacionAutorizacionDTOList = Mapper.Map<List<CotizacionAutorizacionDTO>>(cotizacionAutorizacionList);

				return cotizacionAutorizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void Solicitar(CotizacionAutorizacionDTO dto)
      {
			try
			{
				List<CotizacionAutorizacion> cotizacionAutorizacionList = Mapper.Map<List<CotizacionAutorizacion>>(dto.CotizacionAutorizacionDTOList);

				using (TransactionScope tx = new TransactionScope())
				{

					EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);

					foreach (CotizacionAutorizacion cotizacionAutorizacion in cotizacionAutorizacionList)
               {
						if(cotizacionAutorizacion.CotizacionAutorizacionId != 0)
                  {
							cotizacionAutorizacionRepository.Delete(cotizacionAutorizacion);

							cotizacionAutorizacion.UsuarioId = null;
							cotizacionAutorizacion.FechaAutorizacion = null;
							cotizacionAutorizacion.Estado = EstadoConstante.ACTIVO;
						}

						cotizacionAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
						cotizacionAutorizacionRepository.Insert(cotizacionAutorizacion);
					}

					if(dto.CotizacionProductoDTOList != null)
               {
						foreach(CotizacionProductoDTO cotizacionProductoDTO in dto.CotizacionProductoDTOList)
                  {
							CotizacionProducto cotizacionProducto = cotizacionProductoRepository.GetById(cotizacionProductoDTO.Id);
							cotizacionProducto.SolicitaAutorizacionDescuento = cotizacionProductoDTO.SolicitaAutorizacionDescuento;
							cotizacionProductoRepository.ActualizarSolicitaAutorizacionDescuento(cotizacionProducto);
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
		public void Aprobar(CotizacionAutorizacionDTO dto)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = cotizacionAutorizacionRepository.GetById(dto.CotizacionAutorizacionId);
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);

				cotizacionAutorizacion.UsuarioId = dto.UsuarioCreacionId;
				cotizacionAutorizacion.FechaAutorizacion = DateTime.Now;
				cotizacionAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
				cotizacionAutorizacion.UsuarioCreacionId = dto.UsuarioCreacionId;

				List<CotizacionProducto> cotizacionProductoList = null;
				if (cotizacionAutorizacion.TipoAutorizacion.Codigo == TipoAutorizacionConstante.DSCTO_EN_PRECIO_UNITARIO)
				{
					cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
					{
						CotizacionId = cotizacionAutorizacion.CotizacionId,
						SolicitaAutorizacionDescuento = SiNoConstante.S,
						Estado = EstadoConstante.ACTIVO
					});

					foreach(CotizacionProductoDTO cotizacionProductoDTO in dto.CotizacionProductoDTOList)
               {
						CotizacionProducto cotizacionProducto = cotizacionProductoList.Where(x => x.CotizacionProductoId == cotizacionProductoDTO.Id)
							.FirstOrDefault();
						if (cotizacionProducto == null)
							continue;

						CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO() { 
							PrecioUnitario = cotizacionProductoDTO.PrecioAutorizado,
							Cantidad = cotizacionProducto.Cantidad,
							TipoIGV = cotizacionProducto.TipoAfectacionIgv.TipoIgv.Codigo,
							ImpuestoTasa = cotizacionProducto.Cotizacion.ImpuestoTasa.Tasa
						});

						cotizacionProducto.CantidadBloqueada = SiNoConstante.S;
						cotizacionProducto.SolicitaAutorizacionDescuento = SiNoConstante.N;
						cotizacionProducto.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
						cotizacionProducto.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
						cotizacionProducto.MontoBruto = calculadorVentaItemDTO.MontoBruto;
						cotizacionProducto.MontoNeto = calculadorVentaItemDTO.MontoNeto;
						cotizacionProducto.MontoIgv = calculadorVentaItemDTO.MontoImpuesto;
						cotizacionProducto.MontoTotal = calculadorVentaItemDTO.MontoTotal;
					}

				}

            using (TransactionScope tx = new TransactionScope())
            {

					cotizacionAutorizacionRepository.Update(cotizacionAutorizacion);

					if (cotizacionProductoList != null)
               {
						cotizacionProductoRepository.ActualizarPrecioMontoLote(cotizacionProductoList);

						cotizacionRepository.ActualizarMonto(new Cotizacion() { CotizacionId = cotizacionAutorizacion.CotizacionId });
					}

               tx.Complete();
            }
         }
			catch (Exception)
			{
				throw;
			}
		}
		public void Desaprobar(CotizacionAutorizacionDTO dto)
		{
			try
			{
				CotizacionAutorizacion cotizacionAutorizacion = cotizacionAutorizacionRepository.GetById(dto.CotizacionAutorizacionId);
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.DESAPROBADO);

				cotizacionAutorizacion.UsuarioId = dto.UsuarioCreacionId;
				cotizacionAutorizacion.FechaAutorizacion = DateTime.Now;
				cotizacionAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
				cotizacionAutorizacion.UsuarioCreacionId = dto.UsuarioCreacionId;

				using (TransactionScope tx = new TransactionScope())
				{

					cotizacionAutorizacionRepository.Update(cotizacionAutorizacion);

					if (cotizacionAutorizacion.TipoAutorizacion.Codigo == TipoAutorizacionConstante.DSCTO_EN_PRECIO_UNITARIO)
					{
						List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
						{
							CotizacionId = cotizacionAutorizacion.CotizacionId,
							SolicitaAutorizacionDescuento = SiNoConstante.S,
							Estado = EstadoConstante.ACTIVO
						});

						foreach (CotizacionProducto cotizacionProducto in cotizacionProductoList)
						{
							cotizacionProducto.SolicitaAutorizacionDescuento = SiNoConstante.N;

							cotizacionProductoRepository.ActualizarSolicitaAutorizacionDescuento(cotizacionProducto);
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
		public void Cancelar(CotizacionAutorizacionDTO dto)
		{
			try
			{
				List<CotizacionAutorizacion> cotizacionAutorizacionList = Mapper.Map<List<CotizacionAutorizacion>>(dto.CotizacionAutorizacionDTOList);

				using (TransactionScope tx = new TransactionScope())
				{

					EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.CANCELADO);

					foreach (CotizacionAutorizacion cotizacionAutorizacion in cotizacionAutorizacionList)
					{

						cotizacionAutorizacion.FechaAutorizacion = DateTime.Now;
						cotizacionAutorizacion.EstadoAprobacionId = estadoAprobacion.EntityId;
						cotizacionAutorizacion.Estado = EstadoConstante.INACTIVO;
						cotizacionAutorizacionRepository.Update(cotizacionAutorizacion);

						if(cotizacionAutorizacion.TipoAutorizacion.Codigo == TipoAutorizacionConstante.DSCTO_EN_PRECIO_UNITARIO)
                  {
							List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
							{
								CotizacionId = cotizacionAutorizacion.CotizacionId,
								SolicitaAutorizacionDescuento = SiNoConstante.S,
								Estado = EstadoConstante.ACTIVO
							});

							foreach (CotizacionProducto cotizacionProducto in cotizacionProductoList)
							{
								cotizacionProducto.SolicitaAutorizacionDescuento = SiNoConstante.N;

								cotizacionProductoRepository.ActualizarSolicitaAutorizacionDescuento(cotizacionProducto);
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
