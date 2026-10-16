using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
    public class CortesGratisRepository : ICortesGratisRepository
    {
        private const string SP_PRE_NAME = "sp_cortes_gratis";
        public CortesGratis GetById(int sedeId)
        {
            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), sedeId,
                    new[]
                    {
                typeof(CortesGratis),
                typeof(FamiliaProducto)
                    },
                    x =>
                    {
                        CortesGratis cg = x[0] as CortesGratis;
                        Producto p = x[1] as Producto;
                                                
                        return cg;
                    },
                    "@sedeId"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public List<CortesGratis> Search(CortesGratis entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("FamiliaProductoId", entity?.Producto?.FamiliaProductoId);
            parameters.AddParameter("ClaseProductoId", entity.Producto?.FamiliaProducto?.ClaseProductoId);
            parameters.AddParameter("EmpresaSedeId", entity.EmpresaSedeId);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(CortesGratis),
                        typeof(Producto),
                        typeof(FamiliaProducto),
                        typeof(ClaseProducto),
                        typeof(EmpresaSede)
                    },
                    x =>
                    {
                        CortesGratis cg = x[0] as CortesGratis;
                        Producto p = x[1] as Producto;
                        FamiliaProducto fp = x[2] as FamiliaProducto;
                        ClaseProducto cp = x[3] as ClaseProducto;
                        EmpresaSede es = x[4] as EmpresaSede;

                        cg.Producto = p;
                        cg.ProductoId = p.EntityId;
                        p.FamiliaProducto = fp;
                        p.FamiliaProductoId = fp.EntityId;
                        fp.ClaseProducto = cp;
                        fp.ClaseProductoId = cp.EntityId;
                        cg.EmpresaSede = es;
                        cg.EmpresaSedeId = es.EntityId;
                        
                        return cg;
                    },
                    "producto_id,familia_producto_id,clase_producto_id,empresa_sede_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public CortesGratis SingleSearch(CortesGratis entity)
        {
            try
            {
                List<CortesGratis> cortesGratisList= Search(entity);

                return cortesGratisList.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public int Insert(CortesGratis entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cortes_gratis_id", entity.CortesGratisId);
                parameters.AddParameter("producto_id", entity.ProductoId);
                parameters.AddParameter("cantidadCortes", entity.Cantidad);
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
        public int Update(CortesGratis entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cortesGratisId", entity.CortesGratisId);
                parameters.AddParameter("cantidadcortes", entity.Cantidad);
                parameters.AddParameter("estado", entity.Estado);
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
        public void Delete(CortesGratis entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cortes_gratis_id", entity.CortesGratisId);
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