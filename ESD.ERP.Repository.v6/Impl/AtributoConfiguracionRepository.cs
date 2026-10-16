using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class AtributoConfiguracionRepository : IAtributoConfiguracionRepository
	{
		private const string SP_PRE_NAME = "sp_atrib_config";
		public AtributoConfiguracion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(AtributoConfiguracion)
					},
					x =>
					{
						AtributoConfiguracion ac = x[0] as AtributoConfiguracion;

						return ac;
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
		public List<AtributoConfiguracion> Search(AtributoConfiguracion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(AtributoConfiguracion)
					},
					x =>
					{
						AtributoConfiguracion ac = x[0] as AtributoConfiguracion;

						return ac;
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
		public AtributoConfiguracion SingleSearch(AtributoConfiguracion entity)
		{
			try
			{
				List<AtributoConfiguracion> atributoConfiguracionList = Search(entity);

				return atributoConfiguracionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(AtributoConfiguracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("atributo", entity.Atributo);
				parameters.AddParameter("orden", entity.Orden);
				parameters.AddParameter("editable", entity.Editable);
				parameters.AddParameter("tieneBoton", entity.TieneBoton);
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
		public int Update(AtributoConfiguracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("atributoConfiguracionId", entity.AtributoConfiguracionId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("atributo", entity.Atributo);
				parameters.AddParameter("orden", entity.Orden);
				parameters.AddParameter("editable", entity.Editable);
				parameters.AddParameter("tieneBoton", entity.TieneBoton);
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
		public void Delete(AtributoConfiguracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("atributoConfiguracionId", entity.AtributoConfiguracionId);
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
