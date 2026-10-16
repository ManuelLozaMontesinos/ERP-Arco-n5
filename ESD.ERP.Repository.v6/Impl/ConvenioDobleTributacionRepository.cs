using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ConvenioDobleTributacionRepository : IConvenioDobleTributacionRepository
	{
		private const string SP_PRE_NAME = "sp_conve_d_trib";
		public ConvenioDobleTributacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ConvenioDobleTributacion)
					},
					x =>
					{
						ConvenioDobleTributacion cdt = x[0] as ConvenioDobleTributacion;

						return cdt;
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
		public List<ConvenioDobleTributacion> Search(ConvenioDobleTributacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ConvenioDobleTributacion)
					},
					x =>
					{
						ConvenioDobleTributacion cdt = x[0] as ConvenioDobleTributacion;

						return cdt;
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
		public ConvenioDobleTributacion SingleSearch(ConvenioDobleTributacion entity)
		{
			try
			{
				List<ConvenioDobleTributacion> convenioDobleTributacionList = Search(entity);

				return convenioDobleTributacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ConvenioDobleTributacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("convenioDobleTributacion", entity.Codigo);
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
		public int Update(ConvenioDobleTributacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("convenioDobleTributacionId", entity.ConvenioDobleTributacionId);
				parameters.AddParameter("convenioDobleTributacion", entity.Codigo);
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
		public void Delete(ConvenioDobleTributacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("convenioDobleTributacionId", entity.ConvenioDobleTributacionId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ConvenioDobleTributacion ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(ConvenioDobleTributacion)
					},
					x =>
					{
						ConvenioDobleTributacion cdt = x[0] as ConvenioDobleTributacion;

						return cdt;
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
