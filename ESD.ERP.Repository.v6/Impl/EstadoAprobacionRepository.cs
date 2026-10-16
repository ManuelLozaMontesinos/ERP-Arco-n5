using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EstadoAprobacionRepository : IEstadoAprobacionRepository
	{
		private const string SP_PRE_NAME = "sp_estado_aproba";
		public EstadoAprobacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EstadoAprobacion)
					},
					x =>
					{
						EstadoAprobacion ec = x[0] as EstadoAprobacion;

						return ec;
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
		public List<EstadoAprobacion> Search(EstadoAprobacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EstadoAprobacion)
					},
					x =>
					{
						EstadoAprobacion ec = x[0] as EstadoAprobacion;

						return ec;
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
		public EstadoAprobacion SingleSearch(EstadoAprobacion entity)
		{
			try
			{
				List<EstadoAprobacion> estadoAprobaList = Search(entity);

				return estadoAprobaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EstadoAprobacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoAprobacion", entity.Codigo);
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
		public int Update(EstadoAprobacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("estadoAprobacion", entity.Codigo);
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
		public void Delete(EstadoAprobacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public EstadoAprobacion ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(EstadoAprobacion)
					},
					x =>
					{
						EstadoAprobacion ec = x[0] as EstadoAprobacion;

						return ec;
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
