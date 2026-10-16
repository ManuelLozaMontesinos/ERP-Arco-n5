using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ListaPrecioProveedorRepository : IListaPrecioProveedorRepository
	{
		private const string SP_PRE_NAME = "sp_lista_p_prove";
		public ListaPrecioProveedor GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ListaPrecioProveedor)
					},
					x =>
					{
						ListaPrecioProveedor lpp = x[0] as ListaPrecioProveedor;

						return lpp;
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
		public List<ListaPrecioProveedor> Search(ListaPrecioProveedor entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ListaPrecioProveedor),
						typeof(GrupoProveedor)
					},
					x =>
					{
						ListaPrecioProveedor lpp = x[0] as ListaPrecioProveedor;
						GrupoProveedor gp = x[1] as GrupoProveedor;

						lpp.GrupoProveedor = gp;
						lpp.GrupoProveedorId = gp.EntityId;

						return lpp;
					},
					"grupo_prove_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ListaPrecioProveedor SingleSearch(ListaPrecioProveedor entity)
		{
			try
			{
				List<ListaPrecioProveedor> listaPrecioProveedorList = Search(entity);

				return listaPrecioProveedorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ListaPrecioProveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
				parameters.AddParameter("grupoProveedorId", entity.GrupoProveedorId);
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
		public int Update(ListaPrecioProveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProveedorId", entity.ListaPrecioProveedorId);
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
		public void Delete(ListaPrecioProveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProveedorId", entity.ListaPrecioProveedorId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ListaPrecioProveedor> ListarPorProveedorId(ListaPrecioProveedor entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
			parameters.AddParameter("proveedorId", entity.ProveedorId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPorProveedorId"), parameters,
					new[]
					{
						typeof(ListaPrecioProveedor),
						typeof(ListaPrecio)
					},
					x =>
					{
						ListaPrecioProveedor lpp = x[0] as ListaPrecioProveedor;
						ListaPrecio lp = x[1] as ListaPrecio;

						lpp.ListaPrecio = lp;
						lpp.ListaPrecioId = lp.EntityId;

						return lpp;
					},
					"lista_precio_id"
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
