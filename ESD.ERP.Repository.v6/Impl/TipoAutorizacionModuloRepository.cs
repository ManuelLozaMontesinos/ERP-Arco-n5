using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoAutorizacionModuloRepository : ITipoAutorizacionModuloRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_auto_modulo";
		public TipoAutorizacionModulo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoAutorizacionModulo)
					},
					x =>
					{
						TipoAutorizacionModulo tam = x[0] as TipoAutorizacionModulo;

						return tam;
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
		public List<TipoAutorizacionModulo> Search(TipoAutorizacionModulo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);
			parameters.AddParameter("opcionModuloId", entity.OpcionModuloId);
			parameters.AddParameter("opcionModulo", entity.OpcionModulo?.Codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoAutorizacionModulo),
						typeof(TipoAutorizacion)
					},
					x =>
					{
						TipoAutorizacionModulo tam = x[0] as TipoAutorizacionModulo;
						TipoAutorizacion ta = x[1] as TipoAutorizacion;

						tam.TipoAutorizacion = ta;
						tam.TipoAutorizacionId = ta.EntityId;

						return tam;
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
		public TipoAutorizacionModulo SingleSearch(TipoAutorizacionModulo entity)
		{
			try
			{
				List<TipoAutorizacionModulo> tipoAutorizacionModuloList = Search(entity);

				return tipoAutorizacionModuloList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoAutorizacionModulo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("opcionModuloId", entity.OpcionModuloId);
				parameters.AddParameter("tipoAutorizacionId", entity.TipoAutorizacionId);
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
		public int Update(TipoAutorizacionModulo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoAutorizacionModuloId", entity.TipoAutorizacionModuloId);
				parameters.AddParameter("opcionModuloId", entity.OpcionModuloId);
				parameters.AddParameter("tipoAutorizacionId", entity.TipoAutorizacionId);
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
		public void Delete(TipoAutorizacionModulo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoAutorizacionModuloId", entity.TipoAutorizacionModuloId);
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
