using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CotizacionAutorizacionRepository : ICotizacionAutorizacionRepository
	{
		private const string SP_PRE_NAME = "sp_cotiza_autoriza";
		public CotizacionAutorizacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(CotizacionAutorizacion),
						typeof(TipoAutorizacion)
					},
					x =>
					{
						CotizacionAutorizacion ca = x[0] as CotizacionAutorizacion;
						TipoAutorizacion ta = x[1] as TipoAutorizacion;

						ca.TipoAutorizacion = ta;
						ca.TipoAutorizacionId = ta.EntityId;

						return ca;
					},
					"tipo_autoriza_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<CotizacionAutorizacion> Search(CotizacionAutorizacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.Cotizacion.PeriodoEmpresaSede.EmpresaSede.EmpresaId);
			parameters.AddParameter("cotizacionId", entity.CotizacionId);
			parameters.AddParameter("cotizacion", entity.Cotizacion?.Codigo);
			parameters.AddParameter("periodoEmpresaSedeId", entity.Cotizacion?.PeriodoEmpresaSedeId);
			parameters.AddParameter("containsRazonSocial", entity.Cotizacion?.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Cotizacion?.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("containsVendedor", entity.Cotizacion?.Vendedor?.Proveedor?.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.Cotizacion?.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.Cotizacion?.FechaDesde);
			parameters.AddParameter("tipoAutorizacion", entity?.TipoAutorizacion?.Codigo);
			parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
			parameters.AddParameter("estadoAprobacion", entity?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(CotizacionAutorizacion),
						typeof(TipoAutorizacion),
						typeof(Cotizacion),
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(Proveedor),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(Usuario),
						typeof(EstadoAprobacion)
					},
					x =>
					{
						CotizacionAutorizacion ca = x[0] as CotizacionAutorizacion;
						TipoAutorizacion ta = x[1] as TipoAutorizacion;
						Cotizacion c = x[2] as Cotizacion;
						PeriodoEmpresaSede pes = x[3] as PeriodoEmpresaSede;
						EmpresaSede es = x[4] as EmpresaSede;
						Proveedor p = x[5] as Proveedor;
						Vendedor v = x[6] as Vendedor;
						Proveedor p2 = x[7] as Proveedor;
						Moneda m = x[8] as Moneda;
						Usuario u = x[9] as Usuario;
						EstadoAprobacion ea = x[10] as EstadoAprobacion;

						ca.TipoAutorizacion = ta;
						ca.TipoAutorizacionId = ta.EntityId;
						ca.Cotizacion = c;
						ca.CotizacionId = c.EntityId;
						c.PeriodoEmpresaSede = pes;
						c.PeriodoEmpresaSedeId = pes.EntityId;
						pes.EmpresaSede = es;
						pes.EmpresaSedeId = es.EntityId;
						c.Proveedor = p;
						c.ProveedorId = p.EntityId;
						c.Vendedor = v;
						c.VendedorId = v.EntityId;
						v.Proveedor = p2;
						v.ProveedorId = p2.EntityId;
						c.Moneda = m;
						c.MonedaId = m.EntityId;
						ca.Usuario = u;
						ca.UsuarioId = u?.EntityId;
						ca.EstadoAprobacion = ea;
						ca.EstadoAprobacionId = ea.EstadoAprobacionId;

						return ca;
					},
					"tipo_autoriza_id,cotiza_id,periodo_emp_sede_id,empresa_sede_id,proveedor_id,vendedor_id," +
					"proveedor_id,moneda_id,usuario_id,estado_aproba_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public CotizacionAutorizacion SingleSearch(CotizacionAutorizacion entity)
		{
			try
			{
				List<CotizacionAutorizacion> cotizacionAutorizacionList = Search(entity);

				return cotizacionAutorizacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(CotizacionAutorizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoAutorizacionId", entity.TipoAutorizacionId);
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				int usuarioAccionId = entity.UsuarioCreacionId > 0 ? entity.UsuarioCreacionId : (entity.UsuarioModificacionId ?? (entity.UsuarioId ?? 0));
				parameters.AddParameter("usuarioAccionId", usuarioAccionId);
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
		public int Update(CotizacionAutorizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionAutorizacionId", entity.CotizacionAutorizacionId);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("fechaAutorizacion", entity.FechaAutorizacion);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("estado", entity.Estado);
				int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : (entity.UsuarioCreacionId > 0 ? entity.UsuarioCreacionId : (entity.UsuarioId ?? 0));
				parameters.AddParameter("usuarioAccionId", usuarioAccionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(CotizacionAutorizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("CotizacionAutorizacionId", entity.CotizacionAutorizacionId);
				int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : (entity.UsuarioCreacionId > 0 ? entity.UsuarioCreacionId : (entity.UsuarioId ?? 0));
				parameters.AddParameter("usuarioAccionId", usuarioAccionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void UpdateAuto(CotizacionAutorizacion entity)
		{
            Dictionary<string, object> parameters = new();

            try
            {
				parameters.AddParameter("cotizaId", entity.CotizacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_updateAuto"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}

        public List<CotizacionAutorizacion> ListarPorTipoAutorizacionIdList(CotizacionAutorizacion entity)
		{

			Dictionary<string, object> parameters = new();

			parameters.AddParameter("cotizacionId", entity.CotizacionId);
			parameters.AddParameter("tipoAutorizacionIdList", entity.TipoAutorizacionIdList);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPorTipoAutorizacionIdList"), parameters,
					new[]
					{
						typeof(CotizacionAutorizacion),
						typeof(TipoAutorizacion),
						typeof(Usuario),
						typeof(EstadoAprobacion)
					},
					x =>
					{
						CotizacionAutorizacion ca = x[0] as CotizacionAutorizacion;
						TipoAutorizacion ta = x[1] as TipoAutorizacion;
						Usuario u = x[2] as Usuario;
						EstadoAprobacion ea = x[3] as EstadoAprobacion;

						ca.TipoAutorizacion = ta;
						ca.TipoAutorizacionId = ta.EntityId;
						ca.Usuario = u;
						ca.UsuarioId = u?.EntityId;
						ca.EstadoAprobacion = ea;
						ca.EstadoAprobacionId = ea.EntityId;

						return ca;
					},
					"tipo_autoriza_id,usuario_id,estado_aproba_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
