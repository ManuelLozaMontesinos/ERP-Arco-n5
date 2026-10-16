using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class AlmacenVolumenProductoRepository : IAlmacenVolumenProductoRepository
	{
		private const string SP_PRE_NAME = "sp_alm_vol_prod";
		public AlmacenVolumenProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(AlmacenVolumenProducto)
					},
					x =>
					{
						AlmacenVolumenProducto avp = x[0] as AlmacenVolumenProducto;

						return avp;
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
		public List<AlmacenVolumenProducto> Search(AlmacenVolumenProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(AlmacenVolumenProducto),
						typeof(Producto)
					},
					x =>
					{
						AlmacenVolumenProducto avp = x[0] as AlmacenVolumenProducto;
						Producto p = x[1] as Producto;

						avp.Producto = p;
						avp.ProductoId = p.EntityId;

						return avp;
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
		public AlmacenVolumenProducto SingleSearch(AlmacenVolumenProducto entity)
		{
			try
			{
				List<AlmacenVolumenProducto> almacenVolumenProductoList = Search(entity);

				return almacenVolumenProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(AlmacenVolumenProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("stockMinimo", entity.StockMinimo);
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
		public int Update(AlmacenVolumenProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenVolumenProductoId", entity.AlmacenVolumenProductoId);
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("stockMinimo", entity.StockMinimo);
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
		public void Delete(AlmacenVolumenProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenVolumenProductoId", entity.AlmacenVolumenProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<AlmacenVolumenProducto> ListarPorSedeId(AlmacenVolumenProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodo", entity.Producto.Periodo);
			parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
			parameters.AddParameter("containsDescripcion", entity.Producto.Descripcion);
			parameters.AddParameter("tipoProducto", entity.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.TipoProducto.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_ListarPorSedeId"), parameters,
					new[]
					{
						typeof(AlmacenVolumenProducto),
						typeof(Producto),
						typeof(UnidadMedida)
					},
					x =>
					{
						AlmacenVolumenProducto avp = x[0] as AlmacenVolumenProducto;
						Producto p = x[1] as Producto;
						UnidadMedida um = x[2] as UnidadMedida;

						avp.Producto = p;
						avp.ProductoId = p.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;

						return avp;
					},
					"producto_id,unidad_medida_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarLote(List<AlmacenVolumenProducto> list)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				List<AlmacenVolumenProductoType> almacenVolumenProductoTypeList = new();
				almacenVolumenProductoTypeList = (from s in list
														 select new AlmacenVolumenProductoType
														 {
															 AlmacenVolumenProductoId = s.AlmacenVolumenProductoId,
															 EmpresaSedeId = s.EmpresaSedeId,
															 ProductoId = s.ProductoId,
															 StockMinimo = s.StockMinimo,
															 Estado = s.Estado,
															 UsuarioCreacionId = s.UsuarioCreacionId,
														 }).ToList();

				parameters.AddTableParameter("almacenVolumenProductoList", "dbo.AlmacenVolumenProductoType", almacenVolumenProductoTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<dynamic> ListarStockMinimo(AlmacenVolumenProducto entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("producto", entity?.Producto?.Codigo);

			try
			{
				List<dynamic> resultList = DbFactory.Query<dynamic>(DbCommand.Create(SP_PRE_NAME, "_listarStockMinimo"), parameters);

				return resultList;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
