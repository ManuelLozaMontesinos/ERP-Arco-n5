using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EstadoPrestamoRepository : IEstadoPrestamoRepository
	{
		private const string SP_PRE_NAME = "sp_estado_presta";
		public EstadoPrestamo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EstadoPrestamo)
					},
					x =>
					{
						EstadoPrestamo ep = x[0] as EstadoPrestamo;

						return ep;
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
		public List<EstadoPrestamo> Search(EstadoPrestamo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EstadoPrestamo)
					},
					x =>
					{
						EstadoPrestamo ep = x[0] as EstadoPrestamo;

						return ep;
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
		public EstadoPrestamo SingleSearch(EstadoPrestamo entity)
		{
			try
			{
				List<EstadoPrestamo> estadoPrestamoList = Search(entity);

				return estadoPrestamoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EstadoPrestamo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoPrestamo", entity.Codigo);
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
		public int Update(EstadoPrestamo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoPrestamoId", entity.EstadoPrestamoId);
				parameters.AddParameter("estadoPrestamo", entity.Codigo);
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
		public void Delete(EstadoPrestamo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoPrestamoId", entity.EstadoPrestamoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public EstadoPrestamo ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(EstadoPrestamo)
					},
					x =>
					{
						EstadoPrestamo ep = x[0] as EstadoPrestamo;

						return ep;
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
