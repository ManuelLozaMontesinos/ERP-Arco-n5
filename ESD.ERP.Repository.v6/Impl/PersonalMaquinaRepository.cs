using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
    public class PersonalMaquinaRepository : IPersonalMaquinaRepository
    {
        private const string SP_PRE_NAME = "sp_personal_maquina";
        public PersonalMaquina GetById(int id)
        {
            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
                    new[]
                    {
                        typeof(PersonalMaquina),
                        typeof(Maquina),
                        typeof(Personal),
                        typeof(Personal),
                        typeof(Proveedor),
                        typeof(Proveedor)
                        
                    },
                    x =>
                    {
                        PersonalMaquina pm = x[0] as PersonalMaquina;
                        Maquina m = x[1] as Maquina;    
                        Personal p1 = x[2] as Personal;
                        Personal p2 = x[3] as Personal;
                        Proveedor pvd1 = x[4] as Proveedor;
                        Proveedor pvd2 = x[5] as Proveedor;
                        
                        pm.Maquina = m;
                        pm.MaquinaId = m.EntityId;
                        pm.Personal = p1;
                        pm.PersonalId = p1.EntityId;
                        pm.Asistente = p2;
                        pm.AsistenteId = p2.EntityId;
                        p1.Proveedor = pvd1;
                        p1.ProveedorId = pvd1.EntityId;
                        p2.Proveedor = pvd2;
                        p2.ProveedorId = pvd2.EntityId;

                        return pm;
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
        public List<PersonalMaquina> Search(PersonalMaquina entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("PersonalMaquinaId", entity?.PersonalMaquinaId);
            parameters.AddParameter("MaquinaId", entity?.MaquinaId);
            parameters.AddParameter("PersonalId", entity?.PersonalId);
            parameters.AddParameter("AsistenteId", entity?.AsistenteId);
            parameters.AddParameter("Fecha", entity.Fecha);
            parameters.AddParameter("PersonalNom", entity.Personal?.Proveedor?.RazonSocial);
            parameters.AddParameter("AsistenteNom", entity.Asistente?.Proveedor?.RazonSocial);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(PersonalMaquina),
                        typeof(Maquina),
                        typeof(Personal),
                        typeof(Proveedor),
                        typeof(Personal),
                        typeof(Proveedor),
                    },
                    x =>
                    {
                        PersonalMaquina pm = x[0] as PersonalMaquina;
                        Maquina m = x[1] as Maquina;
                        Personal p1 = x[2] as Personal;
                        Proveedor pvd1 = x[3] as Proveedor;
                        Personal p2 = x[4] as Personal;
                        Proveedor pvd2 = x[5] as Proveedor;
                        
                        pm.Maquina = m;
                        pm.MaquinaId = m.EntityId;
                        pm.Personal = p1;
                        pm.PersonalId = p1.EntityId;
                        p1.Proveedor = pvd1;
                        p1.ProveedorId = pvd1.EntityId;
                        pm.Asistente = p2;
                        pm.AsistenteId = p2.PersonalId;
                        p2.Proveedor = pvd2;
                        p2.ProveedorId = pvd2.EntityId;

                        return pm;
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
        public PersonalMaquina SingleSearch(PersonalMaquina entity)
        {
            try
            {
                List<PersonalMaquina> personalMaquinaList = Search(entity);

                return personalMaquinaList.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public int Insert(PersonalMaquina entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                
                int id = DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.INSERT), parameters);

                return id;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public int Update(PersonalMaquina entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

                return entity.EntityId;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void Delete(PersonalMaquina entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                

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



