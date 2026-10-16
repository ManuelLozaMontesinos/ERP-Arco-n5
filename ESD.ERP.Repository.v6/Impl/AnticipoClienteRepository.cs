using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class AnticipoClienteRepository : IAnticipoClienteRepository
	{
		private const string SP_PRE_NAME = "sp_anticipo_cliente";
		public AnticipoCliente GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(AnticipoCliente),
						typeof(Proveedor),
						typeof(TipoCargoAbono),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(MedioPago),
						typeof(TipoOperacionAnticipo),
						typeof(EstadoComprobante)
					},
					x =>
					{
						AnticipoCliente ac = x[0] as AnticipoCliente;
						Proveedor p = x[1] as Proveedor;
						TipoCargoAbono tca = x[2] as TipoCargoAbono;
						Vendedor v = x[3] as Vendedor;
						Proveedor p2 = x[4] as Proveedor;
						MedioPago mp = x[5] as MedioPago;
						TipoOperacionAnticipo toa = x[6] as TipoOperacionAnticipo;
						EstadoComprobante ec = x[7] as EstadoComprobante;

						ac.Proveedor = p;
						ac.ProveedorId = p.EntityId;
						ac.TipoCargoAbono = tca;
						ac.TipoCargoAbonoId = tca.EntityId;
						ac.Vendedor = v;
						ac.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						ac.MedioPago = mp;
						ac.MedioPagoId = mp.EntityId;
						ac.TipoOperacionAnticipo = toa;
						ac.TipoOperacionAnticipoId = toa.EntityId;
						ac.EstadoComprobante = ec;
						ac.EstadoComprobanteId = ec.EntityId;

						return ac;
					},
					"proveedor_id,tipo_car_abo_id,vendedor_id,proveedor_id,medio_pago_id,tipo_ope_antic_id,estado_compro_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<AnticipoCliente> Search(AnticipoCliente entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("anticipoClienteIdList", entity.AnticipoClienteIdList);
			parameters.AddParameter("proveedorId", entity.ProveedorId);
			parameters.AddParameter("tipoCargoAbono", entity?.TipoCargoAbono?.Codigo);
			parameters.AddParameter("monedaId", entity.MonedaId);
			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("cotizacionId", entity.CotizacionId);
			parameters.AddParameter("nullCotizacionId", entity.NullCotizacionId);
			parameters.AddParameter("tipoOperacionAnticipo", entity?.TipoOperacionAnticipo?.Codigo);
			parameters.AddParameter("tipoOperacionAnticipoList", entity.TipoOperacionAnticipoList);
			parameters.AddParameter("extornado", entity.Extornado);
			parameters.AddParameter("estadoComprobante", entity?.EstadoComprobante?.Codigo);
			parameters.AddParameter("conSaldo", entity.ConSaldo);
			parameters.AddParameter("estado", entity.Estado);
			parameters.AddParameter("nullFechaBaja", entity.NullFechaBaja);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(AnticipoCliente),
						typeof(TipoCargoAbono),
						typeof(Moneda),
						typeof(MedioPago),
						typeof(Banco),
						typeof(TipoOperacionAnticipo),
						typeof(TipoDocumentoComercial),
						typeof(Comprobante),
						typeof(TipoDocumentoComercial),
						typeof(AnticipoCliente),
						typeof(Comprobante),
						typeof(Cotizacion),
						typeof(EstadoComprobante)
					},
					x =>
					{
						AnticipoCliente ac = x[0] as AnticipoCliente;
						TipoCargoAbono tca = x[1] as TipoCargoAbono;
						Moneda m = x[2] as Moneda;
						MedioPago mp = x[3] as MedioPago;
						Banco b = x[4] as Banco;
						TipoOperacionAnticipo tocc = x[5] as TipoOperacionAnticipo;
						TipoDocumentoComercial tdc = x[6] as TipoDocumentoComercial;
						Comprobante c = x[7] as Comprobante;
						TipoDocumentoComercial tdc2 = x[8] as TipoDocumentoComercial;
						AnticipoCliente ac2 = x[9] as AnticipoCliente;
						Comprobante c2 = x[10] as Comprobante;
						Cotizacion co = x[11] as Cotizacion;
						EstadoComprobante ec = x[12] as EstadoComprobante;

						ac.TipoCargoAbono = tca;
						ac.TipoCargoAbonoId = tca.EntityId;
						ac.Moneda = m;
						ac.MonedaId = m.EntityId;
						ac.MedioPago = mp;
						ac.MedioPagoId = mp.EntityId;
						ac.Banco = b;
						ac.BancoId = b?.EntityId;
						ac.TipoOperacionAnticipo = tocc;
						ac.TipoOperacionAnticipoId = tocc.EntityId;
						ac.TipoDocumentoComercial = tdc;
						ac.TipoDocumentoComercialId = tdc?.EntityId;
						ac.Comprobante = c;
						ac.ComprobanteId = c?.EntityId;
						if (!Equals(c, null))
						{
							c.TipoDocumentoComercial = tdc2;
							c.TipoDocumentoComercialId = tdc2.EntityId;
						}
						ac.AnticipoClientePadre = ac2;
						ac.AnticipoClientePadreId = ac2?.EntityId;
						if (!Equals(ac2, null))
						{
							ac2.Comprobante = c2;
							ac2.ComprobanteId = c2?.EntityId;
						}
						ac.Cotizacion = co;
						ac.CotizacionId = co?.EntityId;
						ac.EstadoComprobante = ec;
						ac.EstadoComprobanteId = ec.EntityId;

						return ac;
					},
					"tipo_car_abo_id,moneda_id,medio_pago_id,banco_id,tipo_ope_antic_id,tipo_doc_com_id," +
					"comprobante_id,tipo_doc_com_id,anticipo_cliente_id,comprobante_id,cotiza_id,estado_compro_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public AnticipoCliente SingleSearch(AnticipoCliente entity)
		{
			try
			{
				List<AnticipoCliente> anticipoClienteList = Search(entity);

				return anticipoClienteList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(AnticipoCliente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("tipoCargoAbonoId", entity.TipoCargoAbonoId);
				parameters.AddParameter("numeroOperacion", entity.NumeroOperacion);
				parameters.AddParameter("secuencia", entity.Secuencia);
				parameters.AddParameter("numeroGrupo", entity.NumeroGrupo);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("medioPagoId", entity.MedioPagoId);
				parameters.AddParameter("bancoId", entity.BancoId);
				parameters.AddParameter("fechaReferencia", entity.FechaReferencia);
				parameters.AddParameter("numeroReferencia", entity.NumeroReferencia);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("montoAplicado", entity.MontoAplicado);
				parameters.AddParameter("tipoOperacionAnticipoId", entity.TipoOperacionAnticipoId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("anticipoClientePadreId", entity.AnticipoClientePadreId);
				parameters.AddParameter("extornado", entity.Extornado);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("registradoEnCotizacion", entity.RegistradoEnCotizacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);
				parameters.AddNewIdParameter();

				int id = DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.INSERT), parameters);

				return id;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Update(AnticipoCliente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("anticipoClienteId", entity.AnticipoClienteId);
				parameters.AddParameter("medioPagoId", entity.MedioPagoId);
				parameters.AddParameter("bancoId", entity.BancoId);
				parameters.AddParameter("fechaReferencia", entity.FechaReferencia);
				parameters.AddParameter("numeroReferencia", entity.NumeroReferencia);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("tipoOperacionAnticipoId", entity.TipoOperacionAnticipoId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("anticipoClientePadreId", entity.AnticipoClientePadreId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(AnticipoCliente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("anticipoClienteId", entity.AnticipoClienteId);
				parameters.AddParameter("tipoOperacionAnticipoId", entity.TipoOperacionAnticipoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarExtornado(AnticipoCliente entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("anticipoClienteId", entity.AnticipoClienteId);
				parameters.AddParameter("extornado", entity.Extornado);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarExtornado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoComprobante(AnticipoCliente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("anticipoClienteId", entity.AnticipoClienteId);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoComprobante"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEmision(AnticipoCliente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("anticipoClienteId", entity.AnticipoClienteId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("cajaId", entity.CajaId);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoEmitido"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarMontoAplicado(AnticipoCliente entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("anticipoClienteId", entity.AnticipoClienteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMontoAplicado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int ObtenerSecuencia(AnticipoCliente entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("proveedorId", entity.ProveedorId);
			parameters.AddParameter("numeroGrupo", entity.NumeroGrupo);

			try
			{
				return DbFactory.ExecuteScalar<int>(DbCommand.Create(SP_PRE_NAME, "_obtenerSecuencia"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
