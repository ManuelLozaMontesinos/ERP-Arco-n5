using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MenuRepository : IMenuRepository
	{
		private const string SP_PRE_NAME = "sp_menu";
		public Menu GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Menu)
					},
					x =>
					{
						Menu m = x[0] as Menu;

						return m;
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
		public List<Menu> Search(Menu entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Menu)
					},
					x =>
					{
						Menu m = x[0] as Menu;

						return m;
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
		public Menu SingleSearch(Menu entity)
		{
			try
			{
				List<Menu> menuList = Search(entity);

				return menuList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Menu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("area", entity.Area);
				parameters.AddParameter("url", entity.Url);
				parameters.AddParameter("icono", entity.Icono);
				parameters.AddParameter("menuPadreId", entity.MenuPadreId);
				parameters.AddParameter("orden", entity.Orden);
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
		public int Update(Menu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("menuId", entity.MenuId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("area", entity.Area);
				parameters.AddParameter("url", entity.Url);
				parameters.AddParameter("icono", entity.Icono);
				parameters.AddParameter("menuPadreId", entity.MenuPadreId);
				parameters.AddParameter("orden", entity.Orden);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(Menu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("menuId", entity.MenuId);
				parameters.AddParameter("estado", entity.Estado);

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
