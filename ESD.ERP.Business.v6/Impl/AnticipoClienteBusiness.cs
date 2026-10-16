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
	public class AnticipoClienteBusiness : IAnticipoClienteBusiness
	{
		private readonly IAnticipoClienteRepository anticipoClienteRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly ICotizacionRepository cotizacionRepository;
		private readonly ITipoCargoAbonoRepository tipoCargoAbonoRepository;
		private readonly ITipoOperacionAnticipoRepository tipoOperacionAnticipoRepository;
		private readonly IEstadoComprobanteRepository estadoComprobanteRepository;
		private readonly ITipoDocumentoComercialRepository tipoDocumentoComercialRepository;
		private readonly IComprobanteBusiness comprobanteBusiness;
		private readonly IComprobanteRepository comprobanteRepository;
		private readonly IDocumentoComercialSerieBusiness documentoComercialSerieBusiness;
		private readonly IEstadoSunatRepository estadoSunatRepository;

		public AnticipoClienteBusiness(
			IAnticipoClienteRepository anticipoClienteRepository,
			ITablaBusiness tablaBusiness,
			ICotizacionRepository cotizacionRepository,
			ITipoCargoAbonoRepository tipoCargoAbonoRepository,
			ITipoOperacionAnticipoRepository tipoOperacionAnticipoRepository,
			IEstadoComprobanteRepository estadoComprobanteRepository,
			ITipoDocumentoComercialRepository tipoDocumentoComercialRepository,
			IComprobanteBusiness comprobanteBusiness,
			IComprobanteRepository comprobanteRepository,
			IDocumentoComercialSerieBusiness documentoComercialSerieBusiness,
			IEstadoSunatRepository estadoSunatRepository
			)
		{
			this.anticipoClienteRepository = anticipoClienteRepository;
			this.tablaBusiness = tablaBusiness;
			this.cotizacionRepository = cotizacionRepository;
			this.tipoCargoAbonoRepository = tipoCargoAbonoRepository;
			this.tipoOperacionAnticipoRepository = tipoOperacionAnticipoRepository;
			this.estadoComprobanteRepository = estadoComprobanteRepository;
			this.tipoDocumentoComercialRepository = tipoDocumentoComercialRepository;
			this.comprobanteBusiness = comprobanteBusiness;
			this.comprobanteRepository = comprobanteRepository;
			this.documentoComercialSerieBusiness = documentoComercialSerieBusiness;
			this.estadoSunatRepository = estadoSunatRepository;
		}
		public AnticipoClienteDTO GetById(int id)
		{
			try
			{
				AnticipoCliente anticipoCliente = anticipoClienteRepository.GetById(id);

				AnticipoClienteDTO anticipoClienteDTO = Mapper.Map<AnticipoClienteDTO>(anticipoCliente);

				return anticipoClienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AnticipoClienteDTO> Search(AnticipoClienteDTO dto)
		{
			try
			{
				List<AnticipoCliente> anticipoClienteList = anticipoClienteRepository.Search(Mapper.Map<AnticipoCliente>(dto));

				List<AnticipoClienteDTO> anticipoClienteDTOList = Mapper.Map<List<AnticipoClienteDTO>>(anticipoClienteList);

				return anticipoClienteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public AnticipoClienteDTO SingleSearch(AnticipoClienteDTO dto)
		{
			try
			{
				AnticipoCliente anticipoCliente = anticipoClienteRepository.SingleSearch(Mapper.Map<AnticipoCliente>(dto));

				AnticipoClienteDTO anticipoClienteDTO = Mapper.Map<AnticipoClienteDTO>(anticipoCliente);

				return anticipoClienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(AnticipoClienteDTO dto)
		{
			try
			{
				AnticipoCliente anticipoCliente = Mapper.Map<AnticipoCliente>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					if(dto.TipoCargoAbonoDTO.Codigo == TipoCargoAbonoConstante.ABONO)
               {
						TablaCorrelativoDTO tablaCorrelativoDTO = new()
                  {
							Tabla = TablaConstante.RECIBO_INGRESO,
							PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
							{
								EmpresaSedeDTO = dto.EmpresaSedeDTO
							},
							UsuarioAccionId = dto.UsuarioCreacionId
						};
						anticipoCliente.NumeroOperacion = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);
						anticipoCliente.NumeroGrupo = anticipoCliente.NumeroOperacion;
					}

					newId = anticipoClienteRepository.Insert(anticipoCliente);

					if (dto.CotizacionId.HasValue)
						cotizacionRepository.ActualizarMontoAnticipo(new Cotizacion() { CotizacionId = dto.CotizacionId.Value,
							Estado = EstadoConstante.ACTIVO
						});

					dto.AnticipoClienteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(AnticipoClienteDTO dto)
		{
			try
			{
				AnticipoCliente anticipoCliente = Mapper.Map<AnticipoCliente>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = anticipoClienteRepository.Update(anticipoCliente);

					if (dto.CotizacionId.HasValue)
						cotizacionRepository.ActualizarMontoAnticipo(new Cotizacion()
						{
							CotizacionId = dto.CotizacionId.Value,
							Estado = EstadoConstante.ACTIVO
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
		public void Delete(AnticipoClienteDTO dto)
		{
			try
			{

				AnticipoCliente anticipoCliente = anticipoClienteRepository.GetById(dto.AnticipoClienteId);

				TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository
					.ObtenerPorCodigo(TipoOperacionAnticipoConstante.ANULACION);

				using (TransactionScope tx = new())
				{
					anticipoCliente.TipoOperacionAnticipoId = tipoOperacionAnticipo.EntityId;
					anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;
					anticipoClienteRepository.Delete(anticipoCliente);

					if(anticipoCliente.TipoOperacionAnticipo.Codigo == TipoOperacionAnticipoConstante.ANTICIPO)
               {
						if (anticipoCliente.CotizacionId.HasValue)
							cotizacionRepository.ActualizarMontoAnticipo(new Cotizacion()
							{
								CotizacionId = anticipoCliente.CotizacionId.Value,
								Estado = EstadoConstante.ACTIVO
							});
					}
					else
						if(anticipoCliente.AnticipoClientePadreId.HasValue)
							anticipoClienteRepository.ActualizarMontoAplicado(new AnticipoCliente()
							{
								AnticipoClienteId = anticipoCliente.AnticipoClientePadreId.Value,
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
		public void Extornar(AnticipoClienteDTO dto)
		{
			try
			{

				using (TransactionScope tx = new())
				{
					AnticipoCliente anticipoCliente = anticipoClienteRepository.GetById(dto.AnticipoClienteId);
					anticipoCliente.Extornado = SiNoConstante.S;
					anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;

					anticipoClienteRepository.ActualizarExtornado(anticipoCliente);

					string codigo = string.Empty;
					if (anticipoCliente.TipoCargoAbono.Codigo == TipoCargoAbonoConstante.ABONO)
               {
						codigo = TipoCargoAbonoConstante.CARGO;
						anticipoCliente.Monto *= -1; 
					}
					else
						codigo = TipoCargoAbonoConstante.ABONO;

					TipoCargoAbono tipoCargoAbono = tipoCargoAbonoRepository.ObtenerPorCodigo(codigo);

					TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository
						.ObtenerPorCodigo(TipoOperacionAnticipoConstante.EXTORNO);

					anticipoCliente.TipoCargoAbonoId = tipoCargoAbono.EntityId;
					anticipoCliente.TipoOperacionAnticipoId = tipoOperacionAnticipo.EntityId;
					anticipoCliente.Extornado = SiNoConstante.N;
					anticipoCliente.AnticipoClientePadreId = dto.AnticipoClienteId;
					anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;

					anticipoClienteRepository.Insert(anticipoCliente);

					if (anticipoCliente.CotizacionId.HasValue)
						cotizacionRepository.ActualizarMontoAnticipo(new Cotizacion()
						{
							CotizacionId = anticipoCliente.CotizacionId.Value,
							Estado = EstadoConstante.ACTIVO
						});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Cerrar(AnticipoClienteDTO dto)
		{
			try
			{
				AnticipoCliente anticipoCliente = Mapper.Map<AnticipoCliente>(dto);

				using (TransactionScope tx = new())
				{

					EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.CERRADO);
					anticipoCliente.EstadoComprobanteId = estadoComprobante.EntityId;

					anticipoClienteRepository.ActualizarEstadoComprobante(anticipoCliente);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Emitir(AnticipoClienteDTO dto)
		{
			try
			{

				AnticipoCliente anticipoCliente = anticipoClienteRepository.GetById(dto.AnticipoClienteId);

				if (anticipoCliente.EstadoComprobante.Codigo == EstadoComprobanteConstante.ANULADO)
					throw new Exception(SR.GetString("EX_anticipo_ya_esta_anulado"));

				if (anticipoCliente.EstadoComprobante.Codigo == EstadoComprobanteConstante.EMITIDO)
					throw new Exception(SR.GetString("EX_anticipo_ya_esta_emitido"));

				TipoDocumentoComercial tipoDocumentoComercial = null;
				if (dto.TipoDocumentoComercialId.HasValue)
					tipoDocumentoComercial = tipoDocumentoComercialRepository.GetById(dto.TipoDocumentoComercialId.Value);

				EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(EstadoComprobanteConstante.EMITIDO);

				using (TransactionScope tx = new())
				{
					int? newId = null;
					if (dto.TipoDocumentoComercialId.HasValue)
               {
						dto.ComprobanteDTO.EstadoComprobanteId = estadoComprobante.EstadoComprobanteId;
						newId = comprobanteBusiness.AgregarComprobanteAnticipo(dto.ComprobanteDTO);

						Comprobante comprobante = Mapper.Map<Comprobante>(dto.ComprobanteDTO);

						comprobante.ComprobanteId = newId.Value;
						comprobante.Numero = documentoComercialSerieBusiness.ObtenerCorrelativo(new DocumentoComercialSerieDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = comprobante.PeriodoEmpresaSede.EmpresaSede.EmpresaId },
							TipoDocumentoComercialId = comprobante.TipoDocumentoComercialId,
							Serie = comprobante.Serie
						});

						comprobante.Emitido = SiNoConstante.S;
						comprobante.EstadoComprobanteId = estadoComprobante.EntityId;
						comprobante.CajaId = dto.CajaId.Value;

						EstadoSunat estadoSunat = estadoSunatRepository.ObtenerPorCodigo(EstadoSunatConstante.PENDIENTE_GENERAR);
						comprobante.EstadoSunatId = estadoSunat.EntityId;
						comprobanteRepository.ActualizarEmision(comprobante);
					}

					anticipoCliente.TipoDocumentoComercialId = dto.TipoDocumentoComercialId;
					anticipoCliente.ComprobanteId = newId;
					anticipoCliente.EstadoComprobanteId = estadoComprobante.EntityId;
					anticipoCliente.CajaId = dto.CajaId.Value;
					anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;

					anticipoClienteRepository.ActualizarEmision(anticipoCliente);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Aplicar(AnticipoClienteDTO dto)
		{
			try
			{

				List<AnticipoCliente> anticipoClienteList = anticipoClienteRepository.Search(new AnticipoCliente() { 
					AnticipoClienteIdList = dto.AnticipoClienteIdList
				});

				TipoCargoAbono tipoCargoAbono = tipoCargoAbonoRepository.ObtenerPorCodigo(TipoCargoAbonoConstante.CARGO);

				TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository
					.ObtenerPorCodigo(TipoOperacionAnticipoConstante.APLICACION);

				Comprobante comprobante = comprobanteRepository.GetById(dto.ComprobanteId.Value);

				decimal montoTotal = comprobante.MontoTotal;
				decimal montoAnticipoDescuento = decimal.Zero;
				decimal montoSaldo = montoTotal;

				if (comprobante.MontoAnticipoDescuento > decimal.Zero)
            {
					montoTotal = comprobante.MontoTotalDocumento;
					montoAnticipoDescuento = comprobante.MontoAnticipoDescuento;
				}

				montoSaldo = montoTotal - montoAnticipoDescuento;

				if (montoSaldo == decimal.Zero)
					throw new Exception(SR.GetString("EX_anticipo_sin_aplicacion_requerida"));

				using (TransactionScope tx = new())
				{

					foreach(AnticipoCliente anticipoCliente in anticipoClienteList)
               {
						decimal montoSaldoTmp = anticipoCliente.Monto - anticipoCliente.MontoAplicado;

						if (montoSaldoTmp > montoSaldo)
							montoSaldoTmp = montoSaldo;

						anticipoCliente.Fecha = dto.Fecha;
						anticipoCliente.TipoCargoAbonoId = tipoCargoAbono.EntityId;
						anticipoCliente.Monto = montoSaldoTmp;
						anticipoCliente.MontoAplicado = decimal.Zero;
						anticipoCliente.TipoOperacionAnticipoId = tipoOperacionAnticipo.EntityId;
						anticipoCliente.Extornado = SiNoConstante.N;
						anticipoCliente.CotizacionId = null;
						anticipoCliente.ComprobanteId = dto.ComprobanteId;
						anticipoCliente.Observacion = string.Empty;
						anticipoCliente.Serie = string.Empty;
						anticipoCliente.TipoDocumentoComercialId = null;
						anticipoCliente.AnticipoClientePadreId = anticipoCliente.AnticipoClienteId;
						anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;

						montoSaldo -= montoSaldoTmp;

						anticipoClienteRepository.Insert(anticipoCliente);

						anticipoClienteRepository.ActualizarMontoAplicado(new AnticipoCliente()
						{
							AnticipoClienteId = anticipoCliente.AnticipoClienteId,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});
					}

					comprobanteRepository.ActualizarMontoAnticipo(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId.Value,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					comprobanteRepository.ActualizarMontoAnticipoDescuento(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId.Value,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId.Value,
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
		public void AnularAplicacion(AnticipoClienteDTO dto)
		{
			try
			{

				AnticipoCliente anticipoCliente = anticipoClienteRepository.GetById(dto.AnticipoClienteId);

				TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository
					.ObtenerPorCodigo(TipoOperacionAnticipoConstante.ANULACION);

				using (TransactionScope tx = new())
				{

					anticipoCliente.TipoOperacionAnticipoId = tipoOperacionAnticipo.EntityId;
					anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;

					anticipoClienteRepository.Delete(anticipoCliente);

					anticipoClienteRepository.ActualizarMontoAplicado(new AnticipoCliente()
					{
						AnticipoClienteId = anticipoCliente.AnticipoClientePadreId.Value,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					if (anticipoCliente.ComprobanteId.HasValue)
					{
						comprobanteRepository.ActualizarMontoAnticipo(new Comprobante()
						{
							ComprobanteId = anticipoCliente.ComprobanteId.Value,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});

						comprobanteRepository.ActualizarMontoAnticipoDescuento(new Comprobante()
						{
							ComprobanteId = anticipoCliente.ComprobanteId.Value,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});

						comprobanteRepository.ActualizarMonto(new Comprobante()
						{
							ComprobanteId = anticipoCliente.ComprobanteId.Value,
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
		public void AnularAsignacionPedido(AnticipoClienteDTO dto)
		{
			try
			{
				AnticipoCliente anticipoCliente = Mapper.Map<AnticipoCliente>(dto);

				using (TransactionScope tx = new())
				{
					anticipoCliente.CotizacionId = null;
					anticipoClienteRepository.Update(anticipoCliente);

					if (dto.CotizacionId.HasValue)
						cotizacionRepository.ActualizarMontoAnticipo(new Cotizacion()
						{
							CotizacionId = dto.CotizacionId.Value,
							Estado = EstadoConstante.ACTIVO
						});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void DividirMonto(AnticipoClienteDTO dto)
		{
			try
			{

				AnticipoCliente anticipoCliente = anticipoClienteRepository.GetById(dto.AnticipoClienteId);

				using (TransactionScope tx = new())
				{
					if(dto.Monto == decimal.Zero)
						anticipoCliente.Monto = anticipoCliente.MontoAplicado;
					else
						anticipoCliente.Monto = dto.Monto;

					anticipoCliente.UsuarioCreacionId = dto.UsuarioCreacionId;
					anticipoClienteRepository.Update(anticipoCliente);

					int secuencia = anticipoClienteRepository.ObtenerSecuencia(anticipoCliente);
					anticipoCliente.Secuencia = secuencia + 1;
					anticipoCliente.NumeroOperacion = string.Concat(anticipoCliente.NumeroGrupo, SeparadorConstante.PUNTO, 
						anticipoCliente.Secuencia.ToString("00")); 
					anticipoCliente.CotizacionId = null;
					anticipoCliente.Monto = dto.MontoAplicado;
					anticipoCliente.MontoAplicado = decimal.Zero;

					anticipoClienteRepository.Insert(anticipoCliente);

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
