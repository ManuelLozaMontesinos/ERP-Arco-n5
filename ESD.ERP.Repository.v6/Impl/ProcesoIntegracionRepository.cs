using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProcesoIntegracionRepository : IProcesoIntegracionRepository
	{
		private const string SP_PRE_NAME = "sp_proce_integra";
		public ProcesoIntegracion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProcesoIntegracion),
						typeof(TipoProcesoIntegracion)
					},
					x =>
					{
						ProcesoIntegracion pi = x[0] as ProcesoIntegracion;
						TipoProcesoIntegracion tpi = x[1] as TipoProcesoIntegracion;

						pi.TipoProcesoIntegracion = tpi;
						pi.TipoProcesoIntegracionId = tpi.EntityId;

						return pi;
					},
					"tipo_pro_integra_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ProcesoIntegracion> Search(ProcesoIntegracion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("modulo", entity.Modulo.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProcesoIntegracion)
					},
					x =>
					{
						ProcesoIntegracion pi = x[0] as ProcesoIntegracion;

						return pi;
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
		public ProcesoIntegracion SingleSearch(ProcesoIntegracion entity)
		{
			try
			{
				List<ProcesoIntegracion> proceIntegraList = Search(entity);

				return proceIntegraList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProcesoIntegracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("moduloId", entity.ModuloId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(ProcesoIntegracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proceIntegraId", entity.ProcesoIntegracionId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("nombre", entity.Nombre);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(ProcesoIntegracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proceIntegraId", entity.ProcesoIntegracionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void EjecutarJob(ProcesoIntegracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("procesoIntegracionId", entity.ProcesoIntegracionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_ejecutarJob"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
