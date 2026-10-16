using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EstadoOrdenProduccionSubProcesoRepository : IEstadoOrdenProduccionSubProcesoRepository
	{
		private const string SP_PRE_NAME = "sp_estado_o_p_s_p";
		public EstadoOrdenProduccionSubProceso GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EstadoOrdenProduccionSubProceso)
					},
					x =>
					{
						EstadoOrdenProduccionSubProceso eopsp = x[0] as EstadoOrdenProduccionSubProceso;

						return eopsp;
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
		public List<EstadoOrdenProduccionSubProceso> Search(EstadoOrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EstadoOrdenProduccionSubProceso)
					},
					x =>
					{
						EstadoOrdenProduccionSubProceso eopsp = x[0] as EstadoOrdenProduccionSubProceso;

						return eopsp;
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
		public EstadoOrdenProduccionSubProceso SingleSearch(EstadoOrdenProduccionSubProceso entity)
		{
			try
			{
				List<EstadoOrdenProduccionSubProceso> estadoOrdenProduccionSubProcesoList = Search(entity);

				return estadoOrdenProduccionSubProcesoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EstadoOrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoOrdenProduccionSubProceso", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(EstadoOrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoOrdenProduccionSubProcesoId", entity.EstadoOrdenProduccionSubProcesoId);
				parameters.AddParameter("estadoOrdenProduccionSubProceso", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public void Delete(EstadoOrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoOrdenProduccionSubProcesoId", entity.EstadoOrdenProduccionSubProcesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public EstadoOrdenProduccionSubProceso ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(EstadoOrdenProduccionSubProceso)
					},
					x =>
					{
						EstadoOrdenProduccionSubProceso eopsp = x[0] as EstadoOrdenProduccionSubProceso;

						return eopsp;
					},
					string.Empty
				).FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
