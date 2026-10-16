using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;
using Unity.Builder;

namespace ESD.ERP.Business.Impl
{
	public class CotizacionProductoBusiness : ICotizacionProductoBusiness
	{
		private readonly ICotizacionProductoRepository cotizacionProductoRepository;
		private readonly ICotizacionRepository cotizacionRepository;
		private readonly IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness;
		//private readonly IMesContableRepository mesContableRepository;
		private readonly IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository;
		private readonly ICotizacionAutorizacionRepository cotizacionAutorizacionRepository;
		private readonly IComboProductoRepository comboProductoRepository;
		private readonly IListaPrecioProductoRepository listaPrecioProductoRepository;
		private readonly ICortesGratisRepository cortesGratisRepository;
		private readonly ICortesGratisBusiness cortesGratisBusiness;
		private readonly IFamiliaProductoRepository familiaProductoRepository;
		private readonly IProductoRepository productoRepository;

		public CotizacionProductoBusiness(
			ICotizacionProductoRepository cotizacionProductoRepository,
			ICotizacionRepository cotizacionRepository,
			IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness,
			//IMesContableRepository mesContableRepository,
			IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository,
			ICotizacionAutorizacionRepository cotizacionAutorizacionRepository,
			IComboProductoRepository comboProductoRepository,
			IListaPrecioProductoRepository listaPrecioProductoRepository,
			ICortesGratisRepository cortesGratisRepository,
			IFamiliaProductoRepository familiaProductoRepository,
			IProductoRepository productoRepository,
			ICortesGratisBusiness cortesGratisBusiness
			)
		{
			this.cotizacionProductoRepository = cotizacionProductoRepository;
			this.cotizacionRepository = cotizacionRepository;
			this.movimientoAlmacenSaldoBusiness = movimientoAlmacenSaldoBusiness;
			//this.mesContableRepository = mesContableRepository;
			this.movimientoAlmacenSaldoRepository = movimientoAlmacenSaldoRepository;
			this.cotizacionAutorizacionRepository = cotizacionAutorizacionRepository;
			this.comboProductoRepository = comboProductoRepository;
			this.listaPrecioProductoRepository = listaPrecioProductoRepository;
			this.cortesGratisRepository = cortesGratisRepository;
			this.familiaProductoRepository = familiaProductoRepository;
			this.productoRepository = productoRepository;
			this.cortesGratisBusiness = cortesGratisBusiness;
		}
		public CotizacionProductoDTO GetById(int id)
		{
			try
			{
				CotizacionProducto cotizacionProducto = cotizacionProductoRepository.GetById(id);

				CotizacionProductoDTO cotizacionProductoDTO = Mapper.Map<CotizacionProductoDTO>( cotizacionProducto);

				return cotizacionProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CotizacionProductoDTO> Search(CotizacionProductoDTO dto)
		{
			try
			{
				List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(Mapper.Map<CotizacionProducto>(dto));

				List<CotizacionProductoDTO> cotizacionProductoDTOList = Mapper.Map<List<CotizacionProductoDTO>>( cotizacionProductoList);

				return cotizacionProductoDTOList;
			}
			catch (Exception)
			{	
				throw;
			}
		}
        public CotizacionProductoDTO SingleSearch(CotizacionProductoDTO dto)
		{
			try
			{
				CotizacionProducto cotizacionProducto = cotizacionProductoRepository.SingleSearch(Mapper.Map<CotizacionProducto>(dto));

				CotizacionProductoDTO cotizacionProductoDTO = Mapper.Map<CotizacionProductoDTO>( cotizacionProducto);

				return cotizacionProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CotizacionProductoDTO dto)
		{
			try
			{
				int usuarioAccionId = (dto.UsuarioModificacionId.HasValue && dto.UsuarioModificacionId.Value > 0)
					? dto.UsuarioModificacionId.Value
					: dto.UsuarioCreacionId;

				CotizacionProducto cotizacionProducto = Mapper.Map<CotizacionProducto>(dto);
				cotizacionProducto.UsuarioCreacionId = usuarioAccionId;
				cotizacionProducto.UsuarioModificacionId = usuarioAccionId;

				validarMontoMinimoAnticipo(cotizacionProducto,true);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = cotizacionProductoRepository.Insert(cotizacionProducto);
					cotizacionProducto.CotizacionProductoId = newId;

					cotizacionRepository.ActualizarMonto(new Cotizacion() { 
						CotizacionId = dto.CotizacionId,
						UsuarioCreacionId = usuarioAccionId,
						UsuarioModificacionId = usuarioAccionId
					});

               if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto
                  .EsCombo == SiNoConstante.N)
                  actualizarReservaInterna(cotizacionProducto, true);
               else
						actualizarCombo(cotizacionProducto, true);

               dto.CotizacionProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CotizacionProductoDTO dto)
		{
			try
			{
				int usuarioAccionId = (dto.UsuarioModificacionId.HasValue && dto.UsuarioModificacionId.Value > 0)
					? dto.UsuarioModificacionId.Value
					: dto.UsuarioCreacionId;

				CotizacionProducto cotizacionProducto = Mapper.Map<CotizacionProducto>(dto);
				cotizacionProducto.UsuarioCreacionId = usuarioAccionId;
				cotizacionProducto.UsuarioModificacionId = usuarioAccionId;

				validarMontoMinimoAnticipo(cotizacionProducto, false);

				int id = 0;
				using (TransactionScope tx = new())
				{

					if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto
						.EsCombo == SiNoConstante.N)
						actualizarReservaInterna(cotizacionProducto, false);
					else
						actualizarCombo(cotizacionProducto, false);

					id = cotizacionProductoRepository.Update(cotizacionProducto);

					cotizacionRepository.ActualizarMonto(new Cotizacion()
					{
						CotizacionId = dto.CotizacionId,
						UsuarioCreacionId = usuarioAccionId,
						UsuarioModificacionId = usuarioAccionId
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
		public void Delete(CotizacionProductoDTO dto)
		{
			try
			{
				int usuarioAccionId = (dto.UsuarioModificacionId.HasValue && dto.UsuarioModificacionId.Value > 0)
					? dto.UsuarioModificacionId.Value
					: dto.UsuarioCreacionId;

				CotizacionProducto cotizacionProducto = Mapper.Map<CotizacionProducto>(dto);
				cotizacionProducto.UsuarioCreacionId = usuarioAccionId;
				cotizacionProducto.UsuarioModificacionId = usuarioAccionId;

				using (TransactionScope tx = new())
				{
					cotizacionProductoRepository.Delete(cotizacionProducto);

					cotizacionRepository.ActualizarMonto(new Cotizacion()
					{
						CotizacionId = dto.CotizacionId,
						UsuarioCreacionId = usuarioAccionId,
						UsuarioModificacionId = usuarioAccionId
					});

					if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto
						.EsCombo == SiNoConstante.N)
						eliminarReserva(cotizacionProducto);
					else
						eliminarCombo(cotizacionProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void actualizarReservaInterna(CotizacionProducto cotizacionProducto, bool esNuevo)
		{

			decimal cantidad = cotizacionProducto.Cantidad;
			decimal cantidadAnterior = decimal.Zero;

			cotizacionProducto = cotizacionProductoRepository.GetById(cotizacionProducto.EntityId);

			if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.
				GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.SERVICIO)
				return;

			if (cotizacionProducto.Cotizacion.EsPedido == SiNoConstante.N)
				return;

			if (!esNuevo)
				cantidadAnterior = cotizacionProducto.Cantidad;

			//MesContable mesContable = mesContableRepository.ObtenerPorCodigo(string.Format(StringFormatConstant.DATE_MONTH, cotizacionProducto.Cotizacion.Fecha));

			movimientoAlmacenSaldoBusiness.ValidarSaldoVenta(new MovimientoAlmacenSaldoDTO()
			{
				PeriodoEmpresaSedeId = cotizacionProducto.Cotizacion.PeriodoEmpresaSedeId,
				ProductoId = cotizacionProducto.ProductoId,
				MesContableId = cotizacionProducto.Cotizacion.MesContableId.Value,
				CantidadAnterior = cantidadAnterior,
				CantidadIngreso = cantidad
			});

			MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(new MovimientoAlmacenSaldo()
			{
				PeriodoEmpresaSedeId = cotizacionProducto.Cotizacion.PeriodoEmpresaSedeId,
				AlmacenId = cotizacionProducto.Cotizacion.AlmacenId.Value,
				ProductoId = cotizacionProducto.ProductoId,
				MesContableId = cotizacionProducto.Cotizacion.MesContableId.Value
			});

			if (movimientoAlmacenSaldo == null)
			{
				movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
				{
					PeriodoEmpresaSedeId = cotizacionProducto.Cotizacion.PeriodoEmpresaSedeId,
					AlmacenId = cotizacionProducto.Cotizacion.AlmacenId.Value,
					ProductoId = cotizacionProducto.ProductoId,
					MesContableId = cotizacionProducto.Cotizacion.MesContableId.Value,
					CantidadReserva = cantidad
				};

				movimientoAlmacenSaldoRepository.Insert(movimientoAlmacenSaldo);
			}
			else
			{
				movimientoAlmacenSaldo.CantidadReserva -= cantidadAnterior;
				movimientoAlmacenSaldo.CantidadReserva += cantidad;

				movimientoAlmacenSaldoRepository.ActualizarCantidadReserva(movimientoAlmacenSaldo);
			}

		}
		private void actualizarCombo(CotizacionProducto cotizacionProducto, bool esNuevo)
		{

			Cotizacion cotizacion = cotizacionRepository.GetById(cotizacionProducto.CotizacionId);

			List<CotizacionProducto> cotizacionProductoList = new List<CotizacionProducto>();

			List<ComboProducto> comboProductoList = comboProductoRepository.Search(new ComboProducto()
			{
				ProductoId = cotizacionProducto.ProductoId,
				Estado = EstadoConstante.ACTIVO
			});

			if (esNuevo)
			{

				foreach (ComboProducto comboProducto in comboProductoList)
				{

					ListaPrecioProducto listaPrecioProducto = listaPrecioProductoRepository.SingleSearch(new ListaPrecioProducto()
					{
						ListaPrecioId = cotizacion.ListaPrecioId,
						ProductoId = comboProducto.ProductoIncluidoId,
						Estado = EstadoConstante.ACTIVO
					});

					decimal precioUnitario = decimal.Round((listaPrecioProducto.PrecioUnitario * comboProducto.PorcentajePrecio) / 100,
						comboProducto.NumeroDecimalesRedondeo);

					CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
					{
						PrecioUnitario = precioUnitario,
						Cantidad = cotizacionProducto.Cantidad * comboProducto.Cantidad,
						TipoIGV = comboProducto.ProductoIncluido.TipoIgv.Codigo,
						ImpuestoTasa = listaPrecioProducto.ListaPrecio.ImpuestoTasa.Tasa
					});

					cotizacionProductoList.Add(new CotizacionProducto()
					{
						CotizacionId = cotizacionProducto.CotizacionId,
						Producto = comboProducto.ProductoIncluido,
						ProductoId = comboProducto.ProductoIncluidoId,
						Descripcion = string.Empty,
						Cantidad = cotizacionProducto.Cantidad * comboProducto.Cantidad,
						ValorUnitarioLista = listaPrecioProducto.ValorUnitario,
						PrecioUnitarioLista = listaPrecioProducto.PrecioUnitario,
						ValorUnitario = calculadorVentaItemDTO.ValorUnitario,
						PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario,
						CantidadBloqueada = SiNoConstante.N,
						IncluyeDescuentoListaPrecio = SiNoConstante.N,
						SinSaldo = SiNoConstante.N,
						SolicitaAutorizacionDescuento = SiNoConstante.N,
						PermiteDescuento = cotizacionProducto.PermiteDescuento,
						TipoDescuento = cotizacionProducto.TipoDescuento,
						TipoDescuentoId = cotizacionProducto.TipoDescuentoId,
						DescuentoValor = cotizacionProducto.DescuentoValor,
						PorcentajeDescuento = cotizacionProducto.PorcentajeDescuento,
						MontoBruto = calculadorVentaItemDTO.MontoBruto,
						MontoNeto = calculadorVentaItemDTO.MontoNeto,
						MontoIgv = calculadorVentaItemDTO.MontoImpuesto,
						MontoTotal = calculadorVentaItemDTO.MontoTotal,
						TipoAfectacionIgvId = cotizacionProducto.TipoAfectacionIgvId,
						RegistroComboId = cotizacionProducto.CotizacionProductoId,
						Estado = cotizacionProducto.Estado,
						UsuarioCreacionId = cotizacionProducto.UsuarioCreacionId
					});
				}

				foreach (CotizacionProducto cotizacionProductoTemp in cotizacionProductoList)
				{
					int newId = cotizacionProductoRepository.Insert(cotizacionProductoTemp);
					cotizacionProductoTemp.CotizacionProductoId = newId;

					if (cotizacion.EsPedido == SiNoConstante.S)
						actualizarReservaInterna(cotizacionProductoTemp, esNuevo);
				}
			}
			else
			{
				cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
				{
					CotizacionId = cotizacionProducto.CotizacionId,
					RegistroComboId = cotizacionProducto.EntityId,
					Estado = EstadoConstante.ACTIVO
				});

				foreach (ComboProducto comboProducto in comboProductoList)
				{

					CotizacionProducto cotizacionProductoTemp = cotizacionProductoList.
						Where(x => x.ProductoId == comboProducto.ProductoIncluidoId).FirstOrDefault();

					CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
					{
						PrecioUnitario = cotizacionProductoTemp.PrecioUnitario,
						Cantidad = cotizacionProducto.Cantidad * comboProducto.Cantidad,
						TipoIGV = comboProducto.ProductoIncluido.TipoIgv.Codigo,
						ImpuestoTasa = cotizacion.ImpuestoTasa.Tasa
					});

					cotizacionProductoTemp.Cantidad = cotizacionProducto.Cantidad * comboProducto.Cantidad;
					cotizacionProductoTemp.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
					cotizacionProductoTemp.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
					cotizacionProductoTemp.MontoBruto = calculadorVentaItemDTO.MontoBruto;
					cotizacionProductoTemp.MontoNeto = calculadorVentaItemDTO.MontoNeto;
					cotizacionProductoTemp.MontoIgv = calculadorVentaItemDTO.MontoImpuesto;
					cotizacionProductoTemp.MontoTotal = calculadorVentaItemDTO.MontoTotal;
					cotizacionProductoTemp.UsuarioCreacionId = cotizacionProducto.UsuarioCreacionId;
					cotizacionProductoTemp.UsuarioModificacionId = cotizacionProducto.UsuarioModificacionId ?? cotizacionProducto.UsuarioCreacionId;

					if (cotizacion.EsPedido == SiNoConstante.S)
						actualizarReservaInterna(cotizacionProductoTemp, esNuevo);

					cotizacionProductoRepository.Update(cotizacionProductoTemp);
				}
			}
		}
		private void eliminarReserva(CotizacionProducto cotizacionProducto)
		{

			decimal cantidadAnterior = cotizacionProducto.Cantidad;

			cotizacionProducto = cotizacionProductoRepository.GetById(cotizacionProducto.EntityId);

			if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.
				GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.SERVICIO)
				return;

			if (cotizacionProducto.Cotizacion.EsPedido == SiNoConstante.N)
				return;

			//MesContable mesContable = mesContableRepository.ObtenerPorCodigo(string.Format(StringFormatConstant.DATE_MONTH, cotizacionProducto.Cotizacion.Fecha));

			MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(new MovimientoAlmacenSaldo()
			{
				PeriodoEmpresaSedeId = cotizacionProducto.Cotizacion.PeriodoEmpresaSedeId,
				AlmacenId = cotizacionProducto.Cotizacion.AlmacenId.Value,
				ProductoId = cotizacionProducto.ProductoId,
				MesContableId = cotizacionProducto.Cotizacion.MesContableId.Value
			});

			movimientoAlmacenSaldo.CantidadReserva -= cantidadAnterior;

			movimientoAlmacenSaldoRepository.ActualizarCantidadReserva(movimientoAlmacenSaldo);

		}
		private void eliminarCombo(CotizacionProducto cotizacionProducto)
		{
			List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
			{
				CotizacionId = cotizacionProducto.CotizacionId,
				RegistroComboId = cotizacionProducto.EntityId,
				Estado = EstadoConstante.ACTIVO
			});

			foreach (CotizacionProducto comprobanteProductoTemp in cotizacionProductoList)
			{
				if(cotizacionProducto.Cotizacion.EsPedido == SiNoConstante.S)
					eliminarReserva(comprobanteProductoTemp);

				comprobanteProductoTemp.UsuarioCreacionId = cotizacionProducto.UsuarioCreacionId;
				comprobanteProductoTemp.UsuarioModificacionId = cotizacionProducto.UsuarioModificacionId ?? cotizacionProducto.UsuarioCreacionId;
				cotizacionProductoRepository.Delete(comprobanteProductoTemp);
			}
		}
		private void validarMontoMinimoAnticipo(CotizacionProducto cotizacionProducto, bool esNuevo)
      {
			if (cotizacionProducto.Cotizacion == null)
				return;

			if (cotizacionProducto.Cotizacion.EsPedido == SiNoConstante.N)
				return;

			Cotizacion cotizacion = cotizacionRepository.GetById(cotizacionProducto.CotizacionId);

			decimal montoAnticipo = cotizacion.MontoAnticipo;
			decimal montoTotal = cotizacion.MontoTotal;
			decimal monto = cotizacionProducto.MontoTotal;
			decimal montoAnterior = decimal.Zero;

			if (!esNuevo)
         {
				CotizacionProducto cotizacionProductoTmp = cotizacionProductoRepository.GetById(cotizacionProducto.EntityId);
				montoAnterior = cotizacionProductoTmp.MontoTotal;
			}
			montoTotal = (montoTotal - montoAnterior) + monto;

			decimal montoMinimoAnticipo = decimal.Round(montoTotal / 2, decimal.Zero);
			if (montoAnticipo < montoMinimoAnticipo)
			{
				CotizacionAutorizacion cotizacionAutorizacion = cotizacionAutorizacionRepository.SingleSearch(new CotizacionAutorizacion()
				{
					Cotizacion = new Cotizacion()
					{
						PeriodoEmpresaSede = new PeriodoEmpresaSede()
						{
							EmpresaSede = new EmpresaSede()
							{
								EmpresaId = cotizacion.PeriodoEmpresaSede.EmpresaSede.EmpresaId
							}
						}
					},
					CotizacionId = cotizacion.CotizacionId,
					TipoAutorizacion = new TipoAutorizacion() { Codigo = TipoAutorizacionConstante.NO_REQUERIR_ADELANTO },
					EstadoAprobacion = new EstadoAprobacion() { Codigo = EstadoAprobacionConstante.APROBADO },
					Estado = EstadoConstante.ACTIVO
				});

				if (cotizacionAutorizacion == null)
					throw new Exception(SR.GetString("EX_cotizacion_sin_anticipo"));
			}
		}
      public void ActualizarReserva(CotizacionProductoDTO dto)
      {
			actualizarReservaInterna(Mapper.Map<CotizacionProducto>(dto), true);
      }
		public void ActualizarReservaCombo(CotizacionProductoDTO dto)
		{
			List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
			{
				CotizacionId = dto.CotizacionId,
				RegistroComboId = dto.Id,
				Estado = EstadoConstante.ACTIVO
			});

			foreach (CotizacionProducto cotizacionProducto in cotizacionProductoList)
				actualizarReservaInterna(cotizacionProducto, true);
		}
		public void ActualizarSinSaldo(CotizacionProductoDTO dto)
		{
			try
			{
				int usuarioAccionId = (dto.UsuarioModificacionId.HasValue && dto.UsuarioModificacionId.Value > 0)
					? dto.UsuarioModificacionId.Value
					: dto.UsuarioCreacionId;

				CotizacionProducto cotizacionProducto = Mapper.Map<CotizacionProducto>(dto);
				cotizacionProducto.UsuarioCreacionId = usuarioAccionId;
				cotizacionProducto.UsuarioModificacionId = usuarioAccionId;

				using (TransactionScope tx = new())
				{
					cotizacionProductoRepository.ActualizarSinSaldo(cotizacionProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

       public List<CotizacionProductoDTO> ProductosParcial(CotizacionProductoDTO dto)
       {
            try
            {
				Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

				List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(Mapper.Map<CotizacionProducto>(dto));

                List<CotizacionProductoDTO> cotizacionProductoDTOList = Mapper.Map<List<CotizacionProductoDTO>>(cotizacionProductoList);

				List<CotizacionProductoDTO> cortesGratisDTOList = CortesGratis(new CotizacionProductoDTO()
				{
					CotizacionDTO = dto.CotizacionDTO,
					CotizacionId = dto.CotizacionId,
					Estado = EstadoConstante.ACTIVO
				});

                List<CotizacionProductoDTO> ProductosParciales = new List<CotizacionProductoDTO>();

                if (cotizacion.PeriodoEmpresaSede.EmpresaSede.EmpresaSedeId == 2)
				{
					ProductosParciales = cotizacionProductoDTOList.Where(p => (p.Cantidad - p.CantidadParcial) > 0 && p.ProductoId != 3083 && p.ProductoId != 3946).ToList();
				}
				else{
                    ProductosParciales = cotizacionProductoDTOList.Where(p => (p.Cantidad - p.CantidadParcial) > 0 && p.ProductoId != 3083).ToList();
                }
                ProductosParciales.AddRange(cortesGratisDTOList);
                return ProductosParciales;
            }
            catch (Exception)
            {
                throw;
            }
       }

        public void ActualizarSaldoProduccion(CotizacionProductoDTO dto)
		{
			try
			{
				CotizacionProducto cotizacionProducto = cotizacionProductoRepository.GetById(dto.CotizacionProductoId);

				var cantidadSobrante = cotizacionProducto.Cantidad - cotizacionProducto.CantidadParcial;

				if(cantidadSobrante <= 0){
                    throw new Exception(SR.GetString("EX_cotizacion_producto_parcial"));
                }

				CotizacionProducto cotizaProd = new CotizacionProducto()
				{
					CotizacionProductoId = dto.CotizacionProductoId,
					CantidadParcial = dto.CantidadParcial
				};

                using (TransactionScope tx = new())
                {
                    cotizacionProductoRepository.ActualizarSaldoProduccion(cotizaProd);

                    tx.Complete();
                }
            }
			catch (Exception)
			{
				throw;
			}
		}
		
		// SOLO CONTEMPLAR MELAMINA
		public List<CotizacionProductoDTO> CortesGratis(CotizacionProductoDTO dto)
		{
			try
			{
				Cotizacion cotiza = cotizacionRepository.GetById(dto.CotizacionId);

				List<CotizacionProducto> cotizaProdList = cotizacionProductoRepository.Search(new CotizacionProducto()
				{
					CotizacionId = dto.CotizacionId,
					Estado = EstadoConstante.ACTIVO
				});

				List<CotizacionProductoDTO> cotizacionProductoDTOList = Mapper.Map<List<CotizacionProductoDTO>>(cotizaProdList);

				List<CortesGratisDTO> cortesGratisDTOList = cortesGratisBusiness.Search(new CortesGratisDTO()
				{
					EmpresaSedeId = cotiza.PeriodoEmpresaSede.EmpresaSedeId,
					Estado = EstadoConstante.ACTIVO
				});

				var CorteLinealCoti = cotizaProdList.Where(x => x.ProductoId == 3083 && x.Estado == EstadoConstante.ACTIVO).FirstOrDefault();
				CotizacionProductoDTO CorteLinealCotiDTO = Mapper.Map<CotizacionProductoDTO>(CorteLinealCoti);

				var CorteAgloCoti = cotizaProdList.Where(x => x.ProductoId == 3946 && x.Estado == EstadoConstante.ACTIVO).FirstOrDefault();
				CotizacionProductoDTO CorteAgloCotiDTO = Mapper.Map<CotizacionProductoDTO>(CorteAgloCoti);

				Producto productocl = productoRepository.GetById(3083);
				Producto productoca = productoRepository.GetById(3946);


                var cortesLinealGratis =(from producto in cotizacionProductoDTOList 
										 join corte in cortesGratisDTOList on producto.ProductoId equals corte.ProductoId
										 where corte.Cantidad > 0
										 select producto.Cantidad * corte.Cantidad).Sum();


                CotizacionProductoDTO corteLinealMel = new CotizacionProductoDTO()
                {
                    CotizacionId = cotiza.CotizacionId,
                    CotizacionProductoId = CorteLinealCotiDTO?.CotizacionProductoId ?? 0,
                    ProductoId = productocl.ProductoId,
                    ProductoDTO = Mapper.Map<ProductoDTO>(productocl),
                    Cantidad = CorteLinealCotiDTO?.Cantidad ?? 0,
                    CortesGratis = cortesLinealGratis,
                    Descripcion = CorteLinealCotiDTO?.Descripcion ?? string.Empty,
                };

                CotizacionProductoDTO corteAglo = new CotizacionProductoDTO()
                {
                    CotizacionId = cotiza.CotizacionId,
                    CotizacionProductoId = CorteAgloCotiDTO?.CotizacionProductoId ?? 0,
                    ProductoId = productoca.ProductoId,
                    ProductoDTO = Mapper.Map<ProductoDTO>(productoca),
                    Cantidad = CorteAgloCotiDTO?.Cantidad ?? 0,
                    CortesGratis = 0,
                    Descripcion = CorteAgloCotiDTO?.Descripcion ?? string.Empty,
                };

                List<CotizacionProductoDTO> result = new List<CotizacionProductoDTO>();

                result.Add(corteLinealMel);

                if (cotiza.PeriodoEmpresaSede.EmpresaSede.EntityId == 2)
				{
                    result.Add(corteAglo);
				}		

				return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CotizacionProductoDTO> CortesGratisIlo(CotizacionProductoDTO dto)
		{
			try
			{
				CotizacionDTO cotizaDTO = Mapper.Map<CotizacionDTO>(cotizacionRepository.GetById(dto.CotizacionId));

				List<CotizacionProducto> ProdsParciales = cotizacionProductoRepository.Search(new CotizacionProducto()
				{
					CotizacionId = dto.CotizacionId,
					Estado = EstadoConstante.ACTIVO,
				});

				List<CotizacionProductoDTO> prodsParcialDTO = Mapper.Map<List<CotizacionProductoDTO>>(ProdsParciales);

				cotizaDTO.CotizacionProductoDTOList = prodsParcialDTO;

				var Prods = (from producto in prodsParcialDTO
							 where producto.ProductoId != 3083
							 select producto).ToList();

				CotizacionProductoDTO cotizaProdDTO = prodsParcialDTO.Where(x => x.ProductoId == 3083).FirstOrDefault();

				CotizacionProductoDTO corteLinealDTO = new CotizacionProductoDTO()
				{
					CotizacionProductoId = cotizaProdDTO?.CotizacionProductoId ?? 0,
					CotizacionDTO = cotizaDTO,
					CotizacionId = cotizaDTO.CotizacionId,
					ProductoDTO = Mapper.Map<ProductoDTO>(productoRepository.GetById(3083)),
					ProductoId = 3083,
					CortesGratis = 0,
					UnidadMedidaDTO = cotizaProdDTO?.UnidadMedidaDTO,
				};


				List<CotizacionProductoDTO> result = Prods;
				result.Add(corteLinealDTO);
				return result;
            }
			catch (Exception)
			{
				throw;
			}
		}

    }
}