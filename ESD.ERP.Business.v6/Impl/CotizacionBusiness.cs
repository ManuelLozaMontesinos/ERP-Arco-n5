using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
	public class CotizacionBusiness : ICotizacionBusiness
	{
		private readonly ICotizacionRepository cotizacionRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly ICotizacionProductoRepository cotizacionProductoRepository;
		private readonly ICotizacionProductoBusiness cotizacionProductoBusiness;
		private readonly IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository;
		private readonly IMedioPagoRepository medioPagoRepository;
		private readonly IParametroSistemaRepository parametroSistemaRepository;
		private readonly ITipoComprobanteRepository tipoComprobanteRepository;
		private readonly ITipoDetraccionRepository tipoDetraccionRepository;
		private readonly IEstadoComprobanteRepository estadoComprobanteRepository;
		private readonly IComprobanteBusiness comprobanteBusiness;
		private readonly IComprobanteProductoBusiness comprobanteProductoBusiness;
		private readonly IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness;
		private readonly IEstadoCotizacionRepository estadoCotizacionRepository;
		private readonly ICotizacionAutorizacionRepository cotizacionAutorizacionRepository;
		private readonly IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository;
		private readonly IMovimientoAlmacenCierreRepository movimientoAlmacenCierreRepository;
		private readonly IAnticipoClienteRepository anticipoClienteRepository;
		private readonly IOrdenProduccionBusiness ordenProduccionBusiness;
		private readonly IOrdenProduccionProductoBusiness ordenProduccionProductoBusiness;
		private readonly IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository;
		private readonly IRutaProduccionRepository rutaProduccionRepository;
		private readonly IRutaSubProcesoProduccionRepository rutaSubProcesoProduccionRepository;
		private readonly IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository;
		private readonly IProductoSubProcesoProduccionRepository productoSubProcesoProduccionRepository;
		private readonly IEstadoDespachoRepository estadoDespachoRepository;
		private readonly IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository;
		private readonly ITipoDocumentoComercialRepository tipoDocumentoComercialRepository;
		private readonly IProductoRepository productoRepository;
		private readonly ICortesGratisRepository cortesGratisRepository;
		private readonly ICortesGratisBusiness cortesGratisBusiness;
		private readonly IOrdenProduccionRepository ordenProduccionRepository;
		private readonly IOrdenProduccionProductoRepository ordenProduccionProductoRepository;
		private readonly ICotizacionAutorizacionBusiness cotizacionAutorizacionBusiness;

		public CotizacionBusiness(
			ICotizacionRepository cotizacionRepository,
			ITablaBusiness tablaBusiness,
			ICotizacionProductoRepository cotizacionProductoRepository,
			IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository,
			IMedioPagoRepository medioPagoRepository,
			IParametroSistemaRepository parametroSistemaRepository,
			ITipoComprobanteRepository tipoComprobanteRepository,
			ITipoDetraccionRepository tipoDetraccionRepository,
			IEstadoComprobanteRepository estadoComprobanteRepository,
			IComprobanteBusiness comprobanteBusiness,
			IComprobanteProductoBusiness comprobanteProductoBusiness,
			IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness,
			IEstadoCotizacionRepository estadoCotizacionRepository,
			ICotizacionAutorizacionRepository cotizacionAutorizacionRepository,
			IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository,
			IMovimientoAlmacenCierreRepository movimientoAlmacenCierreRepository,
			IAnticipoClienteRepository anticipoClienteRepository,
			IOrdenProduccionBusiness ordenProduccionBusiness,
			IOrdenProduccionProductoBusiness ordenProduccionProductoBusiness,
			IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository,
			IRutaProduccionRepository rutaProduccionRepository,
			IRutaSubProcesoProduccionRepository rutaSubProcesoProduccionRepository,
			IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository,
			IProductoSubProcesoProduccionRepository productoSubProcesoProduccionRepository,
			IEstadoDespachoRepository estadoDespachoRepository,
			IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository,
			ITipoDocumentoComercialRepository tipoDocumentoComercialRepository,
			IProductoRepository productoRepository,
			ICotizacionProductoBusiness cotizacionProductoBusiness,
			ICortesGratisBusiness cortesGratisBusiness,
			IOrdenProduccionRepository ordenProduccionRepository,
			IOrdenProduccionProductoRepository ordenProduccionProductoRepository,
			ICotizacionAutorizacionBusiness cotizacionAutorizacionBusiness,
			ICortesGratisRepository cortesGratisRepository

            )
        {
			this.cotizacionRepository = cotizacionRepository;
			this.tablaBusiness = tablaBusiness;
			this.cotizacionProductoRepository = cotizacionProductoRepository;
			this.cotizacionProductoBusiness = cotizacionProductoBusiness;
			this.periodoEmpresaSedeRepository = periodoEmpresaSedeRepository;
			this.medioPagoRepository = medioPagoRepository;
			this.parametroSistemaRepository = parametroSistemaRepository;
			this.tipoComprobanteRepository = tipoComprobanteRepository;
			this.tipoDetraccionRepository = tipoDetraccionRepository;
			this.estadoComprobanteRepository = estadoComprobanteRepository;
			this.comprobanteBusiness = comprobanteBusiness;
			this.comprobanteProductoBusiness = comprobanteProductoBusiness;
			this.movimientoAlmacenSaldoBusiness = movimientoAlmacenSaldoBusiness;
			this.estadoCotizacionRepository = estadoCotizacionRepository;
			this.cotizacionAutorizacionRepository = cotizacionAutorizacionRepository;
			this.movimientoAlmacenSaldoRepository = movimientoAlmacenSaldoRepository;
			this.movimientoAlmacenCierreRepository = movimientoAlmacenCierreRepository;
			this.anticipoClienteRepository = anticipoClienteRepository;
			this.ordenProduccionBusiness = ordenProduccionBusiness;
			this.ordenProduccionProductoBusiness = ordenProduccionProductoBusiness;
			this.estadoOrdenProduccionRepository = estadoOrdenProduccionRepository;
			this.rutaProduccionRepository = rutaProduccionRepository;
			this.rutaSubProcesoProduccionRepository = rutaSubProcesoProduccionRepository;
			this.ordenProduccionSubProcesoRepository = ordenProduccionSubProcesoRepository;
			this.productoSubProcesoProduccionRepository = productoSubProcesoProduccionRepository;
			this.estadoDespachoRepository = estadoDespachoRepository;
			this.estadoOrdenProduccionSubProcesoRepository = estadoOrdenProduccionSubProcesoRepository;
			this.tipoDocumentoComercialRepository = tipoDocumentoComercialRepository;
			this.productoRepository = productoRepository;
			this.cortesGratisRepository = cortesGratisRepository;
			this.ordenProduccionRepository = ordenProduccionRepository;
			this.ordenProduccionProductoRepository = ordenProduccionProductoRepository;
			this.cotizacionAutorizacionBusiness = cotizacionAutorizacionBusiness;
			this.cortesGratisBusiness = cortesGratisBusiness;

        }
		public CotizacionDTO GetById(int id)
		{
			try
			{
				Cotizacion cotizacion = cotizacionRepository.GetById(id);

				CotizacionDTO cotizacionDTO = Mapper.Map<CotizacionDTO>(cotizacion);

				return cotizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CotizacionDTO> Search(CotizacionDTO dto)
		{
			try
			{
				List<Cotizacion> cotizacionList = cotizacionRepository.Search(Mapper.Map<Cotizacion>(dto));

				List<CotizacionDTO> cotizacionDTOList = Mapper.Map<List<CotizacionDTO>>(cotizacionList);

				return cotizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CotizacionDTO SingleSearch(CotizacionDTO dto)
		{
			try
			{
				Cotizacion cotizacion = cotizacionRepository.SingleSearch(Mapper.Map<Cotizacion>(dto));

				CotizacionDTO cotizacionDTO = Mapper.Map<CotizacionDTO>(cotizacion);

				return cotizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CotizacionDTO dto)
		{
			try
			{
				Cotizacion cotizacion = Mapper.Map<Cotizacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.COTIZACION,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId
						},
						UsuarioAccionId = cotizacion.UsuarioCreacionId
					};

					cotizacion.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = cotizacionRepository.Insert(cotizacion);

					dto.CotizacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CotizacionDTO dto)
		{
			try
			{
				Cotizacion cotizacion = Mapper.Map<Cotizacion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = cotizacionRepository.Update(cotizacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CotizacionDTO dto)
		{
			try
			{
				Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

				List<ComprobanteDTO> comprobanteDTOList = comprobanteBusiness.Search(new ComprobanteDTO() { 
					PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId,
					CotizacionId = cotizacion.EntityId,
					Estado = EstadoConstante.ACTIVO
				});

				if (comprobanteDTOList.Count > 0)
            {
					string message = string.Empty;
					if (cotizacion.EsPedido == SiNoConstante.S)
						message = SR.GetString("EX_pedido_con_comprobantes");
					else
						message = SR.GetString("EX_cotizacion_con_comprobantes");

					throw new Exception(message);
				}

				EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.ObtenerPorCodigo(EstadoCotizacionConstante.ANULADO);
				cotizacion.EstadoCotizacionId = estadoCotizacion.EntityId;
				cotizacion.Estado = EstadoConstante.INACTIVO;

				using (TransactionScope tx = new TransactionScope())
				{

					if(cotizacion.EsPedido == SiNoConstante.S)
               {
						List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
						{
							CotizacionId = dto.CotizacionId,
							Estado = EstadoConstante.ACTIVO
						});

						foreach (CotizacionProducto cotizacionProducto in cotizacionProductoList)
							eliminarReserva(cotizacionProducto);
					}

					cotizacionRepository.Delete(cotizacion);

					cotizacionProductoRepository.EliminarPorComprobanteId(new CotizacionProducto() { CotizacionId = dto.CotizacionId });

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}


		 public int GenerarComprobante(CotizacionDTO dto)
		 {
			List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
			{
				CotizacionId = dto.CotizacionId,
				NullRegistroComboId = SiNoConstante.S,
				Estado = EstadoConstante.ACTIVO
			});

			if (cotizacionProductoList.Count == 0)
				throw new Exception(SR.GetString("EX_cotizacion_sin_items"));

			PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(dto.PeriodoEmpresaSedeId);

			MedioPago medioPago = medioPagoRepository.SingleSearch(new MedioPago() { 
				EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId,
				Codigo = MedioPagoConstante.EFECTIVO
			});

			TipoComprobante tipoComprobante = tipoComprobanteRepository.ObtenerPorCodigo(TipoComprobanteConstante.NORMAL);

			TipoDetraccion tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);

			EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.PENDIENTE);
			EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.ObtenerPorCodigo(EstadoCotizacionConstante.PEDIDO);
			EstadoDespacho estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(EstadoDespachoConstante.PENDIENTE);

			TipoDocumentoComercial tipoDocumentoComercial = tipoDocumentoComercialRepository.GetById(dto.TipoDocumentoComercialId);

			ParametroSistema parametroSistema = parametroSistemaRepository.SingleSearch(new ParametroSistema() {
				Codigo = ParametroSistemaConstante.DETRACCION_MONTO_MAX_PRODUCTOS
			});

			decimal montoMaximoProductos = parametroSistema.ValorNumerico;
			string[] tipoProductoList = null;

			if(tipoDocumentoComercial.EsInterno == SiNoConstante.S)
         {
				tipoProductoList = new string[]
				{
					TipoProductoConstante.PRODUCTO
				};
			}
         else
         {
				if (cotizacionProductoList.Sum(x => x.MontoTotal) > montoMaximoProductos)
					tipoProductoList = new string[]
					{
						TipoProductoConstante.PRODUCTO,
						TipoProductoConstante.SERVICIO
					};
				else
					tipoProductoList = new string[]
					{
						TipoProductoConstante.PRODUCTO
					};
			}

			try
			{
				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{

					dto.EsPedido = SiNoConstante.S;
					dto.AlmacenId = dto.AlmacenComprobanteId.Value;
					dto.MesContableId = dto.MesContableId.Value;
					cotizacionRepository.Update(Mapper.Map<Cotizacion>(dto));

					cotizacionRepository.ActualizarEstado(new Cotizacion() { 
						CotizacionId = dto.CotizacionId,
						EstadoCotizacionId = estadoCotizacion.EntityId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

					foreach (string tipoProducto in tipoProductoList)
					{
						List<CotizacionProducto> cotizacionProductoListTmp = null;

						if (tipoProductoList.Length > 1)
							cotizacionProductoListTmp = cotizacionProductoList.Where(x => x.Producto.FamiliaProducto.ClaseProducto.
													 GrupoProducto.TipoProducto.Codigo == tipoProducto).ToList();
						else
							cotizacionProductoListTmp = cotizacionProductoList;

						foreach (CotizacionProducto cotizacionProducto in cotizacionProductoListTmp)
						{
							if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.
													 GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.PRODUCTO)
							{
								movimientoAlmacenSaldoBusiness.ValidarSaldoVenta(new MovimientoAlmacenSaldoDTO()
								{
									PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId,
									ProductoId = cotizacionProducto.ProductoId,
									MesContableId = cotizacion.MesContableId.Value,
									CantidadIngreso = cotizacionProducto.Cantidad
								});
							}
						}

                        foreach (CotizacionProducto cotizacionProducto in cotizacionProductoListTmp)
                        {
                            if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.EsCombo == SiNoConstante.N)
                                actualizarReserva(cotizacionProducto, true);
                            else
                            {
                                List<CotizacionProducto> cotizacionProductoList2Tmp = cotizacionProductoRepository.Search(new CotizacionProducto()
                                {
                                    CotizacionId = cotizacionProducto.CotizacionId,
                                    RegistroComboId = cotizacionProducto.CotizacionProductoId,
                                    Estado = EstadoConstante.ACTIVO
                                });

                                foreach (CotizacionProducto cotizacionProducto2 in cotizacionProductoList2Tmp)
                                    actualizarReserva(cotizacionProducto2, true);
                            }
                        }

                        if (cotizacionProductoListTmp.Count > 0)
						{
							decimal montoBruto = cotizacionProductoListTmp.Sum(x => x.MontoBruto);
							decimal montoDescuento1 = cotizacionProductoListTmp.Sum(x => x.MontoDescuento1);
							decimal MontoDescuento2 = cotizacionProductoListTmp.Sum(x => x.MontoDescuento2);
							decimal montoNeto = cotizacionProductoListTmp.Sum(x => x.MontoNeto);
							decimal montoIgv = cotizacionProductoListTmp.Sum(x => x.MontoIgv);
							decimal montoIsc = cotizacionProductoListTmp.Sum(x => x.MontoIsc);
							decimal montoTotal = cotizacionProductoListTmp.Sum(x => x.MontoTotal);

							ComprobanteDTO comprobanteDTO = new()
							{
								PeriodoEmpresaSedeDTO = dto.PeriodoEmpresaSedeDTO,
								PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId,
								TipoDocumentoComercialId = dto.TipoDocumentoComercialId,
								Serie = dto.SerieComprobante,
								Numero = string.Empty,
								Fecha = dto.FechaComprobante,
								CotizacionId = cotizacion.CotizacionId,
								ProveedorId = dto.ProveedorComprobanteId,
								NombreAlterno = string.Empty,
								VendedorId = cotizacion.VendedorId,
								MonedaId = cotizacion.MonedaId,
								MontoTipoCambio = cotizacion.MontoTipoCambio,
								ListaPrecioId = cotizacion.ListaPrecioId,
								TipoCondicionPagoId = cotizacion.TipoCondicionPagoId,
								AlmacenId = dto.AlmacenComprobanteId,
								Observacion = cotizacion.Observacion,
								MedioPagoId = medioPago.EntityId,
								ImpuestoTasaId = cotizacion.ImpuestoTasaId,
								NumeroGuiaRemision = string.Empty,
								MontoBruto = montoBruto,
								MontoDescuentoItem = montoDescuento1 + MontoDescuento2,
								MontoNeto = montoNeto,
								MontoIgv = montoIgv,
								MontoIsc = montoIsc,
								MontoTotal = montoTotal,
								Canjeado = SiNoConstante.N,
								TipoComprobanteId = tipoComprobante.EntityId,
								TipoDetraccionId = tipoDetraccion.EntityId,
								NumeroOperacionPago = string.Empty,
								EsContingencia = dto.SerieEsContingencia,
								Emitido = SiNoConstante.N,
								CodigoHash = string.Empty,
								EstadoComprobanteId = estadoComprobante.EntityId,
								Bancarizado = SiNoConstante.N,
								EstadoDespachoId = estadoDespacho.EntityId,
								ObservacionDespacho = string.Empty,
								EntregaDespacho = string.Empty,
								UsuarioCreacionId = dto.UsuarioCreacionId
							};

							int newIdTmp = comprobanteBusiness.Insert(comprobanteDTO);
							if (newId == 0)
								newId = newIdTmp;

							foreach(CotizacionProducto cotizacionProducto in cotizacionProductoListTmp)
                     {

								ComprobanteProductoDTO comprobanteProductoDTO = new()
								{
									ComprobanteId = newIdTmp,
									ProductoId = cotizacionProducto.ProductoId,
									ProductoDTO = Mapper.Map<ProductoDTO>(cotizacionProducto.Producto),
									Descripcion = cotizacionProducto.Descripcion,
									Cantidad = cotizacionProducto.Cantidad,
									UnidadMedidaId = cotizacionProducto.UnidadMedidaId,
									CantidadPresentacion = cotizacionProducto.CantidadPresentacion,
									CantidadEquivalente = cotizacionProducto.CantidadEquivalente,
									ValorUnitarioLista = cotizacionProducto.ValorUnitarioLista,
									PrecioUnitarioLista = cotizacionProducto.PrecioUnitarioLista,
									ValorUnitario = cotizacionProducto.ValorUnitario,
									PrecioUnitario = cotizacionProducto.PrecioUnitario,
									IncluyeDescuentoListaPrecio = cotizacionProducto.IncluyeDescuentoListaPrecio,
									PermiteDescuento = cotizacionProducto.PermiteDescuento,
									CantidadBloqueada = cotizacionProducto.CantidadBloqueada,
									SolicitaAutorizacionDescuento = SiNoConstante.N,
									RegistroReferencialId = cotizacionProducto.EntityId,
									MontoBruto = cotizacionProducto.MontoBruto,
									TipoDescuentoId = cotizacionProducto.TipoDescuentoId,
									DescuentoValor = cotizacionProducto.DescuentoValor,
									PorcentajeDescuento = cotizacionProducto.PorcentajeDescuento,
									MontoDescuento1 = cotizacionProducto.MontoDescuento1,
									MontoDescuento2 = cotizacionProducto.MontoDescuento2,
									MontoNeto = cotizacionProducto.MontoNeto,
									MontoIgv = cotizacionProducto.MontoIgv,
									MontoIsc = cotizacionProducto.MontoIsc,
									MontoTotal = cotizacionProducto.MontoTotal,
									TipoDetraccionId = cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.TipoDetraccion.TipoDetraccionId,
									PorcentajeDetraccion = cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.TipoDetraccion.Tasa,
									MontoDetraccion = CalculadorVenta.CalcularDetraccion(cotizacionProducto.MontoTotal, 
										cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.TipoDetraccion.Tasa),
									TipoAfectacionIgvId = cotizacionProducto.TipoAfectacionIgvId,
									ObservacionDespacho = string.Empty,
									EntregaDespacho = string.Empty,
									Estado = EstadoConstante.ACTIVO,
									UsuarioCreacionId = dto.UsuarioCreacionId
								};

								comprobanteProductoBusiness.Insert(comprobanteProductoDTO);

								cotizacionProductoRepository.ActualizarCantidadComprobante(new CotizacionProducto()
								{
									CotizacionId = cotizacion.CotizacionId,
									CotizacionProductoId = cotizacionProducto.EntityId
								});
							}
						}
					}

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		 } 


      public void GenerarPedido(CotizacionDTO dto)
      {
			List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
			{
				CotizacionId = dto.CotizacionId,
				NullRegistroComboId = SiNoConstante.S,
				Estado = EstadoConstante.ACTIVO
			});

			if (cotizacionProductoList.Count == 0)
				throw new Exception(SR.GetString("EX_cotizacion_sin_items"));

			Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

			decimal montoMinimoAnticipo = decimal.Round(cotizacion.MontoTotal / 2, 2);
			decimal montoAnticipo = decimal.Zero;

			List<AnticipoCliente> anticipoClienteList = anticipoClienteRepository.Search(new AnticipoCliente() { 
				CotizacionId = cotizacion.CotizacionId,
				TipoOperacionAnticipo = new TipoOperacionAnticipo() { Codigo = TipoOperacionAnticipoConstante.ANTICIPO},
				EstadoComprobante = new EstadoComprobante() { Codigo = EstadoComprobanteConstante.EMITIDO },
				Estado = EstadoConstante.ACTIVO
			});

			montoAnticipo = decimal.Round(anticipoClienteList.Sum(x => x.Monto),2);

			if (montoAnticipo < montoMinimoAnticipo)
			{
				CotizacionAutorizacion cotizacionAutorizacion = cotizacionAutorizacionRepository.SingleSearch(new CotizacionAutorizacion() { 
					Cotizacion = new Cotizacion()
				{
						PeriodoEmpresaSede = new PeriodoEmpresaSede() { EmpresaSede = new EmpresaSede()
							{
								EmpresaId = dto.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId
							}
						}
				},
					CotizacionId = dto.CotizacionId,
					TipoAutorizacion = new TipoAutorizacion() { Codigo = TipoAutorizacionConstante.NO_REQUERIR_ADELANTO },
					EstadoAprobacion = new EstadoAprobacion() { Codigo = EstadoAprobacionConstante.APROBADO},
					Estado = EstadoConstante.ACTIVO
				});

				if (cotizacionAutorizacion == null)
					throw new Exception(SR.GetString("EX_cotizacion_sin_anticipo"));
			}

			EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.ObtenerPorCodigo(EstadoCotizacionConstante.PEDIDO);

			try
			{
				using (TransactionScope tx = new TransactionScope())
				{
					dto.EsPedido = SiNoConstante.S;
					cotizacionRepository.Update(Mapper.Map<Cotizacion>(dto));

					cotizacionRepository.ActualizarEstado(new Cotizacion()
					{
						CotizacionId = dto.CotizacionId,
						EstadoCotizacionId = estadoCotizacion.EntityId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

					List<CotizacionProducto> cotizacionProductoListTmp = cotizacionProductoList.Where(x => x.Producto.FamiliaProducto.ClaseProducto.
													 GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.PRODUCTO).ToList();

					//MesContable mesContable = mesContableRepository.ObtenerPorCodigo(string.Format(StringFormatConstant.DATE_MONTH, dto.Fecha));

					foreach (CotizacionProducto cotizacionProducto in cotizacionProductoListTmp)
					{
						if(cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.EsCombo == SiNoConstante.N)
							movimientoAlmacenSaldoBusiness.ValidarSaldoVenta(new MovimientoAlmacenSaldoDTO()
							{
								PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId,
								ProductoId = cotizacionProducto.ProductoId,
								MesContableId = cotizacion.MesContableId.Value,
								CantidadIngreso = cotizacionProducto.Cantidad
							});
                  else
                  {
							List<CotizacionProducto> cotizacionProductoList2Tmp = cotizacionProductoRepository.Search(new CotizacionProducto() { 
								RegistroComboId = cotizacionProducto.CotizacionProductoId,
								Estado = EstadoConstante.ACTIVO
							});

							List<MovimientoAlmacenSaldoDTO> movimientoAlmacenSaldoDTOList = new List<MovimientoAlmacenSaldoDTO>();
							foreach (CotizacionProducto cotizacionProducto2 in cotizacionProductoList2Tmp)
							{
								movimientoAlmacenSaldoDTOList.Add(new MovimientoAlmacenSaldoDTO()
								{
									PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId,
									ProductoId = cotizacionProducto2.ProductoId,
									MesContableId = cotizacion.MesContableId.Value,
									CantidadIngreso = cotizacionProducto2.Cantidad,
								});
							}

							movimientoAlmacenSaldoBusiness.ValidarSaldoVentaCombo(movimientoAlmacenSaldoDTOList);
						}
					}

					foreach (CotizacionProducto cotizacionProducto in cotizacionProductoListTmp)
					{
						if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.EsCombo == SiNoConstante.N)
							actualizarReserva(cotizacionProducto, true);
					else
					{
							List<CotizacionProducto> cotizacionProductoList2Tmp = cotizacionProductoRepository.Search(new CotizacionProducto()
							{
								CotizacionId = cotizacionProducto.CotizacionId,
								RegistroComboId = cotizacionProducto.CotizacionProductoId,
								Estado = EstadoConstante.ACTIVO
							});

							foreach (CotizacionProducto cotizacionProducto2 in cotizacionProductoList2Tmp)
								actualizarReserva(cotizacionProducto2, true);
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
		private void actualizarReserva(CotizacionProducto cotizacionProducto, bool esNuevo)
		{

			decimal cantidad = cotizacionProducto.Cantidad;
			decimal cantidadAnterior = decimal.Zero;

			cotizacionProducto = cotizacionProductoRepository.GetById(cotizacionProducto.EntityId);

			if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.
				GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.SERVICIO)
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
		private void eliminarReserva(CotizacionProducto cotizacionProducto)
		{

			decimal cantidadAnterior = cotizacionProducto.Cantidad;

			cotizacionProducto = cotizacionProductoRepository.GetById(cotizacionProducto.EntityId);

			if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.
				GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.SERVICIO)
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
      public void CambiarEstado(CotizacionDTO dto)
      {
			try
			{

				EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.GetById(dto.EstadoCotizacionId);

				List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
				{
					CotizacionId = dto.CotizacionId,
					Estado = EstadoConstante.ACTIVO
				});

				using (TransactionScope tx = new TransactionScope())
				{
					Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

					string estadoCotizacionAnterior = cotizacion.EstadoCotizacion.Codigo;

					if (estadoCotizacion.Codigo == EstadoCotizacionConstante.COTIZACION)
					{
						cotizacion.EsPedido = SiNoConstante.N;
						cotizacionRepository.Update(cotizacion);
					}

					cotizacion.EstadoCotizacionId = estadoCotizacion.EntityId;
					cotizacionRepository.ActualizarEstado(cotizacion);

					MovimientoAlmacenCierre movimientoAlmacenCierre = movimientoAlmacenCierreRepository.SingleSearch(new MovimientoAlmacenCierre() {
						PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId,
						AlmacenId = cotizacion.AlmacenId.Value,
						MesContableId = cotizacion.MesContableId.Value
					});

					if(movimientoAlmacenCierre.EstadoAperturaCierre.Codigo == EstadoAperturaCierreConstante.APERTURA)
					{
						List<CotizacionProducto> cotizacionProductoListTmp = cotizacionProductoList.Where(x => x.Producto.FamiliaProducto.ClaseProducto.
														 GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.PRODUCTO).ToList();

						foreach (CotizacionProducto cotizacionProducto in cotizacionProductoListTmp)
						{
							decimal cantidad = cotizacionProducto.Cantidad - cotizacionProducto.CantidadComprobante;
							if(cantidad > decimal.Zero)
                     {
								MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(new MovimientoAlmacenSaldo()
								{
									PeriodoEmpresaSedeId = cotizacion.PeriodoEmpresaSedeId,
									AlmacenId = cotizacion.AlmacenId.Value,
									ProductoId = cotizacionProducto.ProductoId,
									MesContableId = movimientoAlmacenCierre.MesContableId
								});

								movimientoAlmacenSaldo.CantidadReserva -= cantidad;
								movimientoAlmacenSaldoRepository.ActualizarCantidadReserva(movimientoAlmacenSaldo);
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

        public decimal CortesGratisParcial(CotizacionDTO dto)
        {

            Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

			PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(cotizacion.PeriodoEmpresaSedeId);

            List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoBusiness.Search(new CotizacionProductoDTO()
            {
                CotizacionId = dto.CotizacionId,
                Estado = EstadoConstante.ACTIVO
            });
						
			List<CortesGratisDTO> cortesGratisDTOList = cortesGratisBusiness.Search(new CortesGratisDTO()
			{
				Estado = EstadoConstante.ACTIVO,
				EmpresaSedeId = periodoEmpresaSede.EmpresaSedeId
			});
            
            var PlanchasList = (from producto in cotizacionProductoDTOList
                                from corte in cortesGratisDTOList
                                where corte.ProductoId == producto.ProductoId
                                && producto.Estado == EstadoConstante.ACTIVO
                                select producto).ToList();

            
			var total =	PlanchasList.Sum(p => p.Cantidad) * 10;
			var parcial = PlanchasList.Sum(p => p.CantidadParcial) * 10;

			if(parcial > 0){
				return parcial;
			}

			return total;
        }

        public int GenerarOrdenProduccion(CotizacionDTO dto)
        {
            Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

            List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
            {
                CotizacionId = dto.CotizacionId,
                NullRegistroComboId = SiNoConstante.S,
                Estado = EstadoConstante.ACTIVO
            });

            if (cotizacionProductoList.Count == 0)
                throw new Exception(SR.GetString("EX_cotizacion_sin_items"));

            decimal montoMinimoAnticipo = decimal.Round(cotizacion.MontoTotal / 2, 2);
            decimal montoAnticipo = decimal.Zero;

            List<AnticipoCliente> anticipoClienteList = anticipoClienteRepository.Search(new AnticipoCliente()
            {
                CotizacionId = cotizacion.CotizacionId,
                TipoOperacionAnticipo = new TipoOperacionAnticipo() { Codigo = TipoOperacionAnticipoConstante.ANTICIPO },
                EstadoComprobante = new EstadoComprobante() { Codigo = EstadoComprobanteConstante.EMITIDO },
                Estado = EstadoConstante.ACTIVO
            });

            montoAnticipo = decimal.Round(anticipoClienteList.Sum(x => x.Monto), 2);

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
                                EmpresaId = dto.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId
                            }
                        }
                    },
                    CotizacionId = dto.CotizacionId,
                    TipoAutorizacion = new TipoAutorizacion() { Codigo = TipoAutorizacionConstante.NO_REQUERIR_ADELANTO },
                    EstadoAprobacion = new EstadoAprobacion() { Codigo = EstadoAprobacionConstante.APROBADO },
                    Estado = EstadoConstante.ACTIVO
                });

                if (cotizacionAutorizacion == null)
                {
                    if (cotizacion.EstadoCotizacionId != 3 && cotizacion.EstadoCotizacionId != 4)
                    {
                        throw new Exception(SR.GetString("EX_cotizacion_sin_anticipo"));
                    }
                }
            }

            EstadoOrdenProduccion estadoOrdenProduccion = estadoOrdenProduccionRepository.ObtenerPorCodigo(EstadoOrdenProduccionConstante.EN_PROCESO);
            RutaProduccion rutaProduccion = rutaProduccionRepository.SingleSearch(new RutaProduccion());
            List<RutaSubProcesoProduccion> rutaSubProcesoProduccionList = rutaSubProcesoProduccionRepository.Search(new RutaSubProcesoProduccion()
            {
                OrderBy = "rspp.orden",
                RutaProduccionId = rutaProduccion.EntityId
            });

            string incluyeInsumo = SiNoConstante.N;
            if (cotizacionProductoList.Where(x => x.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.EsInsumo == SiNoConstante.S).Count() > 0)
                incluyeInsumo = SiNoConstante.S;

            EstadoDespacho estadoDespacho = null;
            if (incluyeInsumo == SiNoConstante.S)
                estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(EstadoDespachoConstante.PENDIENTE);

            List<int> productoIdList = cotizacionProductoList.GroupBy(x => x.ProductoId).Select(x => x.Key).ToList();

            List<ProductoSubProcesoProduccion> productoSubProcesoProduccionList = productoSubProcesoProduccionRepository.Search(new ProductoSubProcesoProduccion()
            {
                ProductoIdList = productoIdList
            });

            EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository.
                ObtenerPorCodigo(EstadoOrdenProduccionSubProcesoConstante.PENDIENTE);

            try
            {
                if (cotizacion.EnviadoProduccion == SiNoConstante.S)
                {
                   int ID = actualizarOrdenProduccion(dto);
				   return ID;
                }

                int newId = 0;
                    using (TransactionScope tx = new())
                    {
                        OrdenProduccionDTO ordenProduccionDTONew = new()
                        {
                            PeriodoEmpresaSedeDTO = dto.PeriodoEmpresaSedeDTO,
                            PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeDTO.Id,
                            CotizacionId = cotizacion.EntityId,
                            AlmacenId = cotizacion.AlmacenId.Value,
                            Fecha = DateTime.Today,
                            FechaEntrega = dto.Fecha,
                            ProveedorId = cotizacion.ProveedorId,
                            Observacion = cotizacion.Observacion,
                            RutaProduccionId = rutaProduccion.EntityId,
                            EstadoOrdenProduccionId = estadoOrdenProduccion.EntityId,
                            IncluyeInsumo = incluyeInsumo,
                            EstadoDespachoId = estadoDespacho?.EntityId,
                            ObservacionDespacho = string.Empty,
                            UsuarioCreacionId = dto.UsuarioCreacionId
                        };

                        newId = ordenProduccionBusiness.Insert(ordenProduccionDTONew);

                        CotizacionProductoDTO cotizacionProductoDTOCG = dto.CotizacionProductoDTOList
                        .SingleOrDefault(x => x.CotizacionProductoId == 0 && x.Cantidad > 0 && x.ProductoId == 3083);

                        if (cotizacionProductoDTOCG != null)
                        {
                            OrdenProduccionProductoDTO ordenProduccionProductoDTO1 = new()
                            {
                                OrdenProduccionId = newId,
                                ProductoId = 3083,
                                Descripcion = string.Empty,
                                Cantidad = cotizacionProductoDTOCG.Cantidad,
                                EntregadoA = string.Empty,
                                RegistroReferencialId = null,
                                Estado = EstadoConstante.ACTIVO,
                                UsuarioCreacionId = dto.UsuarioCreacionId
                            };
                            ordenProduccionProductoBusiness.Insert(ordenProduccionProductoDTO1);
                        }

                        CotizacionProductoDTO cotizacionProductoDTOAGLOMERADO = dto.CotizacionProductoDTOList
                        .SingleOrDefault(x => x.CotizacionProductoId == 0 && x.Cantidad > 0 && x.ProductoId == 3946);

                        if (cotizacionProductoDTOAGLOMERADO != null)
                        {
                            OrdenProduccionProductoDTO ordenProduccionProductoDTO1 = new()
                            {
                                OrdenProduccionId = newId,
                                ProductoId = cotizacionProductoDTOAGLOMERADO.ProductoId,
                                Descripcion = string.Empty,
                                Cantidad = cotizacionProductoDTOAGLOMERADO.Cantidad,
                                EntregadoA = string.Empty,
                                RegistroReferencialId = null,
                                Estado = EstadoConstante.ACTIVO,
                                UsuarioCreacionId = dto.UsuarioCreacionId
                            };
                            ordenProduccionProductoBusiness.Insert(ordenProduccionProductoDTO1);
                        }

                        foreach (CotizacionProducto cotizacionProducto in cotizacionProductoList)
                        {

                            CotizacionProductoDTO cotizacionProductoDTO = dto.CotizacionProductoDTOList.
                            Where(x => x.CotizacionProductoId == cotizacionProducto.EntityId).SingleOrDefault();

                            OrdenProduccionProductoDTO ordenProduccionProductoDTO = new()
                            {
                                OrdenProduccionId = newId,
                                ProductoId = cotizacionProducto.ProductoId,
                                Descripcion = cotizacionProducto.Descripcion,
                                Cantidad = cotizacionProductoDTO != null ? cotizacionProductoDTO.Cantidad : cotizacionProducto.Cantidad,
                                EntregadoA = string.Empty,
                                RegistroReferencialId = cotizacionProducto.CotizacionProductoId,
                                Estado = EstadoConstante.ACTIVO,
                                UsuarioCreacionId = dto.UsuarioCreacionId
                            };

                            ordenProduccionProductoBusiness.Insert(ordenProduccionProductoDTO);

							CotizacionProductoDTO cotizacionProductoSaldoPendiente = new()
							{
								CotizacionProductoId = cotizacionProducto.CotizacionProductoId,
								CantidadParcial = cotizacionProducto.Cantidad
							};
							cotizacionProductoBusiness.ActualizarSaldoProduccion(cotizacionProductoSaldoPendiente);
						}

                        OrdenProduccionProductoDTO ordenProduccionProductoDTO3 = new()
                        {
                            OrdenProduccionId = newId,
                            Estado = EstadoConstante.ACTIVO
                        };
                        ordenProduccionProductoBusiness.GenerarSubProcesos(ordenProduccionProductoDTO3);

                        cotizacionRepository.ActualizarEnviadoProduccion(new Cotizacion()
                        {
                            CotizacionId = cotizacion.EntityId,
                            EnviadoProduccion = SiNoConstante.S,
                            UsuarioCreacionId = dto.UsuarioCreacionId
                        });

                    tx.Complete();

                    }
                    return newId;
            }
            catch (Exception)
            {
                throw;
            }
        }

		private int actualizarOrdenProduccion(CotizacionDTO dto)
		{

			List<CotizacionProductoDTO> modelCP = dto.CotizacionProductoDTOList;

            List<OrdenProduccion> ordenProduccionList = ordenProduccionRepository.Search(new OrdenProduccion() { CotizacionId = dto.CotizacionId });
             int ID = ordenProduccionList.First().OrdenProduccionId;

             List<CotizacionProducto> cpList = cotizacionProductoRepository.Search(new CotizacionProducto() { CotizacionId = dto.CotizacionId });
             List<CotizacionProductoDTO> cpListDTO = Mapper.Map<List<CotizacionProductoDTO>>(cpList);

             List<OrdenProduccionProducto> oppList = ordenProduccionProductoRepository.Search(new OrdenProduccionProducto() { OrdenProduccionId = ID });

             var NewProds = cpList.Where(ProdC => !oppList.Any(ProdOP => ProdC.CotizacionProductoId == ProdOP.RegistroReferencialId) && ProdC.Estado == EstadoConstante.ACTIVO).ToList();
             List<CotizacionProductoDTO> NewProdsDTO = Mapper.Map<List<CotizacionProductoDTO>>(NewProds);

             using (TransactionScope tx = new())
             {

                 if (NewProdsDTO.Any())
                 {
                     foreach (CotizacionProducto cotizacionProducto in NewProds)
                     {

                         CotizacionProductoDTO cotizacionProductoDTO = NewProdsDTO.
                         Where(x => x.CotizacionProductoId == cotizacionProducto.CotizacionProductoId).SingleOrDefault();

                         OrdenProduccionProductoDTO ordenProduccionProductoDTO = new()
                         {
                             OrdenProduccionId = ID,
                             ProductoId = cotizacionProducto.ProductoId,
                             Descripcion = cotizacionProducto.Descripcion,
                             Cantidad = cotizacionProductoDTO != null ? cotizacionProductoDTO.Cantidad : cotizacionProducto.Cantidad,
                             EntregadoA = string.Empty,
                             RegistroReferencialId = cotizacionProducto.CotizacionProductoId,
                             Estado = EstadoConstante.ACTIVO,
                             UsuarioCreacionId = dto.UsuarioCreacionId
                         };

                         ordenProduccionProductoBusiness.Insert(ordenProduccionProductoDTO);
                     }
                 }

                 foreach (CotizacionProducto cotizacionProducto in cpList)
                 {
                     CotizacionProductoDTO cotizacionProductoDTO = cpListDTO.
                     Where(x => x.CotizacionProductoId == cotizacionProducto.CotizacionProductoId).SingleOrDefault();

                     OrdenProduccionProductoDTO ordenProduccionProductoDTO = new()
                     {
                         RegistroReferencialId = cotizacionProducto.CotizacionProductoId,
                         UsuarioCreacionId = dto.UsuarioCreacionId,
                     };

                     ordenProduccionProductoBusiness.actualizarEnvioProd(ordenProduccionProductoDTO);
                 }

				 foreach(CotizacionProducto cotizaProd in cpList)
				 {
                    CotizacionProductoDTO cotizacionProductoSaldoPendiente = new()
                    {
                        CotizacionProductoId = cotizaProd.CotizacionProductoId,
                        CantidadParcial = cotizaProd.Cantidad
                    };
                    cotizacionProductoBusiness.ActualizarSaldoProduccion(cotizacionProductoSaldoPendiente);
                 }

                 tx.Complete();
             }

            // ACTUALIZAR CORTES LINEALES

            CotizacionProductoDTO corteLinealMel = modelCP.Where(x => x.ProductoId == 3083).FirstOrDefault();
            CotizacionProductoDTO corteLinealAglo = modelCP.Where(x => x.ProductoId == 3946).FirstOrDefault();

			List<OrdenProduccionProducto> clopList = ordenProduccionProductoRepository.Search(new OrdenProduccionProducto() { OrdenProduccionId = ID });

             var MemoriaCLOP = (clopList.Where(x => x.ProductoId == 3083 && x.RegistroReferencialId is null && x.Estado == EstadoConstante.ACTIVO));
             OrdenProduccionProductoDTO clopListDTO = Mapper.Map<OrdenProduccionProductoDTO>(MemoriaCLOP);

            OrdenProduccionProducto clAgloOP = Mapper.Map<OrdenProduccionProducto>((clopList.Where(x => x.ProductoId == 3946 && x.RegistroReferencialId == null && x.Estado == EstadoConstante.ACTIVO)).FirstOrDefault());

			using (TransactionScope tx = new())
			{	
				
				if (corteLinealMel.CotizacionProductoId != 0)
				{				
					if (MemoriaCLOP.Any())
					{
						OrdenProduccionProductoDTO oppDTO = new() { OrdenProduccionProductoId = MemoriaCLOP.First().EntityId, UsuarioCreacionId = dto.UsuarioCreacionId };
						ordenProduccionProductoBusiness.Delete(oppDTO);
					}

					OrdenProduccionProductoDTO ordenProduccionProductoDTO = new()
					{
						RegistroReferencialId = corteLinealMel.CotizacionProductoId,
						Descripcion = corteLinealMel.Descripcion,
						Cantidad = corteLinealMel.Cantidad,
						Estado = EstadoConstante.ACTIVO,
						UsuarioCreacionId = dto.UsuarioCreacionId
					};
					ordenProduccionProductoBusiness.UpdateOrden(ordenProduccionProductoDTO);
				}

				if (corteLinealMel.CotizacionProductoId == 0 && corteLinealMel.Cantidad > 0)
				{

					if (!MemoriaCLOP.Any())
					{
						OrdenProduccionProductoDTO OPPINSERTDTO = new()
						{
							OrdenProduccionId = ID,
							ProductoId = 3083,
							Descripcion = string.Empty,
							Cantidad = corteLinealMel.Cantidad,
							EntregadoA = string.Empty,
							RegistroReferencialId = null,
							Estado = EstadoConstante.ACTIVO,
							UsuarioCreacionId = dto.UsuarioCreacionId
						};
						ordenProduccionProductoBusiness.Insert(OPPINSERTDTO);
					}

					if (MemoriaCLOP.Any())
					{
						OrdenProduccionProductoDTO ordenProduccionProductoDTO1 = new()
						{
							OrdenProduccionProductoId = MemoriaCLOP.FirstOrDefault().OrdenProduccionProductoId,
							Descripcion = string.Empty,
							Cantidad = corteLinealMel.Cantidad,
							UsuarioCreacionId = dto.UsuarioCreacionId
						};
						ordenProduccionProductoBusiness.Update(ordenProduccionProductoDTO1);
					}
				}
				// Fin CorteLinealMelamina 
				
				if(dto.PeriodoEmpresaSedeDTO.EmpresaSedeId == 2) {

						if (corteLinealAglo.CotizacionProductoId != 0 && corteLinealAglo.Cantidad > 0)
                        {

                            if (clAgloOP.RegistroReferencialId == null)
                            {
                                ordenProduccionProductoBusiness.Delete(new OrdenProduccionProductoDTO()
								{
									OrdenProduccionProductoId = clAgloOP.OrdenProduccionProductoId,
									UsuarioCreacionId = dto.UsuarioCreacionId
                                });
                            }

							ordenProduccionProductoBusiness.UpdateOrden(new OrdenProduccionProductoDTO()
							{
                                RegistroReferencialId = corteLinealAglo.CotizacionProductoId,
                                Descripcion = corteLinealAglo.Descripcion,
                                Cantidad = corteLinealAglo.Cantidad,
                                Estado = EstadoConstante.ACTIVO,
                                UsuarioCreacionId = dto.UsuarioCreacionId
                            });
                        }

                        if (corteLinealAglo.CotizacionProductoId == 0 && corteLinealAglo.Cantidad > 0)
                        {

                            if (clAgloOP.OrdenProduccionProductoId == 0)
                            {
                                ordenProduccionProductoBusiness.Insert(new OrdenProduccionProductoDTO()
								{
                                    OrdenProduccionId = ID,
                                    ProductoId = 3946,
                                    Descripcion = string.Empty,
                                    Cantidad = corteLinealAglo.Cantidad,
                                    EntregadoA = string.Empty,
                                    RegistroReferencialId = null,
                                    Estado = EstadoConstante.ACTIVO,
                                    UsuarioCreacionId = dto.UsuarioCreacionId
                                });
                            }

                            if (MemoriaCLOP.Any())
                            {
                                ordenProduccionProductoBusiness.Update(new OrdenProduccionProductoDTO()
								{
									OrdenProduccionProductoId = clAgloOP.OrdenProduccionProductoId,
									Descripcion = string.Empty,
									Cantidad = corteLinealAglo.Cantidad,
									UsuarioCreacionId = dto.UsuarioCreacionId
                                });
                            }
                        }
                        //Fin CorteLinealAglomerado
                }

					cotizacionAutorizacionBusiness.UpdateAuto(new CotizacionAutorizacionDTO()
					{
						CotizacionId = dto.CotizacionId
					}); 
				                 
					ordenProduccionBusiness.ActualizarFecha(new OrdenProduccionDTO()
					{
						OrdenProduccionId = ID,
						Fecha = dto.Fecha
					});
				                 
					ordenProduccionBusiness.EliminarSubProcesos(new OrdenProduccionDTO()
					{
						OrdenProduccionId = ID
					});

					ordenProduccionProductoBusiness.GenerarSubProcesos(new OrdenProduccionProductoDTO()
					{
						OrdenProduccionId = ID,
						Estado = EstadoConstante.ACTIVO
					});
					
					ordenProduccionRepository.EliminarInsumo(ID);

                 tx.Complete();
             }

			return ID;
        }
        public int GenerarOrdenProduccionParcial(CotizacionDTO dto)
        {
            Cotizacion cotizacion = cotizacionRepository.GetById(dto.CotizacionId);

			List<CotizacionProductoDTO> ProductosPedidoDTOList = cotizacionProductoBusiness.Search(new CotizacionProductoDTO()
			{
				CotizacionId = dto.CotizacionId,
				Estado = EstadoConstante.ACTIVO
			});
			            
			List<CotizacionProductoDTO> cotizacionProductoDTOList = dto.CotizacionProductoDTOList.Where(x => x.CantidadParcial > 0).ToList();
            List<CotizacionProducto> OrdenProParcial = Mapper.Map<List<CotizacionProducto>>(cotizacionProductoDTOList);

            if (OrdenProParcial.Count == 0)
                throw new Exception(SR.GetString("EX_cotizacion_sin_items"));

			List<CotizacionProductoDTO> cotizaProdsParcialDTO = new List<CotizacionProductoDTO>();

            cotizaProdsParcialDTO = (from productosCotiza in ProductosPedidoDTOList
								  from productosParcial in cotizacionProductoDTOList
								  where productosCotiza.CotizacionProductoId == productosParcial.CotizacionProductoId
								  select productosCotiza).ToList();

			List<CotizacionProducto> cotizaProdsParcial = Mapper.Map<List<CotizacionProducto>>(cotizaProdsParcialDTO);

            decimal montoMinimoAnticipo = decimal.Round(cotizacion.MontoTotal / 2, 2);
            decimal montoAnticipo = decimal.Zero;

            List<AnticipoCliente> anticipoClienteList = anticipoClienteRepository.Search(new AnticipoCliente()
            {
                CotizacionId = cotizacion.CotizacionId,
                TipoOperacionAnticipo = new TipoOperacionAnticipo() { Codigo = TipoOperacionAnticipoConstante.ANTICIPO },
                EstadoComprobante = new EstadoComprobante() { Codigo = EstadoComprobanteConstante.EMITIDO },
                Estado = EstadoConstante.ACTIVO
            });

            montoAnticipo = decimal.Round(anticipoClienteList.Sum(x => x.Monto), 2);

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
                                EmpresaId = dto.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId
                            }
                        }
                    },
                    CotizacionId = dto.CotizacionId,
                    TipoAutorizacion = new TipoAutorizacion() { Codigo = TipoAutorizacionConstante.NO_REQUERIR_ADELANTO },
                    EstadoAprobacion = new EstadoAprobacion() { Codigo = EstadoAprobacionConstante.APROBADO },
                    Estado = EstadoConstante.ACTIVO
                });

                if (cotizacionAutorizacion == null)
                {
                    if (cotizacion.EstadoCotizacionId != 3 && cotizacion.EstadoCotizacionId != 4)
                    {
                        throw new Exception(SR.GetString("EX_cotizacion_sin_anticipo"));
                    }
                }
            }

            EstadoOrdenProduccion estadoOrdenProduccion = estadoOrdenProduccionRepository.ObtenerPorCodigo(EstadoOrdenProduccionConstante.EN_PROCESO);
            RutaProduccion rutaProduccion = rutaProduccionRepository.SingleSearch(new RutaProduccion());
            List<RutaSubProcesoProduccion> rutaSubProcesoProduccionList = rutaSubProcesoProduccionRepository.Search(new RutaSubProcesoProduccion()
            {
                OrderBy = "rspp.orden",
                RutaProduccionId = rutaProduccion.EntityId
            });

            string incluyeInsumo = SiNoConstante.N;
            if (cotizaProdsParcial.Where(x => x.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.EsInsumo == SiNoConstante.S).Count() > 0)
                incluyeInsumo = SiNoConstante.S;

            EstadoDespacho estadoDespacho = null;
            if (incluyeInsumo == SiNoConstante.S)
                estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(EstadoDespachoConstante.PENDIENTE);

            List<int> productoIdList = cotizaProdsParcial.GroupBy(x => x.ProductoId).Select(x => x.Key).ToList();

            List<ProductoSubProcesoProduccion> productoSubProcesoProduccionList = productoSubProcesoProduccionRepository.Search(new ProductoSubProcesoProduccion()
            {
                ProductoIdList = productoIdList
            });

            EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository.
                ObtenerPorCodigo(EstadoOrdenProduccionSubProcesoConstante.PENDIENTE);

            try
            {
                
				int newId = 0;
                using (TransactionScope tx = new())
                {
					OrdenProduccionDTO ordenProduccionDTONew = new()
                    {
						PeriodoEmpresaSedeDTO = dto.PeriodoEmpresaSedeDTO,
                        PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeDTO.Id,
                        CotizacionId = cotizacion.EntityId,
                        AlmacenId = cotizacion.AlmacenId.Value,
                        Fecha = DateTime.Today,
                        FechaEntrega = dto.Fecha,
                        ProveedorId = cotizacion.ProveedorId,
                        Observacion = cotizacion.Observacion,
                        RutaProduccionId = rutaProduccion.EntityId,
                        EstadoOrdenProduccionId = estadoOrdenProduccion.EntityId,
                        IncluyeInsumo = incluyeInsumo,
                        EstadoDespachoId = estadoDespacho?.EntityId,
                        ObservacionDespacho = string.Empty,
                        UsuarioCreacionId = dto.UsuarioCreacionId
                    };

                    newId = ordenProduccionBusiness.Insert(ordenProduccionDTONew);


                    foreach (CotizacionProducto cotizacionProducto in cotizaProdsParcial)
                    {

						CotizacionProductoDTO cotizacionProductoDTO = dto.CotizacionProductoDTOList.
                        Where(x => x.CotizacionProductoId == cotizacionProducto.CotizacionProductoId).SingleOrDefault();

						if(cotizacionProducto.Cantidad < (cotizacionProducto.CantidadParcial + cotizacionProductoDTO.CantidadParcial))
						throw new Exception(SR.GetString("EX_cotizacion_producto_parcial"));	

                        OrdenProduccionProductoDTO ordenProduccionProductoDTO = new()
                        {
							OrdenProduccionId = newId,
                            ProductoId = cotizacionProducto.ProductoId,
                            Descripcion = cotizacionProducto.Descripcion,
                            Cantidad = cotizacionProductoDTO.CantidadParcial + cotizacionProductoDTO.CortesGratis,
                            EntregadoA = string.Empty,
                            RegistroReferencialId = cotizacionProducto.CotizacionProductoId,
                            Estado = EstadoConstante.ACTIVO,
                            UsuarioCreacionId = dto.UsuarioCreacionId
                        };
                        ordenProduccionProductoBusiness.Insert(ordenProduccionProductoDTO);

                        decimal cantidadRestante = cotizacionProducto.CantidadParcial + cotizacionProductoDTO.CantidadParcial;

                        CotizacionProductoDTO cotizacionProductoSaldoPendiente = new()
						{
							CotizacionProductoId = cotizacionProducto.CotizacionProductoId,
							CantidadParcial = cantidadRestante
                        };
						cotizacionProductoBusiness.ActualizarSaldoProduccion(cotizacionProductoSaldoPendiente);
                    }

                    CotizacionProductoDTO cotizacionProductoDTOCG = dto.CotizacionProductoDTOList
                        .SingleOrDefault(x => x.CotizacionProductoId == 0 && x.CortesGratis > 0 && x.ProductoId == 3083);

                    if (cotizacionProductoDTOCG != null)
                    {
                        OrdenProduccionProductoDTO ordenProduccionProductoDTO1 = new()
                        {
                            OrdenProduccionId = newId,
                            ProductoId = 3083,
                            Descripcion = string.Empty,
                            Cantidad = cotizacionProductoDTOCG.CortesGratis,
                            EntregadoA = string.Empty,
                            RegistroReferencialId = null,
                            Estado = EstadoConstante.ACTIVO,
                            UsuarioCreacionId = dto.UsuarioCreacionId
                        };
                        ordenProduccionProductoBusiness.Insert(ordenProduccionProductoDTO1);
                    }

					if (cotizacion.PeriodoEmpresaSede.EmpresaSede.EmpresaSedeId == 2) { 

						CotizacionProductoDTO cotizacionProductoDTOAGLOMERADO = dto.CotizacionProductoDTOList
						.SingleOrDefault(x => x.CotizacionProductoId == 0 && x.CortesGratis > 0 && x.ProductoId == 3946);

						if (cotizacionProductoDTOAGLOMERADO != null)
						{
							OrdenProduccionProductoDTO ordenProduccionProductoDTO1 = new()
							{
								OrdenProduccionId = newId,
								ProductoId = cotizacionProductoDTOAGLOMERADO.ProductoId,
								Descripcion = string.Empty,
								Cantidad = cotizacionProductoDTOAGLOMERADO.CortesGratis,
								EntregadoA = string.Empty,
								RegistroReferencialId = null,
								Estado = EstadoConstante.ACTIVO,
								UsuarioCreacionId = dto.UsuarioCreacionId
							};
							ordenProduccionProductoBusiness.Insert(ordenProduccionProductoDTO1);
						}
                    }

                    OrdenProduccionProductoDTO ordenProduccionProductoDTO3 = new()
                    {
						OrdenProduccionId = newId,
                        Estado = EstadoConstante.ACTIVO
                    };
                    ordenProduccionProductoBusiness.GenerarSubProcesos(ordenProduccionProductoDTO3);

                    cotizacionRepository.ActualizarEnviadoProduccion(new Cotizacion()
                    {
						CotizacionId = cotizacion.EntityId,
                        EnviadoProduccion = "P",
                        UsuarioCreacionId = dto.UsuarioCreacionId
                    });

					tx.Complete();
                }
                    return newId;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
