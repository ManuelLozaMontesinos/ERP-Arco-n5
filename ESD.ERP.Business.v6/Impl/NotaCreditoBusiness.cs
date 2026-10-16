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
	public class NotaCreditoBusiness : INotaCreditoBusiness
	{
		private readonly INotaCreditoRepository notaCreditoRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly INotaCreditoProductoRepository notaCreditoProductoRepository;
		private readonly ITipoNotaCreditoRepository tipoNotaCreditoRepository;
		private readonly IComprobanteProductoRepository comprobanteProductoRepository;
		private readonly IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository;
		private readonly IDocumentoComercialSerieBusiness documentoComercialSerieBusiness;
		private readonly IEstadoComprobanteRepository estadoComprobanteRepository;
		private readonly IEstadoSunatRepository estadoSunatRepository;
		private readonly ITransaccionAlmacenRepository transaccionAlmacenRepository;
		private readonly IMesContableRepository mesContableRepository;
		private readonly IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository;
		private readonly IMovimientoAlmacenBusiness movimientoAlmacenBusiness;
		private readonly IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness;
		private readonly ITipoDocumentoComercialRepository tipoDocumentoComercialRepository;

		public NotaCreditoBusiness(
			INotaCreditoRepository notaCreditoRepository,
			ITablaBusiness tablaBusiness,
			INotaCreditoProductoRepository notaCreditoProductoRepository,
			ITipoNotaCreditoRepository tipoNotaCreditoRepository,
			IComprobanteProductoRepository comprobanteProductoRepository,
			IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository,
			IDocumentoComercialSerieBusiness documentoComercialSerieBusiness,
			IEstadoComprobanteRepository estadoComprobanteRepository,
			IEstadoSunatRepository estadoSunatRepository,
			ITransaccionAlmacenRepository transaccionAlmacenRepository,
			IMesContableRepository mesContableRepository,
			IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository,
			IMovimientoAlmacenBusiness movimientoAlmacenBusiness,
			IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness,
			ITipoDocumentoComercialRepository tipoDocumentoComercialRepository
			)
		{
			this.notaCreditoRepository = notaCreditoRepository;
			this.tablaBusiness = tablaBusiness;
			this.notaCreditoProductoRepository = notaCreditoProductoRepository;
			this.tipoNotaCreditoRepository = tipoNotaCreditoRepository;
			this.comprobanteProductoRepository = comprobanteProductoRepository;
			this.periodoEmpresaSedeRepository = periodoEmpresaSedeRepository;
			this.documentoComercialSerieBusiness = documentoComercialSerieBusiness;
			this.estadoComprobanteRepository = estadoComprobanteRepository;
			this.estadoSunatRepository = estadoSunatRepository;
			this.transaccionAlmacenRepository = transaccionAlmacenRepository;
			this.mesContableRepository = mesContableRepository;
			this.estadoMovimientoAlmacenRepository = estadoMovimientoAlmacenRepository;
			this.movimientoAlmacenBusiness = movimientoAlmacenBusiness;
			this.movimientoAlmacenProductoBusiness = movimientoAlmacenProductoBusiness;
			this.tipoDocumentoComercialRepository = tipoDocumentoComercialRepository;
		}
		public NotaCreditoDTO GetById(int id)
		{
			try
			{
				NotaCredito notaCredito = notaCreditoRepository.GetById(id);

				NotaCreditoDTO notaCreditoDTO = Mapper.Map<NotaCreditoDTO>(notaCredito);

				return notaCreditoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<NotaCreditoDTO> Search(NotaCreditoDTO dto)
		{
			try
			{
				List<NotaCredito> notaCreditoList = notaCreditoRepository.Search(Mapper.Map<NotaCredito>(dto));

				List<NotaCreditoDTO> notaCreditoDTOList = Mapper.Map<List<NotaCreditoDTO>>(notaCreditoList);

				return notaCreditoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public NotaCreditoDTO SingleSearch(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = notaCreditoRepository.SingleSearch(Mapper.Map<NotaCredito>(dto));

				NotaCreditoDTO notaCreditoDTO = Mapper.Map<NotaCreditoDTO>(notaCredito);

				return notaCreditoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = Mapper.Map<NotaCredito>(dto);

				List<NotaCredito> notaCreditoList = notaCreditoRepository.Search(new NotaCredito() { 
					PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
					ComprobanteId = dto.ComprobanteId,
					Estado = EstadoConstante.ACTIVO
				});

				TipoNotaCredito tipoNotaCredito = tipoNotaCreditoRepository.GetById(notaCredito.TipoNotaCreditoId);

				if (notaCreditoList.Count > 0)
            {
					List<string> tipoNotaCreditoList = new() { TipoNotaCreditoConstante.ANULACION_ERROR_RUC, TipoNotaCreditoConstante.ANULACION_OPERACION };
					if(tipoNotaCreditoList.Contains(tipoNotaCredito.Codigo))
						throw new Exception(SR.GetString("EX_nota_credito_anulacion_otras_asociadas"));
				}
					
				int newId = 0;
				using (TransactionScope tx = new())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new()
               {
						Tabla = TablaConstante.NOTA_CREDITO,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							PeriodoId = notaCredito.PeriodoEmpresaSede.PeriodoId,
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = notaCredito.PeriodoEmpresaSede.EmpresaSede.EmpresaId }
						},
						UsarSedeCentral = true,
						UsuarioAccionId = notaCredito.UsuarioCreacionId
					};

					notaCredito.NumeroOperacion = string.Concat(DateTime.Today.Year.ToString(), tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO));

					newId = notaCreditoRepository.Insert(notaCredito);

					if(tipoNotaCredito.PermiteEditItem == SiNoConstante.N)
               {
						List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto() { 
							ComprobanteId = notaCredito.ComprobanteId,
							Estado = EstadoConstante.ACTIVO
						});

						foreach(ComprobanteProducto comprobanteProducto in comprobanteProductoList)
                  {
							comprobanteProducto.CantidadNotaCredito = comprobanteProducto.Cantidad;
							NotaCreditoProducto notaCreditoProducto = new()
							{
								NotaCreditoId = newId,
								ComprobanteProductoId = comprobanteProducto.EntityId,
								Descripcion = comprobanteProducto.Descripcion,
								Cantidad = comprobanteProducto.Cantidad,
								PrecioUnitario = comprobanteProducto.PrecioUnitario,
								MontoBruto = comprobanteProducto.MontoBruto,
								MontoDescuento1 = comprobanteProducto.MontoDescuento1,
								MontoDescuento2 = comprobanteProducto.MontoDescuento2,
								MontoIgv = comprobanteProducto.MontoIgv,
								MontoNeto = comprobanteProducto.MontoNeto,
								MontoTotal = comprobanteProducto.MontoTotal,
								TipoAfectacionIgvId = comprobanteProducto.TipoAfectacionIgvId,
								Estado = EstadoConstante.ACTIVO,
								UsuarioCreacionId = dto.UsuarioCreacionId
							};

							notaCreditoProductoRepository.Insert(notaCreditoProducto);

							comprobanteProductoRepository.ActualizarCantidadNotaCredito(comprobanteProducto);
						}

						notaCreditoRepository.ActualizarMonto(new NotaCredito() { NotaCreditoId = newId, UsuarioCreacionId = dto.UsuarioCreacionId });
					}

					dto.NotaCreditoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = Mapper.Map<NotaCredito>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = notaCreditoRepository.Update(notaCredito);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = notaCredito = notaCreditoRepository.GetById(dto.Id); ;
				notaCredito.UsuarioCreacionId = dto.UsuarioCreacionId;

				using (TransactionScope tx = new TransactionScope())
				{
					EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.ANULADO);
					notaCredito.EstadoComprobanteId = estadoComprobante.EntityId;

					notaCreditoRepository.Delete(notaCredito);

					TipoNotaCredito tipoNotaCredito = tipoNotaCreditoRepository.GetById(notaCredito.TipoNotaCreditoId);

		
					List<NotaCreditoProducto> notaCreditoProductoList = notaCreditoProductoRepository.Search(new NotaCreditoProducto()
					{
						NotaCreditoId = notaCredito.EntityId,
						Estado = EstadoConstante.ACTIVO
					});

						foreach(NotaCreditoProducto notaCreditoProducto in notaCreditoProductoList)
							comprobanteProductoRepository.ActualizarCantidadNotaCredito(new ComprobanteProducto()
							{
								ComprobanteProductoId = notaCreditoProducto.ComprobanteProductoId.Value,
								CantidadNotaCredito = notaCreditoProducto.ComprobanteProducto.CantidadNotaCredito - notaCreditoProducto.Cantidad,
								UsuarioCreacionId = notaCredito.UsuarioCreacionId
							});

					if(notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
					{
						MovimientoAlmacenDTO movimientoAlmacenDTO = movimientoAlmacenBusiness.SingleSearch(new MovimientoAlmacenDTO() { 
							PeriodoEmpresaSedeId = notaCredito.PeriodoEmpresaSedeId,
							TransaccionAlmacenDTO = new TransaccionAlmacenDTO() { Codigo = TransaccionAlmacenConstante.DEVOLUCION_DE_VENTA},
							ComprobanteId = notaCredito.EntityId,
							Estado = EstadoConstante.ACTIVO
						});

						movimientoAlmacenDTO.UsuarioCreacionId = notaCredito.UsuarioCreacionId;

						movimientoAlmacenBusiness.Delete(movimientoAlmacenDTO);
					}
					

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
        private void NotaCreditoInterna(NotaCreditoDTO dto)
		{
			try
			{
                
				NotaCredito notaCredito = notaCreditoRepository.GetById(dto.NotaCreditoId);
                PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(notaCredito.PeriodoEmpresaSedeId);

                List<NotaCreditoProducto> notaCreditoProductoList = notaCreditoProductoRepository.Search(new NotaCreditoProducto()
				{
					NotaCreditoId = dto.NotaCreditoId,
					Estado = EstadoConstante.ACTIVO
				});
				                
                if (notaCredito.TipoNotaCredito.AfectaAlmacen == SiNoConstante.S)
                {
                    if (notaCreditoProductoList.Count == 0)
                        throw new Exception(SR.GetString("EX_nota_credito_sin_items"));

                    notaCredito = notaCreditoRepository.GetById(notaCredito.EntityId);
                    notaCredito.UsuarioCreacionId = dto.UsuarioCreacionId;

                    crearIngresoAlmacen(periodoEmpresaSede, notaCredito, notaCreditoProductoList.Where(x => x.ComprobanteProducto.Producto.FamiliaProducto.ClaseProducto.
                      GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.PRODUCTO).ToList());
	            }
            }
            catch (Exception)
			{
				throw;
			}
		}
        public void Emitir(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = notaCreditoRepository.GetById(dto.NotaCreditoId);

				List<NotaCreditoProducto> notaCreditoProductoList = notaCreditoProductoRepository.Search(new NotaCreditoProducto()
				{
					NotaCreditoId = dto.NotaCreditoId,
					Estado = EstadoConstante.ACTIVO
				});

				if(notaCredito.TipoNotaCredito.AfectaAlmacen == SiNoConstante.S)
				{
					if (notaCreditoProductoList.Count == 0)
						throw new Exception(SR.GetString("EX_nota_credito_sin_items"));
				}

				if (notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.ANULADO)
					throw new Exception(SR.GetString("EX_nota_credito_ya_esta_anulada"));

				if (notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
					throw new Exception(SR.GetString("EX_nota_credito_ya_esta_emitida"));

				PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(notaCredito.PeriodoEmpresaSedeId);

				using (TransactionScope tx = new())
				{
					notaCredito.Numero = documentoComercialSerieBusiness.ObtenerCorrelativo(new DocumentoComercialSerieDTO()
					{
						EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId },
						TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() { Codigo = TipoDocumentoComercialConstante.NOTA_CREDITO},
						Serie = notaCredito.Serie
					});

					EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.EMITIDO);
					notaCredito.Emitido = SiNoConstante.S;
					notaCredito.EstadoComprobanteId = estadoComprobante.EntityId;

					EstadoSunat estadoSunat = estadoSunatRepository.ObtenerPorCodigo(EstadoSunatConstante.PENDIENTE_GENERAR);
					notaCredito.EstadoSunatId = estadoSunat.EntityId;

					notaCreditoRepository.ActualizarEmision(notaCredito);

					if (notaCredito.TipoNotaCredito.AfectaAlmacen == SiNoConstante.S)
					{
						notaCredito = notaCreditoRepository.GetById(notaCredito.EntityId);
						notaCredito.UsuarioCreacionId = dto.UsuarioCreacionId;

						crearIngresoAlmacen(periodoEmpresaSede, notaCredito, notaCreditoProductoList.Where(x => x.ComprobanteProducto.Producto.FamiliaProducto.ClaseProducto.
						  GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.PRODUCTO).ToList());
					}

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void EmitirInterno(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = notaCreditoRepository.GetById(dto.NotaCreditoId);

				List<NotaCreditoProducto> notaCreditoProductoList = notaCreditoProductoRepository.Search(new NotaCreditoProducto()
				{
					NotaCreditoId = dto.NotaCreditoId,
					Estado = EstadoConstante.ACTIVO
				});

				if (notaCredito.TipoNotaCredito.AfectaAlmacen == SiNoConstante.S)
				{
					if (notaCreditoProductoList.Count == 0)
						throw new Exception(SR.GetString("EX_nota_credito_sin_items"));
				}

				if (notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.ANULADO)
					throw new Exception(SR.GetString("EX_nota_credito_ya_esta_anulada"));

				if (notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
					throw new Exception(SR.GetString("EX_nota_credito_ya_esta_emitida"));

				PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(notaCredito.PeriodoEmpresaSedeId);

				using (TransactionScope tx = new())
				{

                    notaCredito.Numero = documentoComercialSerieBusiness.ObtenerCorrelativo(new DocumentoComercialSerieDTO()
                    {
                        EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId },
                        TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() { Codigo = TipoDocumentoComercialConstante.NOTA_CREDITO },
                        Serie = notaCredito.Serie
                    });

                    EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.EMITIDO);
                    notaCredito.Emitido = SiNoConstante.S;
                    notaCredito.EstadoComprobanteId = estadoComprobante.EntityId;

                    notaCreditoRepository.ActualizarEmision(notaCredito);

                    if (notaCredito.TipoNotaCredito.AfectaAlmacen == SiNoConstante.S)
                    {
                        notaCredito = notaCreditoRepository.GetById(notaCredito.EntityId);
                        notaCredito.UsuarioCreacionId = dto.UsuarioCreacionId;

                        crearIngresoAlmacen(periodoEmpresaSede, notaCredito, notaCreditoProductoList.Where(x => x.ComprobanteProducto.Producto.FamiliaProducto.ClaseProducto.
                          GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.PRODUCTO).ToList());
                    }

                    tx.Complete();
                }
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void crearIngresoAlmacen(PeriodoEmpresaSede periodoEmpresaSede, NotaCredito notaCredito,
			List<NotaCreditoProducto> notaCreditoProductoList)
		{

			if (notaCreditoProductoList.Count == 0)
				return;

			TransaccionAlmacen transaccionAlmacen = transaccionAlmacenRepository.SingleSearch(new TransaccionAlmacen()
			{
				EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId,
				Codigo = TransaccionAlmacenConstante.DEVOLUCION_DE_VENTA
			});

			MesContable mesContable = mesContableRepository.ObtenerPorCodigo(string.Format(StringFormatConstant.DATE_MONTH, notaCredito.Fecha));

			EstadoMovimientoAlmacen estadoMovimientoAlmacen = estadoMovimientoAlmacenRepository.ObtenerPorCodigo(EstadoMovimientoAlmacenConstante.MOVIMIENTO);

			TipoDocumentoComercial tipoDocumentoComercial = tipoDocumentoComercialRepository.ObtenerPorCodigo(TipoDocumentoComercialConstante.NOTA_CREDITO);

			MovimientoAlmacenDTO movimientoAlmacenDTO = new MovimientoAlmacenDTO()
			{
				PeriodoEmpresaSedeId = notaCredito.PeriodoEmpresaSedeId,
				AlmacenId = notaCredito.Comprobante.AlmacenId.Value,
				TransaccionAlmacenId = transaccionAlmacen.EntityId,
				TipoMovimientoAlmacenId = transaccionAlmacen.TipoMovimientoAlmacenId.Value,
				ProveedorId = notaCredito.Comprobante.ProveedorId,
				Fecha = notaCredito.Fecha,
				MesContableId = mesContable.EntityId,
				NumeroOrdenCompra = string.Empty,
				NumeroGuiaRemision = string.Empty,
				ComprobanteId = notaCredito.EntityId,
				TipoDocumentoComercialId = tipoDocumentoComercial.TipoDocumentoComercialId,
				SerieDocumento = notaCredito.Serie,
				NumeroDocumento = notaCredito.Numero,
				TieneGuiaRemision = SiNoConstante.N,
				GuiaRemisionEnModificacion = SiNoConstante.N,
				AfectaSaldo = SiNoConstante.S,
				Observacion = notaCredito.Observacion,
				EstadoMovimientoAlmacenId = estadoMovimientoAlmacen.EntityId,
				Estado = EstadoConstante.ACTIVO,
				UsuarioCreacionId = notaCredito.UsuarioCreacionId
			};

			int newId = movimientoAlmacenBusiness.Insert(movimientoAlmacenDTO);

			foreach (NotaCreditoProducto notaCreditoProducto in notaCreditoProductoList)
			{
				MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = new MovimientoAlmacenProductoDTO()
				{
					MovimientoAlmacenDTO = movimientoAlmacenDTO,
					MovimientoAlmacenId = newId,
					ProductoId = notaCreditoProducto.ComprobanteProducto.ProductoId,
					Descripcion = notaCreditoProducto.Descripcion,
					Cantidad = notaCreditoProducto.Cantidad,
					RegistroReferencialId = notaCreditoProducto.EntityId,
					UsuarioCreacionId = notaCredito.UsuarioCreacionId
				};

				movimientoAlmacenProductoBusiness.Insert(movimientoAlmacenProductoDTO);
			}

		}
		public void ActualizarEmisionElectronica(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = Mapper.Map<NotaCredito>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					notaCreditoRepository.ActualizarEmisionElectronica(notaCredito);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarEstadoSunat(NotaCreditoDTO dto)
      {
			try
			{
				NotaCredito notaCredito = Mapper.Map<NotaCredito>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					notaCreditoRepository.ActualizarEstadoSunat(notaCredito);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarPorComprobanteElectronicoIdList(NotaCreditoDTO dto)
      {
			try
			{
				NotaCredito notaCredito = Mapper.Map<NotaCredito>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					notaCreditoRepository.ActualizarPorComprobanteElectronicoIdList(notaCredito);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Cerrar(NotaCreditoDTO dto)
		{
			try
			{
				NotaCredito notaCredito = Mapper.Map<NotaCredito>(dto);

				List<NotaCreditoProducto> notaCreditoProductoList = notaCreditoProductoRepository.Search(new NotaCreditoProducto()
				{
					NotaCreditoId = dto.NotaCreditoId,
					Estado = EstadoConstante.ACTIVO
				});

				if (notaCreditoProductoList.Count == 0)
					throw new Exception(SR.GetString("EX_nota_credito_sin_items"));

				using (TransactionScope tx = new TransactionScope())
				{

					notaCredito.UsuarioCreacionId = dto.UsuarioCreacionId;
					notaCreditoRepository.Update(notaCredito);

					notaCredito = notaCreditoRepository.GetById(dto.NotaCreditoId);

					if (notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.CERRADO)
						throw new Exception(SR.GetString("EX_nota_credito_ya_esta_cerrada"));

					if (notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.ANULADO)
						throw new Exception(SR.GetString("EX_nota_credito_ya_esta_anulad1"));

					if (notaCredito.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
						throw new Exception(SR.GetString("EX_nota_credito_ya_esta_emitida"));

					EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.CERRADO); 
					notaCredito.EstadoComprobanteId = estadoComprobante.EntityId;

					notaCreditoRepository.ActualizarEstadoComprobante(notaCredito);

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