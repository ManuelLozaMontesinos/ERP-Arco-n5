using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
    public class PuestoRepository : IPuestoRepository
    {
        private const string SP_PRE_NAME = "sp_puesto";
        public Puesto GetById(int id)
        {
            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
                    new[]
                    {
                        typeof(Puesto),
                    },
                    x =>
                    {
                        Puesto p = x[0] as Puesto;
                   
                        return p;
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
        public List<Puesto> Search(Puesto entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("puestoId", entity?.PuestoId);
            parameters.AddParameter("nombre", entity?.Nombre);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(Puesto),
                    },
                    x =>
                    {
                        Puesto p = x[0] as Puesto;

                        return p;
                    },
                    String.Empty
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public Puesto SingleSearch(Puesto entity)
        {
            try
            {
                List<Puesto> contactosList = Search(entity);

                return contactosList.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

        public int Insert(Puesto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("", entity.PuestoId);
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

        public int Update(Puesto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("", entity.PuestoId);
                parameters.AddParameter("", entity.UsuarioCreacionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

                return entity.EntityId;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

        public void Delete(Puesto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("", entity.PuestoId);
                parameters.AddParameter("", entity.UsuarioCreacionId);

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
