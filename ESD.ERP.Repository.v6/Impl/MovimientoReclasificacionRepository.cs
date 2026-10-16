using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MovimientoReclasificacionRepository : IMovimientoReclasificacionRepository
	{
		private const string SP_PRE_NAME = "sp_mov_reclasi";
		public MovimientoReclasificacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MovimientoReclasificacion),
						typeof(MovimientoAlmacen),
						typeof(Proveedor),
						typeof(EstadoMovimientoReclasificacion)
					},
					x =>
					{
						MovimientoReclasificacion mr = x[0] as MovimientoReclasificacion;
						MovimientoAlmacen ma = x[1] as MovimientoAlmacen;
						Proveedor p = x[2] as Proveedor;
						EstadoMovimientoReclasificacion emr = x[3] as EstadoMovimientoReclasificacion;

						mr.MovimientoAlmacen = ma;
						mr.MovimientoAlmacenId = ma.EntityId;
						ma.Proveedor = p;
						ma.ProveedorId = p?.EntityId;
						mr.EstadoMovimientoReclasificacion = emr;
						mr.EstadoMovimientoReclasificacionId = emr.EntityId;

						return mr;
					},
					"mov_almacen_id,proveedor_id,estado_mov_rec_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public List<MovimientoReclasificacion> Search(MovimientoReclasificacion entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
            parameters.AddParameter("containsMovimientoReclasificacion", entity.Codigo);
            parameters.AddParameter("almacenId", entity.MovimientoAlmacen?.AlmacenId);
            parameters.AddParameter("fechaDesde", entity.FechaDesde);
            parameters.AddParameter("fechaHasta", entity.FechaHasta);
            parameters.AddParameter("estado", entity.Estado);
            parameters.AddParameter("estadomovRecla", entity.EstadoMovimientoReclasificacion?.Codigo);
			
            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                typeof(MovimientoReclasificacion),
                typeof(MovimientoAlmacen),
				typeof(Usuario),
                typeof(Almacen),
                typeof(TransaccionAlmacen),
                typeof(TipoMovimientoAlmacen),
                typeof(TipoDocumentoComercial),
                typeof(Proveedor),
				typeof(EstadoMovimientoReclasificacion),
                    },
                    x =>
                    {
                        MovimientoReclasificacion mr = x[0] as MovimientoReclasificacion;
                        MovimientoAlmacen ma = x[1] as MovimientoAlmacen;
                        Usuario u = x[2] as Usuario;
                        Almacen a = x[3] as Almacen;
                        TransaccionAlmacen ta = x[4] as TransaccionAlmacen;
                        TipoMovimientoAlmacen tma = x[5] as TipoMovimientoAlmacen;
                        TipoDocumentoComercial tdc = x[6] as TipoDocumentoComercial;
                        Proveedor p = x[7] as Proveedor;
                        EstadoMovimientoReclasificacion emr = x[8] as EstadoMovimientoReclasificacion;
						

                        mr.MovimientoAlmacen = ma;
                        mr.MovimientoAlmacenId = ma.EntityId;
                        mr.UsuarioAutorizaId = u?.UsuarioId;
                        mr.UsuarioAutoriza = u;
                        ma.Almacen = a;
                        ma.AlmacenId = a.EntityId;
                        ma.TransaccionAlmacen = ta;
                        ma.TransaccionAlmacenId = ta.EntityId;
                        ma.TipoMovimientoAlmacen = tma;
                        ma.TipoMovimientoAlmacenId = tma.EntityId;
                        ma.TipoDocumentoComercial = tdc;
                        ma.TipoDocumentoComercialId = tdc?.EntityId;
                        ma.Proveedor = p;
                        ma.ProveedorId = p?.EntityId;
                        mr.EstadoMovimientoReclasificacion = emr;
                        mr.EstadoMovimientoReclasificacionId = emr.EntityId;
						
                        return mr;
                    },
                    "mov_almacen_id,usuario_id,almacen_id,trans_alma_id,tipo_mov_alma_id,tipo_doc_com_id,proveedor_id,estado_mov_rec_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

        public MovimientoReclasificacion SingleSearch(MovimientoReclasificacion entity)
		{
			try
			{
				List<MovimientoReclasificacion> movimientoReclasificacionList = Search(entity);

				return movimientoReclasificacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public int Insert(MovimientoReclasificacion entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
                parameters.AddParameter("movimientoReclasificacion", entity.Codigo);
                parameters.AddParameter("fecha", entity.Fecha);
                parameters.AddParameter("mesContableId", entity.MesContableId);
                parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
                parameters.AddParameter("observacion", entity.Observacion);
                parameters.AddParameter("estadoMovimientoReclasificacionId", entity.EstadoMovimientoReclasificacionId);
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

        public int Update(MovimientoReclasificacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoReclasificacionId", entity.MovimientoReclasificacionId);
				parameters.AddParameter("observacion", entity.Observacion);
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
       public void Delete(MovimientoReclasificacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoReclasificacionId", entity.MovimientoReclasificacionId);
				parameters.AddParameter("estadoMovimientoReclasificacionId", entity.EstadoMovimientoReclasificacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
				
		public void ActualizarEstado(MovimientoReclasificacion entity)
		{
			Dictionary <string, object> parameters = new();

			try 
			{
				parameters.AddParameter("movimientoReclasificacionId", entity.MovimientoReclasificacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SOLICITAR), parameters);
				
			}
			catch (Exception e) 
			{
				Logger.Error(e.ToString());	
				throw;
			}
		}

		public void AprobDesa(MovimientoReclasificacion entity)
		{
			Dictionary <string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoReclasificacionId", entity.MovimientoReclasificacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);
                parameters.AddParameter("estadomovReclaId", entity.EstadoMovimientoReclasificacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_aprobdesa"), parameters);
            }
			catch (Exception e) 
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoMovimiento(MovimientoReclasificacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoReclasificacionId", entity.MovimientoReclasificacionId);
				parameters.AddParameter("estadoMovimientoReclasificacionId", entity.EstadoMovimientoReclasificacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoMovimiento"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
