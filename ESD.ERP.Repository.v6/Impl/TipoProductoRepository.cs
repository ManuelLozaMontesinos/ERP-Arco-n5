using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoProductoRepository : ITipoProductoRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_producto";
		public TipoProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoProducto)
					},
					x =>
					{
						TipoProducto tp = x[0] as TipoProducto;

						return tp;
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
		public List<TipoProducto> Search(TipoProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("tipoProducto", entity.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoProducto)
					},
					x =>
					{
						TipoProducto tp = x[0] as TipoProducto;

						return tp;
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
		public TipoProducto SingleSearch(TipoProducto entity)
		{
			try
			{
				List<TipoProducto> tipoProductoList = Search(entity);

				return tipoProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoProducto", entity.Codigo);
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("correlativo", entity.Correlativo);
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
		public int Update(TipoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoProductoId", entity.TipoProductoId);
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("tipoProducto", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("correlativo", entity.Correlativo);
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
		public void Delete(TipoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoProductoId", entity.TipoProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TipoProducto ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(TipoProducto)
					},
					x =>
					{
						TipoProducto tp = x[0] as TipoProducto;

						return tp;
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
