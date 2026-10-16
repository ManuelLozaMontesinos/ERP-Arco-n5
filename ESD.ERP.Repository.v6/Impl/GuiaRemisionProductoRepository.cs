using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class GuiaRemisionProductoRepository : IGuiaRemisionProductoRepository
	{
		private const string SP_PRE_NAME = "sp_guia_r_prod";
		public GuiaRemisionProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(GuiaRemisionProducto)
					},
					x =>
					{
						GuiaRemisionProducto grp = x[0] as GuiaRemisionProducto;

						return grp;
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
		public List<GuiaRemisionProducto> Search(GuiaRemisionProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(GuiaRemisionProducto),
						typeof(MovimientoAlmacenProducto),
						typeof(Producto),
						typeof(UnidadMedida)
					},
					x =>
					{
						GuiaRemisionProducto grp = x[0] as GuiaRemisionProducto;
						MovimientoAlmacenProducto map = x[1] as MovimientoAlmacenProducto;
						Producto p = x[2] as Producto;
						UnidadMedida um = x[3] as UnidadMedida;

						grp.MovimientoAlmacenProducto = map;
						grp.MovimientoAlmacenProductoId = map?.EntityId;

						if (!Equals(map, null))
						{
							map.Producto = p;
							map.ProductoId = p.EntityId;

							p.UnidadMedida = um;
							p.UnidadMedidaId = um.EntityId;
						}
						grp.Producto = p;

						return grp;
					},
					"mov_alm_prod_id,producto_id,unidad_medida_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public GuiaRemisionProducto SingleSearch(GuiaRemisionProducto entity)
		{
			try
			{
				List<GuiaRemisionProducto> guiaRemisionProductoList = Search(entity);

				return guiaRemisionProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(GuiaRemisionProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("movimientoAlmacenProductoId", entity.MovimientoAlmacenProductoId);
				parameters.AddParameter("codigoProducto", entity.CodigoProducto);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
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
		public int Update(GuiaRemisionProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionProductoId", entity.GuiaRemisionProductoId);
				parameters.AddParameter("codigoProducto", entity.CodigoProducto);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
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
		public void Delete(GuiaRemisionProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionProductoId", entity.GuiaRemisionProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void EliminarPorGuiaRemisionId(GuiaRemisionProducto entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorGuiaRemisionId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(List<GuiaRemisionProducto> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<GuiaRemisionProductoType> guiaRemisionProductoTypeList = new List<GuiaRemisionProductoType>();
				if (list != null)
				{
					guiaRemisionProductoTypeList = (from s in list
																select new GuiaRemisionProductoType
																{
																	GuiaRemisionProductoId = s.GuiaRemisionProductoId,
																	GuiaRemisionId = s.GuiaRemisionId,
																	MovimientoAlmacenProductoId = s.MovimientoAlmacenProductoId.Value,
																	CodigoProducto = s.CodigoProducto,
																	Descripcion = s.Descripcion,
																	Cantidad = s.Cantidad,
																	PesoBruto = s.PesoBruto,
																	PesoNeto = s.PesoNeto,
																	Estado = s.Estado,
																	UsuarioCreacionId = s.UsuarioCreacionId,
																}).ToList();
				}

				parameters.AddTableParameter("guiaRemisionProductoList", "dbo.GuiaRemisionProductoType", guiaRemisionProductoTypeList);

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
