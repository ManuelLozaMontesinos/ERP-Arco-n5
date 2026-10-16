using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class GrupoProductoRepository : IGrupoProductoRepository
	{
		private const string SP_PRE_NAME = "sp_grupo_producto";
		public GrupoProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(GrupoProducto)
					},
					x =>
					{
						GrupoProducto gp = x[0] as GrupoProducto;

						return gp;
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
		public List<GrupoProducto> Search(GrupoProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("notGrupoProductoId", entity.NotGrupoProductoId);
			parameters.AddParameter("empresaId", entity.TipoProducto?.EmpresaId);
			parameters.AddParameter("tipoProductoId", entity.TipoProductoId);
			parameters.AddParameter("grupoProducto", entity.Codigo);
			parameters.AddParameter("esCombo", entity.EsCombo);
			parameters.AddParameter("periodo", entity.Periodo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(GrupoProducto),
						typeof(TipoProducto)
					},
					x =>
					{
						GrupoProducto gp = x[0] as GrupoProducto;
						TipoProducto tp = x[1] as TipoProducto;

						gp.TipoProductoId = tp.EntityId;
						gp.TipoProducto = tp;

						return gp;
					},
					"tipo_producto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public GrupoProducto SingleSearch(GrupoProducto entity)
		{
			try
			{
				List<GrupoProducto> grupoProductoList = Search(entity);

				return grupoProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(GrupoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoProductoId", entity.TipoProductoId);
				parameters.AddParameter("grupoProducto", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("correlativo", entity.Correlativo);
				parameters.AddParameter("usarIncrementoFraccion", entity.UsarIncrementoFraccion);
				parameters.AddParameter("esCombo", entity.EsCombo);
				parameters.AddParameter("esInsumo", entity.EsInsumo);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
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
		public int Update(GrupoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoProductoId", entity.GrupoProductoId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("usarIncrementoFraccion", entity.UsarIncrementoFraccion);
				parameters.AddParameter("esCombo", entity.EsCombo);
				parameters.AddParameter("esInsumo", entity.EsInsumo);
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
		public void Delete(GrupoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoProductoId", entity.GrupoProductoId);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
