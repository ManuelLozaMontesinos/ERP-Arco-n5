using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ComprobanteRepository : IComprobanteRepository
	{
		private const string SP_PRE_NAME = "sp_comprobante";
		public Comprobante GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Comprobante),
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(TipoDocumentoComercial),
						typeof(Cotizacion),
						typeof(Pedido),
						typeof(Proveedor),
						typeof(TipoNaturalezaJuridica),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(TipoCondicionPago),
						typeof(ImpuestoTasa),
						typeof(TipoComprobante),
						typeof(EstadoComprobante),
						typeof(EstadoDespacho)
					},
					x =>
					{
						Comprobante c = x[0] as Comprobante;
						PeriodoEmpresaSede pes = x[1] as PeriodoEmpresaSede;
						EmpresaSede es = x[2] as EmpresaSede;
						TipoDocumentoComercial tdc = x[3] as TipoDocumentoComercial;
						Cotizacion ct = x[4] as Cotizacion;
						Pedido ped = x[5] as Pedido;
						Proveedor p = x[6] as Proveedor;
						TipoNaturalezaJuridica tnj = x[7] as TipoNaturalezaJuridica;
						Vendedor v = x[8] as Vendedor;
						Proveedor p2 = x[9] as Proveedor;
						Moneda m = x[10] as Moneda;
						TipoCondicionPago tp = x[11] as TipoCondicionPago;
						ImpuestoTasa it = x[12] as ImpuestoTasa;
						TipoComprobante tc = x[13] as TipoComprobante;
						EstadoComprobante ec = x[14] as EstadoComprobante;
						EstadoDespacho ed = x[15] as EstadoDespacho;

						c.PeriodoEmpresaSede = pes;
						c.PeriodoEmpresaSedeId = pes.EntityId;
						pes.EmpresaSede = es;
						pes.EmpresaSedeId = es.EmpresaSedeId;
						c.TipoDocumentoComercial = tdc;
						c.TipoDocumentoComercialId = tdc.EntityId;
						c.Cotizacion = ct;
						c.CotizacionId = ct?.EntityId;
						c.Pedido = ped;
						c.PedidoId = ped?.EntityId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						p.TipoNaturalezaJuridica = tnj;
						p.TipoNaturalezaJuridicaId = tnj.EntityId;
						c.Vendedor = v;
						c.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						c.TipoCondicionPago = tp;
						c.TipoCondicionPagoId = tp.EntityId;
						c.ImpuestoTasa = it;
						c.ImpuestoTasaId = it.EntityId;
						c.TipoComprobante = tc;
						c.TipoComprobanteId = tc.EntityId;
						c.EstadoComprobante = ec;
						c.EstadoComprobanteId = ec.EntityId;
						c.EstadoDespacho = ed;
						c.EstadoDespachoId = ed?.EntityId;

						return c;
					},
					"periodo_emp_sede_id,empresa_sede_id,tipo_doc_com_id,cotiza_id,pedido_id,proveedor_id,tipo_nat_juri_id," +
					"vendedor_id,proveedor_id,moneda_id,tipo_c_pago_id,impuesto_tasa_id,tipo_comprob_id,estado_compro_id,estado_despacho_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Comprobante> Search(Comprobante entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("tipoDocumentoComercial", entity?.TipoDocumentoComercial?.Codigo);
			parameters.AddParameter("tipoDocumentoComercialList", entity.TipoDocumentoComercialList);
			parameters.AddParameter("numero", entity.Numero);
			parameters.AddParameter("cotizacion", entity?.Cotizacion?.Codigo);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("containsVendedor", entity.Vendedor?.Proveedor?.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("cotizacionId", entity.CotizacionId);
			parameters.AddParameter("montoTotalMayorIgual", entity.MontoTotalMayorIgual);
			parameters.AddParameter("tipoComprobanteId", entity.TipoComprobanteId);
			parameters.AddParameter("tipoComprobante", entity?.TipoComprobante?.Codigo);
			parameters.AddParameter("tipoComprobanteList", entity.TipoComprobanteList);
			parameters.AddParameter("estadoSunat", entity?.EstadoSunat?.Codigo);
			parameters.AddParameter("tipoDetraccion", entity?.TipoDetraccion?.Codigo);
			parameters.AddParameter("notTipoDetraccion", entity.NotTipoDetraccion);
			parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
			parameters.AddParameter("estadoComprobanteList", entity.EstadoComprobanteCodigoList);
			parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
			parameters.AddParameter("bancarizado", entity.Bancarizado);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Comprobante),
						typeof(TipoDocumentoComercial),
						typeof(Cotizacion),
						typeof(Pedido),
						typeof(Proveedor),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(TipoCondicionPago),
						typeof(MedioPago),
						typeof(TipoComprobante),
						typeof(TipoDetraccion),
						typeof(EstadoComprobante),
						typeof(EstadoSunat)
					},
					x =>
					{
						Comprobante c = x[0] as Comprobante;
						TipoDocumentoComercial tdc = x[1] as TipoDocumentoComercial;
						Cotizacion ct = x[2] as Cotizacion;
						Pedido ped = x[3] as Pedido;
						Proveedor p = x[4] as Proveedor;
						Vendedor v = x[5] as Vendedor;
						Proveedor p2 = x[6] as Proveedor;
						Moneda m = x[7] as Moneda;
						TipoCondicionPago tcp = x[8] as TipoCondicionPago;
						MedioPago mp = x[9] as MedioPago;
						TipoComprobante tc = x[10] as TipoComprobante;
						TipoDetraccion td = x[11] as TipoDetraccion;
						EstadoComprobante ec = x[12] as EstadoComprobante;
						EstadoSunat es = x[13] as EstadoSunat;

						c.TipoDocumentoComercial = tdc;
						c.TipoDocumentoComercialId = tdc.EntityId;
						c.Cotizacion = ct;
						c.CotizacionId = ct?.EntityId;
						c.Pedido = ped;
						c.PedidoId = ped?.EntityId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						c.Vendedor = v;
						c.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						c.TipoCondicionPago = tcp;
						c.TipoCondicionPagoId = tcp.EntityId;
						c.MedioPago = mp;
						c.MedioPagoId = mp.EntityId;
						c.TipoComprobante = tc;
						c.TipoComprobanteId = tc.EntityId;
						c.TipoDetraccion = td;
						c.TipoDetraccionId = td.EntityId;
						c.EstadoComprobante = ec;
						c.EstadoComprobanteId = ec.EntityId;
						c.EstadoSunat = es;
						c.EstadoSunatId = es?.EntityId;

						return c;
					},
					"tipo_doc_com_id,cotiza_id,pedido_id,proveedor_id,vendedor_id,proveedor_id,moneda_id,tipo_c_pago_id," +
					"medio_pago_id,tipo_comprob_id,tipo_detrac_id,estado_compro_id,estado_sunat_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Comprobante SingleSearch(Comprobante entity)
		{
			try
			{
				List<Comprobante> comprobanteList = Search(entity);

				return comprobanteList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("numeroOperacion", entity.NumeroOperacion);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("pedidoId", entity.PedidoId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("nombreAlterno", entity.NombreAlterno);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTipoCambio", entity.MontoTipoCambio);
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
				parameters.AddParameter("tipoCondicionPagoId", entity.TipoCondicionPagoId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("medioPagoId", entity.MedioPagoId);
				parameters.AddParameter("porcentajeTarjeta", entity.PorcentajeTarjeta);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("numeroGuiaRemision", entity.NumeroGuiaRemision);
				parameters.AddParameter("montoBrutoDocumento", entity.MontoBrutoDocumento);
				parameters.AddParameter("montoNetoDocumento", entity.MontoNetoDocumento);
				parameters.AddParameter("montoIgvDocumento", entity.MontoIgvDocumento);
				parameters.AddParameter("montoTotalDocumento", entity.MontoTotalDocumento);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("porcentajeDescuento", entity.PorcentajeDescuento);
				parameters.AddParameter("montoDescuento", entity.MontoDescuento);
				parameters.AddParameter("montoDescuentoItem", entity.MontoDescuentoItem);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoAnticipo", entity.MontoAnticipo);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("montoAnticipoDescuento", entity.MontoAnticipoDescuento);
				parameters.AddParameter("canjeado", entity.Canjeado);
				parameters.AddParameter("tipoComprobanteId", entity.TipoComprobanteId);
				parameters.AddParameter("comprobanteCanjeId", entity.ComprobanteCanjeId);
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);
				parameters.AddParameter("tipoDetraccionId", entity.TipoDetraccionId);
				parameters.AddParameter("porcentajeDetraccion", entity.PorcentajeDetraccion);
				parameters.AddParameter("montoDetraccion", entity.MontoDetraccion);
				parameters.AddParameter("numeroOperacionPago", entity.NumeroOperacionPago);
				parameters.AddParameter("montoPago", entity.MontoPago);
				parameters.AddParameter("fechaPago", entity.FechaPago);
				parameters.AddParameter("montoCargo", entity.MontoCargo);
				parameters.AddParameter("esContingencia", entity.EsContingencia);
				parameters.AddParameter("emitido", entity.Emitido);
				parameters.AddParameter("comprobanteElectronicoId", entity.ComprobanteElectronicoId);
				parameters.AddParameter("codigoHash", entity.CodigoHash);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("bancarizado", entity.Bancarizado);
				parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
				parameters.AddParameter("observacionDespacho", entity.ObservacionDespacho);
				parameters.AddParameter("entregaDespacho", entity.EntregaDespacho);
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
		public int Update(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("nombreAlterno", entity.NombreAlterno);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("tipoCondicionPagoId", entity.TipoCondicionPagoId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("medioPagoId", entity.MedioPagoId);
				parameters.AddParameter("porcentajeTarjeta", entity.PorcentajeTarjeta);
				parameters.AddParameter("numeroGuiaRemision", entity.NumeroGuiaRemision);
				parameters.AddParameter("tipoDetraccionId", entity.TipoDetraccionId);
				parameters.AddParameter("porcentajeDetraccion", entity.PorcentajeDetraccion);
				parameters.AddParameter("montoDetraccion", entity.MontoDetraccion);
				parameters.AddParameter("esContingencia", entity.EsContingencia);
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
		public void Delete(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoComprobante(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
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
		public void ActualizarMonto(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMonto"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarMedioPago(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("medioPagoId", entity.MedioPagoId);
				parameters.AddParameter("porcentajeTarjeta", entity.PorcentajeTarjeta);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMedioPago"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEmision(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("tipoDetraccionId", entity.TipoDetraccionId);
				parameters.AddParameter("porcentajeDetraccion", entity.PorcentajeDetraccion);
				parameters.AddParameter("montoDetraccion", entity.MontoDetraccion);
				parameters.AddParameter("emitido", entity.Emitido);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("cajaId", entity.CajaId);
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoEmitido"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoSunat(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoSunat"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarNumeroGuiaRemision(Comprobante entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("numeroGuiaRemision", entity.NumeroGuiaRemision);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarNumeroGuiaRemision"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarEmisionElectronica(Comprobante entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);
				parameters.AddParameter("comprobanteElectronicoId", entity.ComprobanteElectronicoId);
				parameters.AddParameter("codigoHash", entity.CodigoHash);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEmisionElectronica"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Comprobante> ListarDocumentoEmitido(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("empresaId", entity?.PeriodoEmpresaSede?.EmpresaSede?.EmpresaId);
			parameters.AddParameter("tipoDocumentoComercial", entity.TipoDocumentoComercial.Codigo);
			parameters.AddParameter("tipoDocumentoComercialList", entity.TipoDocumentoComercialList);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("estadoSunatList", entity.EstadoSunatList);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarDocumentoEmitido"), parameters,
					new[]
					{
						typeof(Comprobante),
						typeof(TipoDocumentoComercial),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(EstadoSunat)
					},
					x =>
					{
						Comprobante c = x[0] as Comprobante;
						TipoDocumentoComercial tdc = x[1] as TipoDocumentoComercial;
						Proveedor p = x[2] as Proveedor;
						Moneda m = x[3] as Moneda;
						EstadoSunat esu = x[4] as EstadoSunat;

						c.TipoDocumentoComercial = tdc;
						c.TipoDocumentoComercialId = tdc.EntityId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						c.EstadoSunat = esu;
						c.EstadoSunatId = esu.EntityId;

						return c;
					},
					"tipo_doc_com_id,proveedor_id,moneda_id,estado_sunat_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Comprobante> ListarResumenEstadoSunat(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("empresaId", entity?.PeriodoEmpresaSede?.EmpresaSede?.EmpresaId);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarResumenEstadoSunat"), parameters,
					new[]
					{
						typeof(Comprobante),
						typeof(EstadoSunat)
					},
					x =>
					{
						Comprobante c = x[0] as Comprobante;
						EstadoSunat esu = x[1] as EstadoSunat;

						c.EstadoSunat = esu;
						c.EstadoSunatId = esu.EntityId;

						return c;
					},
					"estado_sunat_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarPorComprobanteElectronicoIdList(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);
				parameters.AddParameter("comprobanteElectronicoIdList", entity.ComprobanteElectronicoIdList);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarPorComprobanteElectronicoIdList"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarDetraccion(Comprobante entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("numeroOperacionPago", entity.NumeroOperacionPago);
				parameters.AddParameter("montoPago", entity.MontoPago);
				parameters.AddParameter("fechaPago", entity.FechaPago);
				parameters.AddParameter("montoCargo", entity.MontoCargo);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarDetraccion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarBancarizado(Comprobante entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("bancarizado", entity.Bancarizado);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarBancarizado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarTipoComprobante(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("tipoComprobanteId", entity.TipoComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarTipoComprobante"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<dynamic> ListarCobranza(Comprobante entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("numero", entity.Numero);
			parameters.AddParameter("cotizacion", entity?.Cotizacion?.Codigo);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("containsVendedor", entity.Vendedor?.Proveedor?.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
			parameters.AddParameter("estadoComprobanteList", entity.EstadoComprobanteCodigoList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				List<dynamic> resultList = DbFactory.Query<dynamic>(DbCommand.Create(SP_PRE_NAME, "_listarCobranza"), parameters);

				return resultList;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarMontoAnticipo(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMontoAnticipo"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarMontoAnticipoDescuento(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMontoAnticipoDescuento"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public List<Comprobante> BuscarDespacho(Comprobante entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSede?.PeriodoEmpresaSedeId);
            parameters.AddParameter("tipoDocumentoComercial", entity?.TipoDocumentoComercial?.Codigo);
            parameters.AddParameter("tipoDocumentoComercialList", entity.TipoDocumentoComercialList);
            parameters.AddParameter("numero", entity.Numero);
            parameters.AddParameter("cotizacion", entity?.Cotizacion?.Codigo);
            parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
            parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
            parameters.AddParameter("containsVendedor", entity.Vendedor?.Proveedor?.RazonSocial);
            parameters.AddParameter("fechaDesde", entity.FechaDesde);
            parameters.AddParameter("fechaHasta", entity.FechaHasta);
            parameters.AddParameter("tipoComprobanteId", entity.TipoComprobanteId);
            parameters.AddParameter("tipoComprobante", entity?.TipoComprobante?.Codigo);
            parameters.AddParameter("tipoComprobanteList", entity.TipoComprobanteList);
            parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
            parameters.AddParameter("estadoComprobanteList", entity.EstadoComprobanteCodigoList);
            parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
            parameters.AddParameter("estado", entity.Estado);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_buscarDespacho"), parameters,
                    new[]
                    {
                        typeof(Comprobante),
                        typeof(TipoDocumentoComercial),
                        typeof(Cotizacion),
                        typeof(Proveedor),
                        typeof(Vendedor),
                        typeof(Proveedor),
                        typeof(TipoComprobante),
                        typeof(EstadoComprobante),
                        typeof(EstadoDespacho),
                        typeof(Personal),
                        typeof(Proveedor)
                    },
                    x =>
                    {
                        Comprobante c = x[0] as Comprobante;
                        TipoDocumentoComercial tdc = x[1] as TipoDocumentoComercial;
                        Cotizacion ct = x[2] as Cotizacion;
                        Proveedor p = x[3] as Proveedor;
                        Vendedor v = x[4] as Vendedor;
                        Proveedor p2 = x[5] as Proveedor;
                        TipoComprobante tc = x[6] as TipoComprobante;
                        EstadoComprobante ec = x[7] as EstadoComprobante;
                        EstadoDespacho ed = x[8] as EstadoDespacho;
                        Personal per = x[9] as Personal;
                        Proveedor p3 = x[10] as Proveedor;

                        c.TipoDocumentoComercial = tdc;
                        c.TipoDocumentoComercialId = tdc.EntityId;
                        c.Cotizacion = ct;
                        c.CotizacionId = ct?.EntityId;
                        c.Proveedor = p;
                        c.ProveedorId = p.EntityId;
                        c.Vendedor = v;
                        c.VendedorId = v.EntityId;
                        v.Proveedor = p2;
                        v.ProveedorId = p2.EntityId;
                        c.TipoComprobante = tc;
                        c.TipoComprobanteId = tc.EntityId;
                        c.EstadoComprobante = ec;
                        c.EstadoComprobanteId = ec.EntityId;
                        c.EstadoDespacho = ed;
                        c.EstadoDespachoId = ed?.EntityId;
                        c.Personal = per;
                        c.PersonalId = per?.EntityId;
                        if (!Equals(per, null))
                        {
                            per.Proveedor = p3;
                            per.ProveedorId = p3.EntityId;
                        }

                        return c;
                    },
                    "tipo_doc_com_id,cotiza_id,proveedor_id,vendedor_id,proveedor_id,tipo_comprob_id,estado_compro_id," +
                    "estado_despacho_id,personal_id,proveedor_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

		public List<Comprobante> BuscarPorEmpresaSede(Comprobante entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaSedeId", entity.PeriodoEmpresaSede.EmpresaSedeId);
			parameters.AddParameter("tipoDocumentoComercial", entity?.TipoDocumentoComercial?.Codigo);
			parameters.AddParameter("tipoDocumentoComercialList", entity.TipoDocumentoComercialList);
			parameters.AddParameter("numero", entity.Numero);
			parameters.AddParameter("cotizacion", entity?.Cotizacion?.Codigo);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("containsVendedor", entity.Vendedor?.Proveedor?.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("cotizacionId", entity.CotizacionId);
			parameters.AddParameter("montoTotalMayorIgual", entity.MontoTotalMayorIgual);
			parameters.AddParameter("tipoComprobanteId", entity.TipoComprobanteId);
			parameters.AddParameter("tipoComprobante", entity?.TipoComprobante?.Codigo);
			parameters.AddParameter("tipoComprobanteList", entity.TipoComprobanteList);
			parameters.AddParameter("estadoSunat", entity?.EstadoSunat?.Codigo);
			parameters.AddParameter("tipoDetraccion", entity?.TipoDetraccion?.Codigo);
			parameters.AddParameter("notTipoDetraccion", entity.NotTipoDetraccion);
			parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
			parameters.AddParameter("estadoComprobanteList", entity.EstadoComprobanteCodigoList);
			parameters.AddParameter("bancarizado", entity.Bancarizado);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_buscarPorEmpresaSede"), parameters,
					new[]
					{
						typeof(Comprobante),
						typeof(TipoDocumentoComercial),
						typeof(Cotizacion),
						typeof(Pedido),
						typeof(Proveedor),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(TipoCondicionPago),
						typeof(MedioPago),
						typeof(TipoComprobante),
						typeof(TipoDetraccion),
						typeof(EstadoComprobante),
						typeof(EstadoSunat)
					},
					x =>
					{
						Comprobante c = x[0] as Comprobante;
						TipoDocumentoComercial tdc = x[1] as TipoDocumentoComercial;
						Cotizacion ct = x[2] as Cotizacion;
						Pedido ped = x[3] as Pedido;
						Proveedor p = x[4] as Proveedor;
						Vendedor v = x[5] as Vendedor;
						Proveedor p2 = x[6] as Proveedor;
						Moneda m = x[7] as Moneda;
						TipoCondicionPago tcp = x[8] as TipoCondicionPago;
						MedioPago mp = x[9] as MedioPago;
						TipoComprobante tc = x[10] as TipoComprobante;
						TipoDetraccion td = x[11] as TipoDetraccion;
						EstadoComprobante ec = x[12] as EstadoComprobante;
						EstadoSunat es = x[13] as EstadoSunat;

						c.TipoDocumentoComercial = tdc;
						c.TipoDocumentoComercialId = tdc.EntityId;
						c.Cotizacion = ct;
						c.CotizacionId = ct?.EntityId;
						c.Pedido = ped;
						c.PedidoId = ped?.EntityId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						c.Vendedor = v;
						c.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						c.TipoCondicionPago = tcp;
						c.TipoCondicionPagoId = tcp.EntityId;
						c.MedioPago = mp;
						c.MedioPagoId = mp.EntityId;
						c.TipoComprobante = tc;
						c.TipoComprobanteId = tc.EntityId;
						c.TipoDetraccion = td;
						c.TipoDetraccionId = td.EntityId;
						c.EstadoComprobante = ec;
						c.EstadoComprobanteId = ec.EntityId;
						c.EstadoSunat = es;
						c.EstadoSunatId = es?.EntityId;

						return c;
					},
					"tipo_doc_com_id,cotiza_id,pedido_id,proveedor_id,vendedor_id,proveedor_id,moneda_id,tipo_c_pago_id," +
					"medio_pago_id,tipo_comprob_id,tipo_detrac_id,estado_compro_id,estado_sunat_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public void ActualizarEstadoDespacho(Comprobante entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("comprobanteId", entity.ComprobanteId);
                parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
                parameters.AddParameter("personalId", entity.PersonalId);
                parameters.AddParameter("fechaDespacho", entity.FechaDespacho);
                parameters.AddParameter("observacionDespacho", entity.ObservacionDespacho);
                parameters.AddParameter("entregaDespacho", entity.EntregaDespacho);
                parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoDespacho"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void ActualizarEstadoDespachoPorCantidadEntregada(Comprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoDespachoPorCantidadEntregada"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
