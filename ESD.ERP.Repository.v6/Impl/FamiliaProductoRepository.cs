using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class FamiliaProductoRepository : IFamiliaProductoRepository
	{
		private const string SP_PRE_NAME = "sp_familia_producto";
		public FamiliaProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
					},
					x =>
					{
						FamiliaProducto fp = x[0] as FamiliaProducto;
						ClaseProducto cp = x[1] as ClaseProducto;
						GrupoProducto gp = x[2] as GrupoProducto;
						TipoProducto tp = x[3] as TipoProducto;

						fp.ClaseProducto = cp;
						fp.ClaseProductoId = cp.EntityId;
						cp.GrupoProducto = gp;
						cp.GrupoProductoId = cp.EntityId;
						gp.TipoProducto = tp;
						gp.TipoProductoId = tp.EntityId;

						return fp;
					},
					"clase_producto_id,grupo_producto_id,tipo_producto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public List<FamiliaProducto> Search(FamiliaProducto entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("notFamiliaProductoId", entity.NotFamiliaProductoId);
            parameters.AddParameter("claseProductoId", entity.ClaseProductoId);
            parameters.AddParameter("familiaProducto", entity.Codigo);
            parameters.AddParameter("periodo", entity.Periodo);
            parameters.AddParameter("estado", entity.Estado);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(FamiliaProducto)
                    },
                    x =>
                    {
                        FamiliaProducto fp = x[0] as FamiliaProducto;

                        return fp;
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
        public FamiliaProducto SingleSearch(FamiliaProducto entity)
		{
			try
			{
				List<FamiliaProducto> familiaProductoList = Search(entity);

				return familiaProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(FamiliaProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("claseProductoId", entity.ClaseProductoId);
				parameters.AddParameter("familiaProducto", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("correlativo", entity.Correlativo);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public int Update(FamiliaProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("familiaProductoId", entity.FamiliaProductoId);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public void Delete(FamiliaProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("familiaProductoId", entity.FamiliaProductoId);
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
		public void ActualizarCorrelativo(FamiliaProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("familiaProductoId", entity.FamiliaProductoId);
				parameters.AddParameter("correlativo", entity.Correlativo);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCorrelativo"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void EliminarPorClaseProductoId(FamiliaProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("claseProductoId", entity.ClaseProductoId);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorClaseProductoId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
