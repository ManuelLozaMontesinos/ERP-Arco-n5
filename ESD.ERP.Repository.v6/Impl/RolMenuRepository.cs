using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class RolMenuRepository : IRolMenuRepository
	{
		private const string SP_PRE_NAME = "sp_rol_menu";
		public RolMenu GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(RolMenu)
					},
					x =>
					{
						RolMenu rm = x[0] as RolMenu;

						return rm;
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
		public List<RolMenu> Search(RolMenu entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("rolList", entity.RolCodigoList);
			parameters.AddParameter("lectura", entity.Lectura);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(RolMenu),
						typeof(Menu)
					},
					x =>
					{
						RolMenu rm = x[0] as RolMenu;
						Menu m = x[1] as Menu;

						rm.Menu = m;
						rm.MenuId = m.EntityId;

						return rm;
					},
					"menu_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public RolMenu SingleSearch(RolMenu entity)
		{
			try
			{
				List<RolMenu> rolMenuList = Search(entity);

				return rolMenuList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(RolMenu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rolId", entity.RolId);
				parameters.AddParameter("menuId", entity.MenuId);
				parameters.AddParameter("lectura", entity.Lectura);
				parameters.AddParameter("escritura", entity.Escritura);
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
		public int Update(RolMenu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rolMenuId", entity.RolMenuId);
				parameters.AddParameter("rolId", entity.RolId);
				parameters.AddParameter("menuId", entity.MenuId);
				parameters.AddParameter("lectura", entity.Lectura);
				parameters.AddParameter("escritura", entity.Escritura);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(RolMenu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rolMenuId", entity.RolMenuId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<RolMenu> ListarPorRolId(RolMenu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rolId", entity.RolId);

				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPorRolId"), parameters,
					new[]
					{
						typeof(RolMenu),
						typeof(Menu)
					},
					x =>
					{
						RolMenu rm = x[0] as RolMenu;
						Menu m = x[1] as Menu;

						rm.Menu = m;
						rm.MenuId = m.EntityId;

						return rm;
					},
					"menu_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(RolMenu entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rolId", entity.RolId);
				List<RolMenuType> rolMenuType = new List<RolMenuType>();
				rolMenuType = (from s in entity.RolMenuList
					select new RolMenuType
					{
						RolMenuId = s.RolMenuId,
						RolId = s.RolId,
						MenuId = s.MenuId,
						Lectura = s.Lectura,
						Escritura = s.Escritura
					}).ToList();

				parameters.AddTableParameter("rolMenuList", "dbo.RolMenuType", rolMenuType);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
