using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CotizacionRepository : ICotizacionRepository
	{
		private const string SP_PRE_NAME = "sp_cotiza";
		public Cotizacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Cotizacion),
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(Proveedor),
						typeof(TipoDocumentoIdentidad),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(ImpuestoTasa),
						typeof(EstadoCotizacion)
					},
					x =>
					{
						Cotizacion c = x[0] as Cotizacion;
						PeriodoEmpresaSede pes = x[1] as PeriodoEmpresaSede;
						EmpresaSede es = x[2] as EmpresaSede;
						Proveedor p = x[3] as Proveedor;
						TipoDocumentoIdentidad tdi = x[4] as TipoDocumentoIdentidad;
						Vendedor v = x[5] as Vendedor;
						Proveedor p2 = x[6] as Proveedor;
						Moneda m = x[7] as Moneda;
						ImpuestoTasa it = x[8] as ImpuestoTasa;
						EstadoCotizacion ec = x[9] as EstadoCotizacion;

						c.PeriodoEmpresaSede = pes;
						c.PeriodoEmpresaSedeId = pes.EntityId;
						pes.EmpresaSede = es;
						pes.EmpresaSedeId = es.EmpresaSedeId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						p.TipoDocumentoIdentidad = tdi;
						p.TipoDocumentoIdentidadId = tdi.EntityId;
						c.Vendedor = v;
						c.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						c.ImpuestoTasa = it;
						c.ImpuestoTasaId = it.EntityId;
						c.EstadoCotizacion = ec;
						c.EstadoCotizacionId = ec.EntityId;

						return c;
					},
					"periodo_emp_sede_id,empresa_sede_id,proveedor_id,tipo_doc_iden_id,vendedor_id,proveedor_id," +
					"moneda_id,impuesto_tasa_id,estado_cotiza_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
       public List<Cotizacion> Search(Cotizacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("cotizacion", entity.Codigo);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("containsVendedor", entity.Vendedor?.Proveedor?.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaDesde);
			parameters.AddParameter("esPedido", entity.EsPedido);
			parameters.AddParameter("estadoCotizacionId", entity.EstadoCotizacionId);
			parameters.AddParameter("estadoCotizacionList", entity.EstadoCotizacionList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Cotizacion),
						typeof(Proveedor),
						typeof(TipoNaturalezaJuridica),
						typeof(TipoDocumentoIdentidad),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(EstadoCotizacion)
					},
					x =>
					{
						Cotizacion c = x[0] as Cotizacion;
						Proveedor p = x[1] as Proveedor;
						TipoNaturalezaJuridica tnj = x[2] as TipoNaturalezaJuridica;
						TipoDocumentoIdentidad tdi = x[3] as TipoDocumentoIdentidad;
						Vendedor v = x[4] as Vendedor;
						Proveedor p2 = x[5] as Proveedor;
						Moneda m = x[6] as Moneda;
						EstadoCotizacion ec = x[7] as EstadoCotizacion;

						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						p.TipoNaturalezaJuridica = tnj;
						p.TipoNaturalezaJuridicaId = tnj.EntityId;
						p.TipoDocumentoIdentidad = tdi;
						p.TipoDocumentoIdentidadId = tdi.EntityId;
						c.Vendedor = v;
						c.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						c.EstadoCotizacion = ec;
						c.EstadoCotizacionId = ec.EntityId;

						return c;
					},
					"proveedor_id,tipo_nat_juri_id,tipo_doc_iden_id,vendedor_id,proveedor_id,moneda_id,estado_cotiza_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public Cotizacion SingleSearch(Cotizacion entity)
		{
			try
			{
				List<Cotizacion> cotizaList = Search(entity);

				return cotizaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public int Insert(Cotizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("cotizacion", entity.Codigo);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTipoCambio", entity.MontoTipoCambio);
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
				parameters.AddParameter("tipoCondicionPagoId", entity.TipoCondicionPagoId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("porcentajeDescuento", entity.PorcentajeDescuento);
				parameters.AddParameter("montoDescuento", entity.MontoDescuento);
				parameters.AddParameter("montoDescuentoItem", entity.MontoDescuentoItem);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoAnticipo", entity.MontoAnticipo);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("mesContableId", entity.MesContableId);
				parameters.AddParameter("esPedido", entity.EsPedido);
				parameters.AddParameter("enviadoProduccion", entity.EnviadoProduccion);
				parameters.AddParameter("estadoCotizacionId", entity.EstadoCotizacionId);
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
		public int Update(Cotizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("tipoCondicionPagoId", entity.TipoCondicionPagoId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("mesContableId", entity.MesContableId);
				parameters.AddParameter("esPedido", entity.EsPedido);
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
		public void Delete(Cotizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("estadoCotizacionId", entity.EstadoCotizacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarMonto(Cotizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMonto"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstado(Cotizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("estadoCotizacionId", entity.EstadoCotizacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarMontoAnticipo(Cotizacion entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMontoAnticipo"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEnviadoProduccion(Cotizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("enviadoProduccion", entity.EnviadoProduccion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEnviadoProduccion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		/*public void ActualizarAnticipo(Cotizacion entity) 
		{
			Dictionary <string, object> parameters = new();

			try
			{
				parameters.AddParameter("")
			}
			catch (Exception e)
			{
				Logger.Error (e.ToString());
				throw;
			}
		}*/

    }
}
