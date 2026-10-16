using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class AlmacenTransaccionConfiguracionRepository : IAlmacenTransaccionConfiguracionRepository
	{
		private const string SP_PRE_NAME = "sp_alm_trans_config";
		public AlmacenTransaccionConfiguracion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(AlmacenTransaccionConfiguracion)
					},
					x =>
					{
						AlmacenTransaccionConfiguracion atc = x[0] as AlmacenTransaccionConfiguracion;

						return atc;
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
		public List<AlmacenTransaccionConfiguracion> Search(AlmacenTransaccionConfiguracion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("almacenTransaccionId", entity.AlmacenTransaccionId);
			parameters.AddParameter("visible", entity.Visible);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(AlmacenTransaccionConfiguracion),
						typeof(AtributoConfiguracion)
					},
					x =>
					{
						AlmacenTransaccionConfiguracion atc = x[0] as AlmacenTransaccionConfiguracion;
						AtributoConfiguracion ac = x[1] as AtributoConfiguracion;

						atc.AtributoConfiguracion = ac;
						atc.AtributoConfiguracionId = ac.EntityId;

						return atc;
					},
					"atrib_config_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public AlmacenTransaccionConfiguracion SingleSearch(AlmacenTransaccionConfiguracion entity)
		{
			try
			{
				List<AlmacenTransaccionConfiguracion> almacenTransaccionConfiguracionList = Search(entity);

				return almacenTransaccionConfiguracionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(AlmacenTransaccionConfiguracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenTransaccionId", entity.AlmacenTransaccionId);
				parameters.AddParameter("atributoConfiguracionId", entity.AtributoConfiguracionId);
				parameters.AddParameter("visible", entity.Visible);
				parameters.AddParameter("requerido", entity.Requerido);
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
		public int Update(AlmacenTransaccionConfiguracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenTransaccionConfiguracionId", entity.AlmacenTransaccionConfiguracionId);
				parameters.AddParameter("visible", entity.Visible);
				parameters.AddParameter("requerido", entity.Requerido);
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
		public void Delete(AlmacenTransaccionConfiguracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenTransaccionConfiguracionId", entity.AlmacenTransaccionConfiguracionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<AlmacenTransaccionConfiguracion> ListarPorPorAlmacenTransaccionId(AlmacenTransaccionConfiguracion entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("almacenTransaccionId", entity.AlmacenTransaccionId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPorAlmacenTransaccionId"), parameters,
					new[]
					{
						typeof(AlmacenTransaccionConfiguracion),
						typeof(AtributoConfiguracion)
					},
					x =>
					{
						AlmacenTransaccionConfiguracion cbi = x[0] as AlmacenTransaccionConfiguracion;
						AtributoConfiguracion ac = x[1] as AtributoConfiguracion;

						cbi.AtributoConfiguracionId = ac.EntityId;
						cbi.AtributoConfiguracion = ac;

						return cbi;
					},
					"atrib_config_id"
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
