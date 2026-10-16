using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
   public class RolRepository : IRolRepository
   {
      private const string SP_PRE_NAME = "sp_rol";
      public Rol GetById(int id)
      {
         try
         {
            return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
               new[]
               {
                  typeof(Rol)
               },
               x =>
               {
                  Rol r = x[0] as Rol;

                  return r;
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
      public List<Rol> Search(Rol entity)
      {
         Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

         parameters.AddParameter("empresaId", entity.EmpresaId);
         parameters.AddParameter("containsNombre", entity.Nombre);
         parameters.AddParameter("estado", entity.Estado);

         try
         {
            return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
               new[]
               {
                  typeof(Rol),
                  typeof(OrigenRegistro)
               },
               x =>
               {
                  Rol r = x[0] as Rol;
                  OrigenRegistro ore = x[1] as OrigenRegistro;

                  return r;
               },
               "origen_registro_id"
            );
         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }
      }
      public Rol SingleSearch(Rol entity)
      {
         try
         {
            List<Rol> rolList = Search(entity);

            return rolList.FirstOrDefault();
         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }
      }
      public int Insert(Rol entity)
      {
         Dictionary<string, object> parameters = new();

         try
         {
            parameters.AddParameter("empresaId", entity.EmpresaId);
            parameters.AddParameter("rol", entity.Codigo);
            parameters.AddParameter("nombre", entity.Nombre);
            parameters.AddParameter("origenRegistroId", entity.OrigenRegistroId);
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
      public int Update(Rol entity)
      {
         Dictionary<string, object> parameters = new();

         try
         {
            parameters.AddParameter("rolId", entity.RolId);
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
      public void Delete(Rol entity)
      {
         Dictionary<string, object> parameters = new();

         try
         {
            parameters.AddParameter("rolId", entity.RolId);
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
