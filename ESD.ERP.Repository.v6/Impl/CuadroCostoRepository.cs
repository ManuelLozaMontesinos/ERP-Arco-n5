using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CuadroCostoRepository : ICuadroCostoRepository
	{
		private const string SP_PRE_NAME = "sp_cuadro_costo";
		public CuadroCosto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(CuadroCosto)
					},
					x =>
					{
						CuadroCosto d = x[0] as CuadroCosto;

						return d;
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
		public List<CuadroCosto> Search(CuadroCosto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
			parameters.AddParameter("mesContableId", entity.MesContableId);
			parameters.AddParameter("producto", entity.Producto.Codigo);
			parameters.AddParameter("containsDescripcion", entity.Producto.Descripcion);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(CuadroCosto),
						typeof(Producto)
					},
					x =>
					{
						CuadroCosto cc = x[0] as CuadroCosto;
						Producto p = x[1] as Producto;

						cc.Producto = p;
						cc.ProductoId = p.EntityId;

						return cc;
					},
					"producto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public CuadroCosto SingleSearch(CuadroCosto entity)
		{
			try
			{
				List<CuadroCosto> diaList = Search(entity);

				return diaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(CuadroCosto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
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
		public int Update(CuadroCosto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cuadroCostoId", entity.CuadroCostoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(CuadroCosto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cuadroCostoId", entity.CuadroCostoId);

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
