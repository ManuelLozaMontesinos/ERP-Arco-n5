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
	public class ComprobanteBusiness : IComprobanteBusiness
	{
		private readonly IComprobanteRepository comprobanteRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IMedioPagoRepository medioPagoRepository;
		private readonly IComprobanteProductoRepository comprobanteProductoRepository;
		private readonly ITipoDetraccionRepository tipoDetraccionRepository;
		private readonly IParametroSistemaRepository parametroSistemaRepository;
		private readonly IDocumentoComercialSerieBusiness documentoComercialSerieBusiness;
		private readonly IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository;
		private readonly IEstadoComprobanteRepository estadoComprobanteRepository;
		private readonly ITransaccionAlmacenRepository transaccionAlmacenRepository;
		private readonly IMesContableRepository mesContableRepository;
		private readonly IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository;
		private readonly IMovimientoAlmacenBusiness movimientoAlmacenBusiness;
		private readonly IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness;
		private readonly IEstadoSunatRepository estadoSunatRepository;
		private readonly ICotizacionRepository cotizacionRepository;
		private readonly ICotizacionProductoRepository cotizacionProductoRepository;
		private readonly IEstadoCotizacionRepository estadoCotizacionRepository;
		private readonly ICotizacionProductoBusiness cotizacionProductoBusiness;
		private readonly IAnticipoClienteRepository anticipoClienteRepository;
		private readonly ITipoOperacionAnticipoRepository tipoOperacionAnticipoRepository;
		private readonly IEstadoDespachoRepository estadoDespachoRepository;
		private readonly ITipoAccionAuditoriaRepository tipoAccionAuditoriaRepository;
		private readonly IComprobanteTrazaRepository comprobanteTrazaRepository;
		private readonly IComprobanteProductoEntregaRepository comprobanteProductoEntregaRepository;

		public ComprobanteBusiness(
			IComprobanteRepository comprobanteRepository,
			ITablaBusiness tablaBusiness,
			IMedioPagoRepository medioPagoRepository,
			IComprobanteProductoRepository comprobanteProductoRepository,
			ITipoDetraccionRepository tipoDetraccionRepository,
			IParametroSistemaRepository parametroSistemaRepository,
			IDocumentoComercialSerieBusiness documentoComercialSerieBusiness,
			IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository,
			IEstadoComprobanteRepository estadoComprobanteRepository,
			ITransaccionAlmacenRepository transaccionAlmacenRepository,
			IMesContableRepository mesContableRepository,
			IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository,
			IMovimientoAlmacenBusiness movimientoAlmacenBusiness,
			IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness,
			IEstadoSunatRepository estadoSunatRepository,
			ICotizacionRepository cotizacionRepository,
			ICotizacionProductoRepository cotizacionProductoRepository,
			IEstadoCotizacionRepository estadoCotizacionRepository,
			ICotizacionProductoBusiness cotizacionProductoBusiness,
			IAnticipoClienteRepository anticipoClienteRepository,
			ITipoOperacionAnticipoRepository tipoOperacionAnticipoRepository,
			IEstadoDespachoRepository estadoDespachoRepository,
			ITipoAccionAuditoriaRepository tipoAccionAuditoriaRepository,
			IComprobanteTrazaRepository comprobanteTrazaRepository,
			IComprobanteProductoEntregaRepository comprobanteProductoEntregaRepository
			)
		{
			this.comprobanteRepository = comprobanteRepository;
			this.tablaBusiness = tablaBusiness;
			this.medioPagoRepository = medioPagoRepository;
			this.comprobanteProductoRepository = comprobanteProductoRepository;
			this.tipoDetraccionRepository = tipoDetraccionRepository;
			this.parametroSistemaRepository = parametroSistemaRepository;
			this.documentoComercialSerieBusiness = documentoComercialSerieBusiness;
			this.periodoEmpresaSedeRepository = periodoEmpresaSedeRepository;
			this.estadoComprobanteRepository = estadoComprobanteRepository;
			this.transaccionAlmacenRepository = transaccionAlmacenRepository;
			this.mesContableRepository = mesContableRepository;
			this.estadoMovimientoAlmacenRepository = estadoMovimientoAlmacenRepository;
			this.movimientoAlmacenBusiness = movimientoAlmacenBusiness;
			this.movimientoAlmacenProductoBusiness = movimientoAlmacenProductoBusiness;
			this.estadoSunatRepository = estadoSunatRepository;
			this.cotizacionRepository = cotizacionRepository;
			this.cotizacionProductoRepository = cotizacionProductoRepository;
			this.estadoCotizacionRepository = estadoCotizacionRepository;
			this.cotizacionProductoBusiness = cotizacionProductoBusiness;
			this.anticipoClienteRepository = anticipoClienteRepository;
			this.tipoOperacionAnticipoRepository = tipoOperacionAnticipoRepository;
			this.estadoDespachoRepository = estadoDespachoRepository;
			this.tipoAccionAuditoriaRepository = tipoAccionAuditoriaRepository;
			this.comprobanteTrazaRepository = comprobanteTrazaRepository;
			this.comprobanteProductoEntregaRepository = comprobanteProductoEntregaRepository;
		}
		public ComprobanteDTO GetById(int id)
		{
			try
			{
				Comprobante comprobante = comprobanteRepository.GetById(id);

				ComprobanteDTO comprobanteDTO = Mapper.Map<ComprobanteDTO>(comprobante);

				return comprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteDTO> Search(ComprobanteDTO dto)
		{
			try
			{
				List<Comprobante> comprobanteList = comprobanteRepository.Search(Mapper.Map<Comprobante>(dto));

				List<ComprobanteDTO> comprobanteDTOList = Mapper.Map<List<ComprobanteDTO>>(comprobanteList);

				return comprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ComprobanteDTO SingleSearch(ComprobanteDTO dto)
		{
			try
			{
				Comprobante comprobante = comprobanteRepository.SingleSearch(Mapper.Map<Comprobante>(dto));

				ComprobanteDTO comprobanteDTO = Mapper.Map<ComprobanteDTO>(comprobante);

				return comprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ComprobanteDTO dto)
		{
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					comprobante.NumeroOperacion = obtenerNumeroOperacion(dto);

					newId = comprobanteRepository.Insert(comprobante);

					dto.ComprobanteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		private string obtenerNumeroOperacion(ComprobanteDTO dto)
      {
			TablaCorrelativoDTO tablaCorrelativoDTO = new()
         {
				Tabla = TablaConstante.COMPROBANTE_PAGO,
				PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
				{
					PeriodoId = dto.PeriodoEmpresaSedeDTO.PeriodoId,
					EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = dto.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId }
				},
				UsarSedeCentral = true,
				UsuarioAccionId = dto.UsuarioCreacionId
			};

			return string.Concat(DateTime.Today.Year.ToString(), tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO));
		}
		public int Update(ComprobanteDTO dto)
		{
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = comprobanteRepository.Update(comprobante);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ComprobanteDTO dto)
		{
			try
			{

				EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.ANULADO);

            List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
            {
               ComprobanteId = dto.ComprobanteId,
               NotNullRegistroReferencialId = SiNoConstante.S,
               Estado = EstadoConstante.ACTIVO
            });

				TipoAccionAuditoria tipoAccionAuditoria = tipoAccionAuditoriaRepository.ObtenerPorCodigo(TipoAccionAuditoriaConstante.ELIMINACION);

				using (TransactionScope tx = new())
				{
					Comprobante comprobante = comprobanteRepository.GetById(dto.ComprobanteId);
					comprobante.UsuarioCreacionId = dto.UsuarioCreacionId;
					comprobante.EstadoComprobanteId = estadoComprobante.EntityId;

					string estadoAnteriorComprobante = comprobante.EstadoComprobante.Codigo;

					comprobanteRepository.Delete(comprobante);

					comprobanteTrazaRepository.Insert(new ComprobanteTraza()
					{
						ComprobanteId = dto.ComprobanteId,
						TipoAccionAuditoriaId = tipoAccionAuditoria.EntityId,
						Fecha = DateTime.Now,
						Descripcion = dto.Observacion,
						UsuarioId = dto.UsuarioCreacionId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					MovimientoAlmacenDTO movimientoAlmacenDTO = movimientoAlmacenBusiness.SingleSearch(new MovimientoAlmacenDTO()
					{
						PeriodoEmpresaSedeId = comprobante.PeriodoEmpresaSedeId,
						ComprobanteId = comprobante.ComprobanteId,
						Estado = EstadoConstante.ACTIVO
					});

					comprobanteProductoRepository.EliminarPorComprobanteId(new ComprobanteProducto()
					{
						ComprobanteId = comprobante.ComprobanteId,
						UsuarioCreacionId = comprobante.UsuarioCreacionId
					});

					if (movimientoAlmacenDTO != null)
					{
						movimientoAlmacenBusiness.Delete(movimientoAlmacenDTO);
					}

					if (comprobante.CotizacionId.HasValue)
					{

						foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
						{
							cotizacionProductoRepository.ActualizarCantidadComprobante(new CotizacionProducto()
							{
								CotizacionId = comprobante.CotizacionId.Value,
								CotizacionProductoId = comprobanteProducto.RegistroReferencialId.Value
							});

							if (estadoAnteriorComprobante == EstadoComprobanteConstante.EMITIDO){

								if (comprobanteProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.EsCombo == SiNoConstante.S)
									cotizacionProductoBusiness.ActualizarReservaCombo(new CotizacionProductoDTO()
									{
										CotizacionProductoId = comprobanteProducto.RegistroReferencialId.Value,
										CotizacionId = comprobante.CotizacionId.Value
									});
								else
									cotizacionProductoBusiness.ActualizarReserva(new CotizacionProductoDTO()
									{
										CotizacionProductoId = comprobanteProducto.RegistroReferencialId.Value,
										Cantidad = comprobanteProducto.Cantidad
									});
							}
						}

						actualizarEstadoPedido(comprobante.CotizacionId.Value, comprobante.UsuarioCreacionId);
					}

					List<AnticipoCliente> anticipoClienteList = anticipoClienteRepository.Search(new AnticipoCliente() { 
						ComprobanteId = comprobante.ComprobanteId,
						TipoOperacionAnticipo = new() { Codigo = TipoOperacionAnticipoConstante.APLICACION},
						Estado = EstadoConstante.ACTIVO
					});

					if(anticipoClienteList.Count > 0){
						TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository
							.ObtenerPorCodigo(TipoOperacionAnticipoConstante.ANULACION);

						foreach(AnticipoCliente anticipoCliente in anticipoClienteList){	
							anticipoCliente.TipoOperacionAnticipoId = tipoOperacionAnticipo.EntityId;
							anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;

							anticipoClienteRepository.Delete(anticipoCliente);
						}

						List<int> idList = anticipoClienteList.Where( x => x.AnticipoClientePadreId.HasValue).Select(x => x.AnticipoClientePadreId.Value).ToList();
						foreach(int id in idList)
							anticipoClienteRepository.ActualizarMontoAplicado(new AnticipoCliente()
							{
								AnticipoClienteId = id,
								UsuarioCreacionId = dto.UsuarioCreacionId
							});

						AnticipoCliente anticipoClienteTmp = anticipoClienteList.FirstOrDefault();

						comprobanteRepository.ActualizarMontoAnticipo(new Comprobante()
						{
							ComprobanteId = anticipoClienteTmp.ComprobanteId.Value,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});

						comprobanteRepository.ActualizarMontoAnticipoDescuento(new Comprobante()
						{
							ComprobanteId = anticipoClienteTmp.ComprobanteId.Value,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});

						comprobanteRepository.ActualizarMonto(new Comprobante()
						{
							ComprobanteId = anticipoClienteTmp.ComprobanteId.Value,
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
		public void AplicarTasaPorMedioPago(ComprobanteDTO dto)
		{

			try
			{
				MedioPago medioPago = medioPagoRepository.GetById(dto.MedioPagoId);
				Comprobante comprobante = comprobanteRepository.GetById(dto.ComprobanteId);

				if (medioPago.MedioPagoId == comprobante.MedioPagoId)
					return;

				decimal porcentajeAnterior = CalculadorVenta.Round(comprobante.PorcentajeTarjeta / 100);
				decimal porcentajeNuevo = CalculadorVenta.Round(medioPago.PorcentajeTarjeta / 100);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
				{
					ComprobanteId = dto.ComprobanteId,
					NullRegistroComboId = SiNoConstante.S,
					Estado = EstadoConstante.ACTIVO
				});

				foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
				{

					CalculadorVentaItemDTO calculadorVentaItem = new()
               {
						TipoIGV = comprobanteProducto.TipoAfectacionIgv.TipoIgv.Codigo,
						Cantidad = comprobanteProducto.Cantidad,
						PrecioUnitario = CalculadorVenta.Round(comprobanteProducto.PrecioUnitario / (1 + porcentajeAnterior)),
						ImpuestoTasa = comprobante.ImpuestoTasa.Tasa,
						DetraccionTasa = comprobanteProducto.TipoDetraccion.Tasa,
						TipoDescuento = comprobanteProducto.TipoDescuento.Codigo,
						DescuentoValor = comprobanteProducto.DescuentoValor
					};

					calculadorVentaItem = CalculadorVenta.CalcularMontos(calculadorVentaItem);

					comprobanteProducto.ValorUnitario = calculadorVentaItem.ValorUnitario;
					comprobanteProducto.PrecioUnitario = calculadorVentaItem.PrecioUnitario;
					comprobanteProducto.MontoBruto = calculadorVentaItem.MontoBruto;
					comprobanteProducto.MontoDescuento1 = calculadorVentaItem.MontoDescuento1;
					comprobanteProducto.MontoNeto = calculadorVentaItem.MontoNeto;
					comprobanteProducto.MontoIgv = calculadorVentaItem.MontoImpuesto;
					comprobanteProducto.MontoTotal = calculadorVentaItem.MontoTotal;
					comprobanteProducto.MontoDetraccion = calculadorVentaItem.MontoDetraccion;
				}

				if (porcentajeNuevo > 0)
				{
					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
					{

						CalculadorVentaItemDTO calculadorVentaItem = new()
                  {
							TipoIGV = comprobanteProducto.TipoAfectacionIgv.TipoIgv.Codigo,
							Cantidad = comprobanteProducto.Cantidad,
							PrecioUnitario = CalculadorVenta.Round(comprobanteProducto.PrecioUnitario +
							(comprobanteProducto.PrecioUnitario * porcentajeNuevo)),
							ImpuestoTasa = comprobante.ImpuestoTasa.Tasa,
							DetraccionTasa = comprobanteProducto.TipoDetraccion.Tasa,
							DescuentoValor = comprobanteProducto.DescuentoValor
						};

						calculadorVentaItem = CalculadorVenta.CalcularMontos(calculadorVentaItem);

						comprobanteProducto.ValorUnitario = calculadorVentaItem.ValorUnitario;
						comprobanteProducto.PrecioUnitario = calculadorVentaItem.PrecioUnitario;
						comprobanteProducto.MontoBruto = calculadorVentaItem.MontoBruto;
						comprobanteProducto.MontoDescuento1 = calculadorVentaItem.MontoDescuento1;
						comprobanteProducto.MontoNeto = calculadorVentaItem.MontoNeto;
						comprobanteProducto.MontoIgv = calculadorVentaItem.MontoImpuesto;
						comprobanteProducto.MontoTotal = calculadorVentaItem.MontoTotal;
						comprobanteProducto.MontoDetraccion = calculadorVentaItem.MontoDetraccion;
					}
				}

				foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
					comprobanteProductoRepository.ActualizarMonto(comprobanteProducto);

				comprobante.MedioPagoId = medioPago.EntityId;
				comprobante.PorcentajeTarjeta = medioPago.PorcentajeTarjeta;

				comprobanteRepository.ActualizarMedioPago(comprobante);

				comprobanteRepository.ActualizarMonto(comprobante);
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Cerrar(ComprobanteDTO dto)
		{
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
				{
					ComprobanteId = dto.ComprobanteId,
					NullRegistroComboId = SiNoConstante.S,
					Estado = EstadoConstante.ACTIVO
				});

				if (comprobanteProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_comprobante_sin_items"));

				using (TransactionScope tx = new TransactionScope())
				{

					comprobante.UsuarioCreacionId = dto.UsuarioCreacionId;
					comprobanteRepository.Update(comprobante);

					comprobante = comprobanteRepository.GetById(dto.ComprobanteId);

					if (comprobante.EstadoComprobante.Codigo == EstadoComprobanteConstante.CERRADO)
						throw new Exception(SR.GetString("EX_comprobante_ya_esta_cerrado"));

					if (comprobante.EstadoComprobante.Codigo == EstadoComprobanteConstante.ANULADO)
						throw new Exception(SR.GetString("EX_comprobante_ya_esta_anulado"));

					if (comprobante.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
						throw new Exception(SR.GetString("EX_comprobante_ya_esta_emitido"));

					if (comprobante.TipoDocumentoComercial.ValidarProveedor == SiNoConstante.S)
					{
						switch (comprobante.TipoDocumentoComercial.Codigo)
						{
							case TipoDocumentoComercialConstante.FACTURA:
								if (string.IsNullOrEmpty(comprobante.Proveedor.NumeroRuc))
									throw new Exception(SR.GetString("EX_comprobante_ruc_invalido"));
								break;

							case TipoDocumentoComercialConstante.BOLETA_VENTA:
								if (comprobante.MontoTotal > comprobante.TipoDocumentoComercial.MontoMaximoSinProveedor)
								{
									if (comprobante.Proveedor.Codigo == ProveedorConstante.GENERICO)
										throw new Exception(SR.GetString("EX_documento_comercial_cliente_no_permitido"));

									if (string.IsNullOrEmpty(comprobante.Proveedor.NumeroDocumentoIdentidad))
										throw new Exception(SR.GetString("EX_comprobante_doc_identidad_invalido"));
								}
							break;
						}
					}

					EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.CERRADO);
					comprobante.EstadoComprobanteId = estadoComprobante.EntityId;

					comprobanteRepository.ActualizarEstadoComprobante(comprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Emitir(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
				{
					ComprobanteId = dto.ComprobanteId,
					NullRegistroComboId = SiNoConstante.S,
					Estado = EstadoConstante.ACTIVO
				});

				if (comprobanteProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_comprobante_sin_items"));

				int? cajaId = dto.CajaId.Value;

				comprobante = comprobanteRepository.GetById(dto.ComprobanteId);
				comprobante.UsuarioCreacionId = dto.UsuarioCreacionId;

				if (comprobante.EstadoComprobante.Codigo == EstadoComprobanteConstante.ANULADO)
					throw new Exception(SR.GetString("EX_comprobante_ya_esta_anulado"));

				if (comprobante.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
					throw new Exception(SR.GetString("EX_comprobante_ya_esta_emitido"));

				if (comprobante.TipoDocumentoComercial.ValidarProveedor == SiNoConstante.S)
				{
					switch (comprobante.TipoDocumentoComercial.Codigo)
					{
						case TipoDocumentoComercialConstante.FACTURA:
							if (string.IsNullOrEmpty(comprobante.Proveedor.NumeroRuc))
								throw new Exception(SR.GetString("EX_comprobante_ruc_invalido"));
							break;

						case TipoDocumentoComercialConstante.BOLETA_VENTA:
							if (comprobante.MontoTotal > comprobante.TipoDocumentoComercial.MontoMaximoSinProveedor)
							{
								if (comprobante.Proveedor.Codigo == ProveedorConstante.GENERICO)
									throw new Exception(SR.GetString("EX_documento_comercial_cliente_no_permitido"));

								if (string.IsNullOrEmpty(comprobante.Proveedor.NumeroDocumentoIdentidad))
									throw new Exception(SR.GetString("EX_comprobante_doc_identidad_invalido"));
							}
							break;
					}
				}

				int tipoDetraccionId = 0;
				decimal montoDetraccion = decimal.Zero;
				decimal porcentajeDetraccion = decimal.Zero;

				if (comprobante.TipoDocumentoComercial.Codigo != TipoDocumentoComercialConstante.FACTURA)
				{
					TipoDetraccion tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);
					tipoDetraccionId = tipoDetraccion.EntityId;
				}
				else
				{

					decimal montoMaximoServicios = decimal.Zero;
					decimal montoMaximoProductos = decimal.Zero;

					List<ParametroSistema> parametroSistemaList = parametroSistemaRepository.Search(new ParametroSistema()
					{
						OrderBy = "orden_grupo asc",
						GrupoParametroSistema = new GrupoParametroSistema() { Codigo = GrupoParametroSistemaConstante.VENTAS_DETRACCION }
					});

					ParametroSistema parametroSistema = parametroSistemaList.Where
						(x => x.Codigo == ParametroSistemaConstante.DETRACCION_MONTO_MAX_SERVICIOS).FirstOrDefault();
					montoMaximoServicios = parametroSistema == null ? decimal.Zero : parametroSistema.ValorNumerico;

					parametroSistema = parametroSistemaList.Where
											(x => x.Codigo == ParametroSistemaConstante.DETRACCION_MONTO_MAX_PRODUCTOS).FirstOrDefault();
					montoMaximoProductos = parametroSistema == null ? decimal.Zero : parametroSistema.ValorNumerico;

					TipoDetraccion tipoDetraccion = null;

               if (comprobante.MontoTotal <= montoMaximoServicios && comprobante.MontoTotal <= montoMaximoProductos)
					{
                  tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);
                  tipoDetraccionId = tipoDetraccion.EntityId;
                  montoDetraccion = decimal.Zero;
                  porcentajeDetraccion = decimal.Zero;
               }
					else
					{

                  List<string> tipoDetraccionCodigoList = comprobanteProductoList
                      .Where(x => x.TipoDetraccion != null &&
                                  x.TipoDetraccion.Codigo != TipoDetraccionConstante.SIN_DETRACCION)
                      .Select(x => x.TipoDetraccion.Codigo)
                      .Distinct().ToList();

                  if (tipoDetraccionCodigoList.Count == 0)
                  {
                     tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);
                     tipoDetraccionId = tipoDetraccion.EntityId;
                     montoDetraccion = decimal.Zero;
                     porcentajeDetraccion = decimal.Zero;
                  }
                  else
                  {
                     List<TipoDetraccion> tipoDetraccionList = tipoDetraccionRepository.Search(new TipoDetraccion()
                     {
                        TipoDetraccionList = tipoDetraccionCodigoList,
                        Estado = EstadoConstante.ACTIVO
                     });

                     tipoDetraccion = tipoDetraccionList.OrderByDescending(x => x.Tasa).FirstOrDefault();
                     tipoDetraccionId = tipoDetraccion.EntityId;
                     montoDetraccion = CalculadorVenta.CalcularDetraccion(comprobante.MontoTotal, tipoDetraccion.Tasa);
                     porcentajeDetraccion = tipoDetraccion.Tasa;
                  }
               }
				}

				PeriodoEmpresaSede periodoEmpresaSede =  periodoEmpresaSedeRepository.GetById(comprobante.PeriodoEmpresaSedeId);

				using (TransactionScope tx = new())
				{
					comprobante.Numero = documentoComercialSerieBusiness.ObtenerCorrelativo(new DocumentoComercialSerieDTO() { 
						EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId},
						TipoDocumentoComercialId = comprobante.TipoDocumentoComercialId,
						Serie = comprobante.Serie
					});

					comprobante.TipoDetraccionId = tipoDetraccionId;
					comprobante.PorcentajeDetraccion = porcentajeDetraccion;
					comprobante.MontoDetraccion = montoDetraccion;
					EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.EMITIDO);
					comprobante.Emitido = SiNoConstante.S;
					comprobante.EstadoComprobanteId = estadoComprobante.EntityId;
					comprobante.CajaId = cajaId;

					if (comprobante.TipoDocumentoComercial.EsInterno == SiNoConstante.N) {
						EstadoSunat estadoSunat = estadoSunatRepository.ObtenerPorCodigo(EstadoSunatConstante.PENDIENTE_GENERAR);
						comprobante.EstadoSunatId = estadoSunat.EntityId;
					}
					comprobanteRepository.ActualizarEmision(comprobante);

					comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
					{
						ComprobanteId = dto.ComprobanteId,
						Producto = new Producto() { FamiliaProducto = new FamiliaProducto() { ClaseProducto = new ClaseProducto()
                  {
							GrupoProducto = new GrupoProducto() { 
								TipoProducto = new TipoProducto() { Codigo = TipoProductoConstante.PRODUCTO },
								EsCombo = SiNoConstante.N
							}
                  }}},
						Estado = EstadoConstante.ACTIVO
					});

					crearSalidaAlmacen(periodoEmpresaSede, comprobante, comprobanteProductoList);

					if (comprobante.CotizacionId.HasValue)
						actualizarEstadoPedido(comprobante.CotizacionId.Value, comprobante.UsuarioCreacionId);

               tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void crearSalidaAlmacen(PeriodoEmpresaSede periodoEmpresaSede, Comprobante comprobante, 
			List<ComprobanteProducto> comprobanteProductoList)
      {

			if (comprobanteProductoList.Count == 0)
				return;

			TransaccionAlmacen transaccionAlmacen = transaccionAlmacenRepository.SingleSearch(new TransaccionAlmacen() { 
				EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId,
				Codigo = TransaccionAlmacenConstante.SALIDA_POR_VENTAS
			});

			MesContable mesContable = mesContableRepository.ObtenerPorCodigo(string.Format(StringFormatConstant.DATE_MONTH, comprobante.Fecha));

			EstadoMovimientoAlmacen estadoMovimientoAlmacen = estadoMovimientoAlmacenRepository.ObtenerPorCodigo(EstadoMovimientoAlmacenConstante.MOVIMIENTO);

			MovimientoAlmacenDTO movimientoAlmacenDTO = new MovimientoAlmacenDTO()
			{
				PeriodoEmpresaSedeId = comprobante.PeriodoEmpresaSedeId,
				AlmacenId = comprobante.AlmacenId.Value,
				TransaccionAlmacenId = transaccionAlmacen.EntityId,
				TipoMovimientoAlmacenId = transaccionAlmacen.TipoMovimientoAlmacenId.Value,
				ProveedorId = comprobante.ProveedorId,
				Fecha = comprobante.Fecha,
				MesContableId = mesContable.EntityId,
				NumeroOrdenCompra = string.Empty,
				NumeroGuiaRemision = string.Empty,
				ComprobanteId = comprobante.EntityId,
				TipoDocumentoComercialId = comprobante.TipoDocumentoComercialId,
				SerieDocumento = comprobante.Serie,
				NumeroDocumento = comprobante.Numero,
				TieneGuiaRemision = SiNoConstante.N,
				GuiaRemisionEnModificacion = SiNoConstante.N,
				AfectaSaldo = SiNoConstante.S,
				Observacion = comprobante.Observacion,
				EstadoMovimientoAlmacenId = estadoMovimientoAlmacen.EntityId,
				Estado = EstadoConstante.ACTIVO,
				UsuarioCreacionId = comprobante.UsuarioCreacionId
			};

			int newId = movimientoAlmacenBusiness.Insert(movimientoAlmacenDTO);

			foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList) {
				MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = new MovimientoAlmacenProductoDTO()
				{
					MovimientoAlmacenDTO = movimientoAlmacenDTO,
					MovimientoAlmacenId = newId,
					ProductoId = comprobanteProducto.ProductoId,
					Descripcion = comprobanteProducto.Descripcion,
					Cantidad = comprobanteProducto.Cantidad,
					RegistroReferencialId = comprobanteProducto.EntityId,
					UsuarioCreacionId = comprobante.UsuarioCreacionId
				};

				movimientoAlmacenProductoBusiness.AgregarDesdeReserva(movimientoAlmacenProductoDTO);
			}

		}
		private void actualizarEstadoPedido(int cotizacionId, int usuarioAccionId)
		{

			string codigo = string.Empty;

			List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto()
			{
				CotizacionId = cotizacionId,
				NullRegistroComboId = SiNoConstante.S,
				Estado = EstadoConstante.ACTIVO
			});

			int total = cotizacionProductoList.Count;
			int totalComprobante = cotizacionProductoList.Count(x => x.CantidadComprobante > decimal.Zero);

			if (totalComprobante == decimal.Zero)
				codigo = EstadoCotizacionConstante.PEDIDO;
			else
			{
				if (totalComprobante == total)
					codigo = EstadoCotizacionConstante.FACTURADO;
				else
					codigo = EstadoCotizacionConstante.FACTURADO_PARCIAL;
			}

			EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.ObtenerPorCodigo(codigo);

			cotizacionRepository.ActualizarEstado(new Cotizacion()
			{
				CotizacionId = cotizacionId,
				EstadoCotizacionId = estadoCotizacion.EntityId,
				UsuarioModificacionId = usuarioAccionId
			});
		}
      public void ActualizarEmisionElectronica(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteRepository.ActualizarEmisionElectronica(comprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<ComprobanteDTO> ListarDocumentoEmitido(ComprobanteDTO dto)
      {
			try
			{
				List<Comprobante> comprobanteList = comprobanteRepository.ListarDocumentoEmitido(Mapper.Map<Comprobante>(dto));

				List<ComprobanteDTO> comprobanteDTOList = Mapper.Map<List<ComprobanteDTO>>(comprobanteList);

				return comprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarEstadoSunat(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteRepository.ActualizarEstadoSunat(comprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteDTO> ListarResumenEstadoSunat(ComprobanteDTO dto)
		{
			try
			{
				List<Comprobante> comprobanteList = comprobanteRepository.ListarResumenEstadoSunat(Mapper.Map<Comprobante>(dto));

				List<ComprobanteDTO> comprobanteDTOList = Mapper.Map<List<ComprobanteDTO>>(comprobanteList);

				return comprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarPorComprobanteElectronicoIdList(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteRepository.ActualizarPorComprobanteElectronicoIdList(comprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void EmitirCanje(ComprobanteDTO dto)
		{
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
				{
					ComprobanteId = dto.ComprobanteId,
					Estado = EstadoConstante.ACTIVO
				});

				if (comprobanteProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_comprobante_sin_items"));

				comprobante = comprobanteRepository.GetById(dto.ComprobanteId);
				comprobante.UsuarioCreacionId = dto.UsuarioCreacionId;

				if (comprobante.EstadoComprobante.Codigo == EstadoComprobanteConstante.ANULADO)
					throw new Exception(SR.GetString("EX_comprobante_ya_esta_anulado"));

				if (comprobante.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
					throw new Exception(SR.GetString("EX_comprobante_ya_esta_emitido"));

				if (comprobante.TipoDocumentoComercial.ValidarProveedor == SiNoConstante.S)
				{
					switch (comprobante.TipoDocumentoComercial.Codigo)
					{
						case TipoDocumentoComercialConstante.FACTURA:
							if (string.IsNullOrEmpty(comprobante.Proveedor.NumeroRuc))
								throw new Exception(SR.GetString("EX_comprobante_ruc_invalido"));
							break;

						case TipoDocumentoComercialConstante.BOLETA_VENTA:
							if (comprobante.MontoTotal > comprobante.TipoDocumentoComercial.MontoMaximoSinProveedor)
							{
								if (comprobante.Proveedor.Codigo == ProveedorConstante.GENERICO)
									throw new Exception(SR.GetString("EX_documento_comercial_cliente_no_permitido"));

								if (string.IsNullOrEmpty(comprobante.Proveedor.NumeroDocumentoIdentidad))
									throw new Exception(SR.GetString("EX_comprobante_doc_identidad_invalido"));
							}
							break;
					}
				}

				int tipoDetraccionId = 0;
				decimal montoDetraccion = decimal.Zero;
				decimal porcentajeDetraccion = decimal.Zero;

				if (comprobante.TipoDocumentoComercial.Codigo != TipoDocumentoComercialConstante.FACTURA)
				{
					TipoDetraccion tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);
					tipoDetraccionId = tipoDetraccion.EntityId;
				}
				else
				{

					decimal montoMaximoServicios = decimal.Zero;
					decimal montoMaximoProductos = decimal.Zero;

					List<ParametroSistema> parametroSistemaList = parametroSistemaRepository.Search(new ParametroSistema()
					{
						OrderBy = "orden_grupo asc",
						GrupoParametroSistema = new GrupoParametroSistema() { Codigo = GrupoParametroSistemaConstante.VENTAS_DETRACCION }
					});

					ParametroSistema parametroSistema = parametroSistemaList.Where
						(x => x.Codigo == ParametroSistemaConstante.DETRACCION_MONTO_MAX_SERVICIOS).FirstOrDefault();
					montoMaximoServicios = parametroSistema == null ? decimal.Zero : parametroSistema.ValorNumerico;

					parametroSistema = parametroSistemaList.Where
											(x => x.Codigo == ParametroSistemaConstante.DETRACCION_MONTO_MAX_PRODUCTOS).FirstOrDefault();
					montoMaximoProductos = parametroSistema == null ? decimal.Zero : parametroSistema.ValorNumerico;

					TipoDetraccion tipoDetraccion = null;

					if (comprobanteProductoList.Where(x => x.Producto.FamiliaProducto.ClaseProducto.
						 GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.SERVICIO).ToList().Count > 0)
					{

						if (comprobante.MontoTotal >= montoMaximoServicios)
						{
							tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SERVICIOS);
							tipoDetraccionId = tipoDetraccion.EntityId;

							montoDetraccion = CalculadorVenta.CalcularDetraccion(comprobante.MontoTotal, tipoDetraccion.Tasa);
							porcentajeDetraccion = tipoDetraccion.Tasa;

						}
						else
						{
							tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);
							tipoDetraccionId = tipoDetraccion.EntityId;
						}
					}
					else
					{
						tipoDetraccionId = comprobanteProductoList.Max(x => x.TipoDetraccionId);
						montoDetraccion = comprobanteProductoList.Sum(x => x.MontoDetraccion);
						porcentajeDetraccion = comprobanteProductoList.Max(x => x.PorcentajeDetraccion);

						if (montoDetraccion > 0)
						{
							decimal montoTotal = comprobanteProductoList.Where(x => x.MontoDetraccion > 0).Sum(x => x.MontoTotal);

							if (montoTotal < montoMaximoProductos)
							{
								tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);
								tipoDetraccionId = tipoDetraccion.EntityId;

								montoDetraccion = decimal.Zero;
								porcentajeDetraccion = decimal.Zero;
							}
						}
						else
						{
							tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(TipoDetraccionConstante.SIN_DETRACCION);
							tipoDetraccionId = tipoDetraccion.EntityId;

							montoDetraccion = decimal.Zero;
							porcentajeDetraccion = decimal.Zero;
						}
					}
				}

				PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(comprobante.PeriodoEmpresaSedeId);

				using (TransactionScope tx = new())
				{
					comprobanteRepository.Update(Mapper.Map<Comprobante>(dto));

					comprobante.Numero = documentoComercialSerieBusiness.ObtenerCorrelativo(new DocumentoComercialSerieDTO()
					{
						EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId },
						TipoDocumentoComercialId = comprobante.TipoDocumentoComercialId,
						Serie = comprobante.Serie
					});

					comprobante.TipoDetraccionId = tipoDetraccionId;
					comprobante.PorcentajeDetraccion = porcentajeDetraccion;
					comprobante.MontoDetraccion = montoDetraccion;
					EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.EMITIDO);
					comprobante.Emitido = SiNoConstante.S;
					comprobante.EstadoComprobanteId = estadoComprobante.EntityId;

					if (comprobante.TipoDocumentoComercial.EsInterno == SiNoConstante.N)
					{
						EstadoSunat estadoSunat = estadoSunatRepository.ObtenerPorCodigo(EstadoSunatConstante.PENDIENTE_GENERAR);
						comprobante.EstadoSunatId = estadoSunat.EntityId;
					}
					comprobanteRepository.ActualizarEmision(comprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void EliminarCanje(ComprobanteDTO dto)
		{
			try
			{

				EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.ANULADO);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
				{
					ComprobanteId = dto.ComprobanteId,
					Estado = EstadoConstante.ACTIVO
				});

				using (TransactionScope tx = new TransactionScope())
				{
					Comprobante comprobante = comprobanteRepository.GetById(dto.ComprobanteId);
					comprobante.UsuarioCreacionId = dto.UsuarioCreacionId;
					comprobante.EstadoComprobanteId = estadoComprobante.EntityId;

					comprobanteRepository.Delete(comprobante);

					foreach(ComprobanteProducto comprobanteProducto in comprobanteProductoList)
               {
						comprobanteProductoRepository.Delete(comprobanteProducto);

						comprobanteProductoRepository.ActualizarNuloRegistroCanjeId(new ComprobanteProducto()
						{
							ComprobanteProductoId = comprobanteProducto.ComprobanteProductoId,
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
      public void ActualizarDetraccion(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteRepository.ActualizarDetraccion(comprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarTipoComprobante(ComprobanteDTO dto)
		{
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				using (TransactionScope tx = new())
				{
					comprobanteRepository.ActualizarTipoComprobante(comprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public int AgregarComprobanteAnticipo(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);
				List<ComprobanteProducto> comprobanteProductoList = Mapper.Map<List<ComprobanteProducto>>(dto.ComprobanteProductoDTOList);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					comprobante.NumeroOperacion = obtenerNumeroOperacion(dto);

					newId = comprobanteRepository.Insert(comprobante);

					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
               {
						comprobanteProducto.ComprobanteId = newId;
						comprobanteProductoRepository.Insert(comprobanteProducto);
					}

					dto.ComprobanteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public int ActualizarComprobanteAnticipo(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);
				List<ComprobanteProducto> comprobanteProductoList = Mapper.Map<List<ComprobanteProducto>>(dto.ComprobanteProductoDTOList);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = comprobanteRepository.Update(comprobante);
					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
						comprobanteProductoRepository.Update(comprobanteProducto);

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
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
      public void AnularComprobanteAnticipo(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);
				List<ComprobanteProducto> comprobanteProductoList = Mapper.Map<List<ComprobanteProducto>>(dto.ComprobanteProductoDTOList);

				EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.ANULADO);

				using (TransactionScope tx = new())
				{
					comprobante.UsuarioCreacionId = dto.UsuarioCreacionId;
					comprobante.EstadoComprobanteId = estadoComprobante.EntityId;
					comprobanteRepository.Delete(comprobante);

					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
						comprobanteProductoRepository.Delete(comprobanteProducto);

					tx.Complete();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<dynamic> ListarCobranza(ComprobanteDTO dto)
		{
			try
			{
				List<dynamic> dynamicList = comprobanteRepository.ListarCobranza(Mapper.Map<Comprobante>(dto));

				return dynamicList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteDTO> BuscarDespacho(ComprobanteDTO dto)
		{
			try
			{
				List<Comprobante> comprobanteList = comprobanteRepository.BuscarDespacho(Mapper.Map<Comprobante>(dto));

				List<ComprobanteDTO> comprobanteDTOList = Mapper.Map<List<ComprobanteDTO>>(comprobanteList);

				return comprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void CerrarDespacho(ComprobanteDTO dto)
		{
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
				{
					ComprobanteId = dto.ComprobanteId,
					NullRegistroComboId = SiNoConstante.S,
					Estado = EstadoConstante.ACTIVO
				});

				EstadoDespacho estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(EstadoDespachoConstante.ATENDIDO);
				DateTime fechaDespacho = DateTime.Now;

				comprobanteProductoList = comprobanteProductoList.Where(x => x.CantidadEntregada == decimal.Zero).ToList();

				using (TransactionScope tx = new())
				{

					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList) {
						comprobanteProductoEntregaRepository.Insert(new ComprobanteProductoEntrega()
						{
							ComprobanteProductoId = comprobanteProducto.EntityId,
							Cantidad = comprobanteProducto.CantidadPresentacion == decimal.Zero ?
														comprobanteProducto.Cantidad : comprobanteProducto.CantidadPresentacion,
							Fecha = comprobanteProducto.FechaDespacho.HasValue ? comprobanteProducto.FechaDespacho.Value : fechaDespacho,
							Observacion = comprobanteProducto.ObservacionDespacho,
							EntregaDespacho = dto.EntregaDespacho,
							PersonalId = comprobanteProducto.PersonalId == null ? dto.PersonalId.Value : comprobanteProducto.PersonalId.Value
						});

						comprobanteProductoRepository.ActualizarCantidadEntregada(new ComprobanteProducto() {
							ComprobanteProductoId = comprobanteProducto.EntityId,
						});
					}

					comprobanteRepository.ActualizarEstadoDespacho(new Comprobante() { 
						ComprobanteId = dto.ComprobanteId,
						FechaDespacho = fechaDespacho,
						PersonalId = dto.PersonalId,
						EstadoDespachoId = estadoDespacho.EntityId,
						ObservacionDespacho = dto.ObservacionDespacho,
						EntregaDespacho = dto.EntregaDespacho
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteDTO> BuscarPorEmpresaSede(ComprobanteDTO dto)
		{
			try
			{
				List<Comprobante> comprobanteList = comprobanteRepository.BuscarPorEmpresaSede(Mapper.Map<Comprobante>(dto));

				List<ComprobanteDTO> comprobanteDTOList = Mapper.Map<List<ComprobanteDTO>>(comprobanteList);

				return comprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarEstadoDespachoPorCantidadEntregada(ComprobanteDTO dto)
      {
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				using (TransactionScope tx = new())
				{
					comprobanteRepository.ActualizarEstadoDespachoPorCantidadEntregada(comprobante);

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
