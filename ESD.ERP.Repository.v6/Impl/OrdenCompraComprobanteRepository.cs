using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenCompraComprobanteRepository : IOrdenCompraComprobanteRepository
	{
		private const string SP_PRE_NAME = "sp_orden_c_comp";
		public OrdenCompraComprobante GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OrdenCompraComprobante)
					},
					x =>
					{
						OrdenCompraComprobante occ = x[0] as OrdenCompraComprobante;

						return occ;
					},
					string.Empty
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<OrdenCompraComprobante> Search(OrdenCompraComprobante entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoId", entity?.OrdenCompra?.PeriodoEmpresaSede?.PeriodoId);
			parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
			parameters.AddParameter("ordenCompra", entity?.OrdenCompra?.Codigo);
            parameters.AddParameter("containsRazonSocial", entity.OrdenCompra?.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.OrdenCompra?.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
			parameters.AddParameter("serie", entity.Serie);
			parameters.AddParameter("numero", entity.Numero);
			parameters.AddParameter("registroReferencialId", entity.RegistroReferencialId);
			parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OrdenCompraComprobante),
						typeof(OrdenCompra),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(TipoDocumentoComercial),
						typeof(CondicionPago)
					},
					x =>
					{
						OrdenCompraComprobante occ = x[0] as OrdenCompraComprobante;
						OrdenCompra oc = x[1] as OrdenCompra;
						Proveedor p = x[2] as Proveedor;
						Moneda m = x[3] as Moneda;
						TipoDocumentoComercial tdc = x[4] as TipoDocumentoComercial;
						CondicionPago cp = x[5] as CondicionPago;

						occ.OrdenCompra = oc;
						occ.OrdenCompraId = oc.EntityId;
						oc.Proveedor = p;
						oc.ProveedorId = p.EntityId;
						oc.Moneda = m;
						oc.MonedaId = p.EntityId;
						occ.TipoDocumentoComercial = tdc;
						occ.TipoDocumentoComercialId = tdc.EntityId;
						occ.CondicionPago = cp;
						occ.CondicionPagoId = cp?.EntityId;

						return occ;
					},
					"orden_co_id,proveedor_id,moneda_id,tipo_doc_com_id,cond_pago_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public OrdenCompraComprobante SingleSearch(OrdenCompraComprobante entity)
		{
			try
			{
				List<OrdenCompraComprobante> ordenCompraComprobanteList = Search(entity);

				return ordenCompraComprobanteList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(OrdenCompraComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("fechaVencimiento", entity.FechaVencimiento);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoImpuesto", entity.MontoImpuesto);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
				parameters.AddParameter("registroReferencialId", entity.RegistroReferencialId);
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
		public int Update(OrdenCompraComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraComprobanteId", entity.OrdenCompraComprobanteId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("fechaVencimiento", entity.FechaVencimiento);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoImpuesto", entity.MontoImpuesto);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
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
		public void Delete(OrdenCompraComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraComprobanteId", entity.OrdenCompraComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
