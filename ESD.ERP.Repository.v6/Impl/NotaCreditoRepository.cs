using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class NotaCreditoRepository : INotaCreditoRepository
	{
		private const string SP_PRE_NAME = "sp_nota_credito";
		public NotaCredito GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(NotaCredito),
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(TipoNotaCredito),
						typeof(Comprobante),
						typeof(TipoDocumentoComercial),
						typeof(Cotizacion),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(ImpuestoTasa),
						typeof(EstadoComprobante)
					},
					x =>
					{
						NotaCredito nc = x[0] as NotaCredito;
						PeriodoEmpresaSede pes = x[1] as PeriodoEmpresaSede;
						EmpresaSede es = x[2] as EmpresaSede;
						TipoNotaCredito tnc = x[3] as TipoNotaCredito;
						Comprobante c = x[4] as Comprobante;
						TipoDocumentoComercial tdc = x[5] as TipoDocumentoComercial;
						Cotizacion ct = x[6] as Cotizacion;
						Proveedor p = x[7] as Proveedor;
						Moneda m = x[8] as Moneda;
						Vendedor v = x[9] as Vendedor;
						Proveedor p2 = x[10] as Proveedor;
						ImpuestoTasa it = x[11] as ImpuestoTasa;
						EstadoComprobante ec = x[12] as EstadoComprobante;

						nc.PeriodoEmpresaSede = pes;
						nc.PeriodoEmpresaSedeId = pes.EntityId;
						pes.EmpresaSede = es;
						pes.EmpresaSedeId = es.EntityId;
						nc.TipoNotaCredito = tnc;
						nc.TipoNotaCreditoId = tnc.EntityId;
						nc.Comprobante = c;
						nc.ComprobanteId = c.EntityId;
						c.TipoDocumentoComercial = tdc;
						c.TipoDocumentoComercialId = tdc.EntityId;
						c.Cotizacion = ct;
						c.CotizacionId = ct?.EntityId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						nc.Vendedor = v;
						nc.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						nc.ImpuestoTasa = it;
						nc.ImpuestoTasaId = it.EntityId;
						nc.EstadoComprobante = ec;
						nc.EstadoComprobanteId = ec.EntityId;

						return nc;
					},
					"periodo_emp_sede_id,empresa_sede_id,tipo_nota_c_id,comprobante_id,tipo_doc_com_id,cotiza_id,proveedor_id,moneda_id," +
					"vendedor_id,proveedor_id,impuesto_tasa_id,estado_compro_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<NotaCredito> Search(NotaCredito entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("numero", entity.Numero);
			parameters.AddParameter("containsRazonSocial", entity.Comprobante?.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Comprobante?.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("containsVendedor", entity.Vendedor?.Proveedor?.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("tipoNotaCreditoId", entity.TipoNotaCreditoId);
			parameters.AddParameter("estadoCotizacionList", entity.TipoNotaCreditoList);
			parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
			parameters.AddParameter("tipoDocComprobanteId", entity.Comprobante?.TipoDocumentoComercialId);
			parameters.AddParameter("comprobante", entity.Comprobante?.Numero);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(NotaCredito),
						typeof(TipoNotaCredito),
						typeof(Comprobante),
						typeof(TipoDocumentoComercial),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(EstadoComprobante),
						typeof(EstadoSunat)
					},
					x =>
					{
						NotaCredito nc = x[0] as NotaCredito;
						TipoNotaCredito tnc = x[1] as TipoNotaCredito;
						Comprobante c = x[2] as Comprobante;
						TipoDocumentoComercial tdc = x[3] as TipoDocumentoComercial;
						Proveedor p = x[4] as Proveedor;
						Moneda m = x[5] as Moneda;
						Vendedor v = x[6] as Vendedor;
						Proveedor p2 = x[7] as Proveedor;
						EstadoComprobante ec = x[8] as EstadoComprobante;
						EstadoSunat es = x[9] as EstadoSunat;

						nc.TipoNotaCredito = tnc;
						nc.TipoNotaCreditoId = tnc.EntityId;
						nc.Comprobante = c;
						nc.ComprobanteId = c.EntityId;
						c.TipoDocumentoComercial = tdc;
						c.TipoDocumentoComercialId = tdc.EntityId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						nc.Vendedor = v;
						nc.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						nc.EstadoComprobante = ec;
						nc.EstadoComprobanteId = ec.EntityId;
						nc.EstadoSunat = es;
						nc.EstadoSunatId = es?.EntityId;

						return nc;
					},
					"tipo_nota_c_id,comprobante_id,tipo_doc_com_id,proveedor_id,moneda_id,vendedor_id,proveedor_id,estado_compro_id,estado_sunat_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public NotaCredito SingleSearch(NotaCredito entity)
		{
			try
			{
				List<NotaCredito> notaCreditoList = Search(entity);

				return notaCreditoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("numeroOperacion", entity.NumeroOperacion);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("tipoNotaCreditoId", entity.TipoNotaCreditoId);
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("esContingencia", entity.EsContingencia);
				parameters.AddParameter("emitido", entity.Emitido);
				parameters.AddParameter("comprobanteElectronicoId", entity.ComprobanteElectronicoId);
				parameters.AddParameter("codigoHash", entity.CodigoHash);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);
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
		public int Update(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
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
		public void Delete(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
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
		public void ActualizarMonto(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMonto"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEmision(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("emitido", entity.Emitido);
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
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
		public void ActualizarEmisionElectronica(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
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
		public void ActualizarEstadoSunat(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoSunat"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarPorComprobanteElectronicoIdList(NotaCredito entity)
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
		public void ActualizarEstadoComprobante(NotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
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
	}
}
