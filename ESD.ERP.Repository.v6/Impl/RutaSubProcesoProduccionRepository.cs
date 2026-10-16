using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class RutaSubProcesoProduccionRepository : IRutaSubProcesoProduccionRepository
	{
		private const string SP_PRE_NAME = "sp_ruta_p_s_p_p";
		public RutaSubProcesoProduccion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(RutaSubProcesoProduccion)
					},
					x =>
					{
						RutaSubProcesoProduccion pspp = x[0] as RutaSubProcesoProduccion;

						return pspp;
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
		public List<RutaSubProcesoProduccion> Search(RutaSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("rutaProduccionId", entity.RutaProduccionId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(RutaSubProcesoProduccion),
						typeof(SubProcesoProduccion),
						typeof(ProcesoProduccion)
					},
					x =>
					{
						RutaSubProcesoProduccion rspp = x[0] as RutaSubProcesoProduccion;
						SubProcesoProduccion spp = x[1] as SubProcesoProduccion;
						ProcesoProduccion pp = x[2] as ProcesoProduccion;

						rspp.SubProcesoProduccion = spp;
						rspp.SubProcesoProduccionId = spp.EntityId;
						spp.ProcesoProduccion = pp;
						spp.ProcesoProduccionId = pp.EntityId;

						return rspp;
					},
					"sub_proce_p_id,proce_prod_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public RutaSubProcesoProduccion SingleSearch(RutaSubProcesoProduccion entity)
		{
			try
			{
				List<RutaSubProcesoProduccion> productoSubProcesoProduccionList = Search(entity);

				return productoSubProcesoProduccionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(RutaSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rutaProduccionId", entity.RutaProduccionId);
				parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
				parameters.AddParameter("orden", entity.Orden);
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
		public int Update(RutaSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoSubProcesoProduccionId", entity.RutaSubProcesoProduccionId);
				parameters.AddParameter("orden", entity.Orden);
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
		public void Delete(RutaSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoSubProcesoProduccionId", entity.RutaSubProcesoProduccionId);
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
