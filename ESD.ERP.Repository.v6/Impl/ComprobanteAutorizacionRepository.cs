using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ComprobanteAutorizacionRepository : IComprobanteAutorizacionRepository
	{
		private const string SP_PRE_NAME = "sp_comprob_autoriza";
		public ComprobanteAutorizacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ComprobanteAutorizacion),
						typeof(TipoAutorizacion)
					},
					x =>
					{
						ComprobanteAutorizacion ca = x[0] as ComprobanteAutorizacion;
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
		public List<ComprobanteAutorizacion> Search(ComprobanteAutorizacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.Comprobante.PeriodoEmpresaSede.EmpresaSede.EmpresaId);
			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("periodoEmpresaSedeId", entity.Comprobante?.PeriodoEmpresaSedeId);
			parameters.AddParameter("containsRazonSocial", entity.Comprobante?.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Comprobante?.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("containsVendedor", entity.Comprobante?.Vendedor?.Proveedor?.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.Comprobante?.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.Comprobante?.FechaDesde);
			parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
			parameters.AddParameter("estadoAprobacion", entity?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ComprobanteAutorizacion),
						typeof(TipoAutorizacion),
						typeof(Comprobante),
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(TipoDocumentoComercial),
						typeof(Proveedor),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(Usuario),
						typeof(EstadoAprobacion)
					},
					x =>
					{
						ComprobanteAutorizacion ca = x[0] as ComprobanteAutorizacion;
						TipoAutorizacion ta = x[1] as TipoAutorizacion;
						Comprobante c = x[2] as Comprobante;
						PeriodoEmpresaSede pes = x[3] as PeriodoEmpresaSede;
						EmpresaSede es = x[4] as EmpresaSede;
						TipoDocumentoComercial tdc = x[5] as TipoDocumentoComercial;
						Proveedor p = x[6] as Proveedor;
						Vendedor v = x[7] as Vendedor;
						Proveedor p2 = x[8] as Proveedor;
						Moneda m = x[9] as Moneda;
						Usuario u = x[10] as Usuario;
						EstadoAprobacion ea = x[11] as EstadoAprobacion;

						ca.TipoAutorizacion = ta;
						ca.TipoAutorizacionId = ta.EntityId;
						ca.Comprobante = c;
						ca.ComprobanteId = c.EntityId;
						c.PeriodoEmpresaSede = pes;
						c.PeriodoEmpresaSedeId = pes.EntityId;
						pes.EmpresaSede = es;
						pes.EmpresaSedeId = es.EntityId;
						c.TipoDocumentoComercial = tdc;
						c.TipoDocumentoComercialId = tdc.EntityId;
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
					"tipo_autoriza_id,comprobante_id,periodo_emp_sede_id,empresa_sede_id,tipo_doc_com_id,proveedor_id,vendedor_id," +
					"proveedor_id,moneda_id,usuario_id,estado_aproba_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ComprobanteAutorizacion SingleSearch(ComprobanteAutorizacion entity)
		{
			try
			{
				List<ComprobanteAutorizacion> comprobanteAutorizacionList = Search(entity);

				return comprobanteAutorizacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ComprobanteAutorizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoAutorizacionId", entity.TipoAutorizacionId);
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
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
		public int Update(ComprobanteAutorizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteAutorizacionId", entity.ComprobanteAutorizacionId);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("fechaAutorizacion", entity.FechaAutorizacion);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("estado", entity.Estado);
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
		public void Delete(ComprobanteAutorizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ComprobanteAutorizacionId", entity.ComprobanteAutorizacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ComprobanteAutorizacion> ListarPorTipoAutorizacionIdList(ComprobanteAutorizacion entity)
		{

			Dictionary<string, object> parameters = new();

			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("tipoAutorizacionIdList", entity.TipoAutorizacionIdList);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPorTipoAutorizacionIdList"), parameters,
					new[]
					{
						typeof(ComprobanteAutorizacion),
						typeof(TipoAutorizacion),
						typeof(Usuario),
						typeof(EstadoAprobacion)
					},
					x =>
					{
						ComprobanteAutorizacion ca = x[0] as ComprobanteAutorizacion;
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
