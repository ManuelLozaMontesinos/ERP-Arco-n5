using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class NotificacionPushRepository : INotificacionPushRepository
	{
		private const string SP_PRE_NAME = "sp_notifi_push";
		public NotificacionPush GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(NotificacionPush)
					},
					x =>
					{
						NotificacionPush np = x[0] as NotificacionPush;

						return np;
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
		public List<NotificacionPush> Search(NotificacionPush entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("fecha", entity.Fecha);
			parameters.AddParameter("lectura", entity.Lectura);
			parameters.AddParameter("usuarioId", entity.UsuarioId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(NotificacionPush)
					},
					x =>
					{
						NotificacionPush np = x[0] as NotificacionPush;

						return np;
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
		public NotificacionPush SingleSearch(NotificacionPush entity)
		{
			try
			{
				List<NotificacionPush> notificacionPushList = Search(entity);

				return notificacionPushList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(NotificacionPush entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("titulo", entity.Titulo);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
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
		public int Update(NotificacionPush entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notificacionPushId", entity.NotificacionPushId);
				parameters.AddParameter("descripcion", entity.Descripcion);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(NotificacionPush entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notificacionPushId", entity.NotificacionPushId);

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