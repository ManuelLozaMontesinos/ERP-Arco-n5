using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProveedorProductoRepository : IProveedorProductoRepository
	{
		private const string SP_PRE_NAME = "sp_prove_prod";
		public ProveedorProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProveedorProducto)
					},
					x =>
					{
						ProveedorProducto pp = x[0] as ProveedorProducto;

						return pp;
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
		public List<ProveedorProducto> Search(ProveedorProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("proveedorId", entity.ProveedorId);
			parameters.AddParameter("producto", entity?.Producto?.Codigo);
			parameters.AddParameter("containsDescripcion", entity?.Producto?.Descripcion);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProveedorProducto),
						typeof(Producto),
						typeof(UnidadMedida),
						typeof(FamiliaProducto),
						typeof(TipoIgv)
					},
					x =>
					{
						ProveedorProducto pp = x[0] as ProveedorProducto;
						Producto p = x[1] as Producto;
						UnidadMedida um = x[2] as UnidadMedida;
						FamiliaProducto fp = x[3] as FamiliaProducto;
						TipoIgv ti = x[4] as TipoIgv;

						pp.Producto = p;
						pp.ProductoId = p.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						p.FamiliaProducto = fp;
						p.FamiliaProductoId = fp.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;

						return pp;
					},
					"producto_id,unidad_medida_id,familia_producto_id,tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ProveedorProducto SingleSearch(ProveedorProducto entity)
		{
			try
			{
				List<ProveedorProducto> proveProdList = Search(entity);

				return proveProdList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProveedorProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("codigoReferencial", entity.CodigoReferencial);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("masterPack", entity.MasterPack);
				parameters.AddParameter("volumen", entity.Volumen);
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
		public int Update(ProveedorProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorProductoId", entity.ProveedorProductoId);
				parameters.AddParameter("codigoReferencial", entity.CodigoReferencial);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("masterPack", entity.MasterPack);
				parameters.AddParameter("volumen", entity.Volumen);
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
		public void Delete(ProveedorProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorProductoId", entity.ProveedorProductoId);
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
