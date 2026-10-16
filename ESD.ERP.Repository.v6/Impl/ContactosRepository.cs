using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
    public class ContactosRepository: IContactosRepository
    {
        private const string SP_PRE_NAME = "sp_contactos";
        public Contactos GetById(int id)
        {
            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
                    new[]
                    {
                        typeof(Contactos),
                        typeof(TipoDocumentoIdentidad),
                        typeof(Proveedor),
                        
                    },
                    x =>
                    {
                        Contactos c = x[0] as Contactos;
                        TipoDocumentoIdentidad tdi = x[1] as TipoDocumentoIdentidad;
                        Proveedor pvd = x[2] as Proveedor;

                        c.TipoDocumentoIdentidad = tdi;
                        c.TipoDocumentoIdentidadId = tdi.EntityId;
                        c.Proveedor = pvd;
                        c.ProveedorId = pvd.EntityId;
                        
                        return c;
                    },
                        "tipo_doc_iden_id,proveedor_id"

                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public List<Contactos> Search(Contactos entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("contactoId", entity?.ContactosId);
            parameters.AddParameter("proveedorId", entity?.ProveedorId); 
            parameters.AddParameter("estado", entity?.Estado);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(Contactos),
                        typeof(Proveedor),
                        typeof(TipoDocumentoIdentidad),
                        typeof(Puesto),
                    },
                    x =>
                    {
                        Contactos c = x[0] as Contactos;
                        Proveedor p = x[1] as Proveedor;
                        TipoDocumentoIdentidad tdi = x[2] as TipoDocumentoIdentidad;
                        Puesto po = x[3] as Puesto;
                        
                        c.Proveedor = p;
                        c.ProveedorId = p.ProveedorId;
                        c.TipoDocumentoIdentidad = tdi;
                        c.TipoDocumentoIdentidadId = tdi.TipoDocumentoIdentidadId;
                        c.Puesto = po;
                        c.PuestoId = po.PuestoId;

                        return c;
                    },
                    "proveedor_id,tipo_doc_iden_id,puesto_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public Contactos SingleSearch(Contactos entity)
        {
            try
            {
                List<Contactos> contactosList = Search(entity);

                return contactosList.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

        public int Insert(Contactos entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("proveedorId", entity.ProveedorId);
                parameters.AddParameter("puestoId", entity.PuestoId);
                parameters.AddParameter("nombre", entity.NombreC);
                parameters.AddParameter("apellido_pat", entity.ApellidoPaterno);
                parameters.AddParameter("apellido_mat", entity.ApellidoMaterno);
                parameters.AddParameter("tipoDocIdentidadId", entity.TipoDocumentoIdentidadId);
                parameters.AddParameter("numerodocIden", entity.NumeroDocumentoIdentidadC);
                parameters.AddParameter("telefono", entity.Telefono);
                parameters.AddParameter("correo", entity.Correo);
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

        public int Update(Contactos entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("", entity.ContactosId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

                return entity.EntityId;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

        public void Delete(Contactos entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("contactosId", entity.ContactosId);

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
