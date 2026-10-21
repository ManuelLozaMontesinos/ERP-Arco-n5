using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenProduccionRepository : IOrdenProduccionRepository
	{
		private const string SP_PRE_NAME = "sp_orden_pro";
        public OrdenProduccion GetById(int id)
        {
            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
                    new[]
                    {
                        typeof(OrdenProduccion),
                        typeof(PeriodoEmpresaSede),
                        typeof(EmpresaSede),
                        typeof(Proveedor),
                        typeof(EstadoOrdenProduccion),
                        typeof(EstadoDespacho),
						typeof(Cotizacion)
                    },
                    x =>
                    {
                        OrdenProduccion op = x[0] as OrdenProduccion;
                        PeriodoEmpresaSede pes = x[1] as PeriodoEmpresaSede;
                        EmpresaSede es = x[2] as EmpresaSede;
                        Proveedor p = x[3] as Proveedor;
                        EstadoOrdenProduccion eop = x[4] as EstadoOrdenProduccion;
                        EstadoDespacho ed = x[5] as EstadoDespacho;
						Cotizacion c = x[6] as Cotizacion;

                        op.PeriodoEmpresaSede = pes;
                        op.PeriodoEmpresaSedeId = pes.EntityId;
                        pes.EmpresaSede = es;
                        pes.EmpresaSedeId = es.EmpresaSedeId;
                        op.Proveedor = p;
                        op.ProveedorId = p.EntityId;
                        op.EstadoOrdenProduccion = eop;
                        op.EstadoOrdenProduccionId = eop.EntityId;
                        op.EstadoDespacho = ed;
                        op.EstadoDespachoId = ed?.EntityId;
						op.Cotizacion = c;
						op.CotizacionId = c.EntityId;

                        return op;
                    },
                    "periodo_emp_sede_id,empresa_sede_id,proveedor_id,estado_orden_p_id,estado_despacho_id,cotiza_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public List<OrdenProduccion> Search(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("ordenProduccion", entity.Codigo);
			parameters.AddParameter("cotizacionId", entity.CotizacionId);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("estadoOrdenProduccionList", entity.EstadoOrdenProduccionList);
			parameters.AddParameter("incluyeInsumo", entity.IncluyeInsumo);
			parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
			parameters.AddParameter("cotiza", entity.Cotizacion?.Codigo);
			parameters.AddParameter("containsVendedor", entity.Cotizacion?.Vendedor.Proveedor.RazonSocial);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OrdenProduccion),
						typeof(Cotizacion),
						typeof(Proveedor),
						typeof(EstadoOrdenProduccion),
						typeof(EstadoDespacho),
						typeof(Personal),
						typeof(Proveedor),
						typeof(Vendedor),
						typeof(Proveedor),
						typeof(EstadoOrdenProduccion),
						typeof(EstadoOrdenProduccion),
					},
					x =>
					{
						OrdenProduccion op = x[0] as OrdenProduccion;
						Cotizacion c = x[1] as Cotizacion;
						Proveedor p = x[2] as Proveedor;
						EstadoOrdenProduccion eop = x[3] as EstadoOrdenProduccion;
						EstadoDespacho ed = x[4] as EstadoDespacho;
						Personal per = x[5] as Personal;
						Proveedor p2 = x[6] as Proveedor;
						Vendedor v = x[7] as Vendedor;
						Proveedor pvd = x[8] as Proveedor;
						EstadoOrdenProduccion eopMel = x[9] as EstadoOrdenProduccion;
						EstadoOrdenProduccion eopMdf = x[10] as EstadoOrdenProduccion;

                        op.Cotizacion = c;
						op.CotizacionId = c?.EntityId;
						op.Proveedor = p;
						op.ProveedorId = p.EntityId;
						op.EstadoOrdenProduccion = eop;
						op.EstadoOrdenProduccionId = eop.EntityId;
						op.EstadoDespacho = ed;
						op.EstadoDespachoId = ed?.EntityId;
						op.Personal = per;
						op.PersonalId = per?.EntityId;
						if (!Equals(per, null))
						{
							per.Proveedor = p2;
							per.ProveedorId = p2.EntityId;
						}
						c.Vendedor = v;
						c.VendedorId = v.VendedorId;
						v.Proveedor = pvd;
						v.ProveedorId = pvd.ProveedorId;
						if (!Equals(eopMel, null))
						{
							op.EstadoPlanoMel = eopMel;
							op.EstadoPlanoMelId = eopMel.EntityId;
						}
						if(!Equals(eopMdf, null))
						{
                            op.EstadoPlanoMdf = eopMdf;
                            op.EstadoPlanoMdfId = eopMdf.EntityId;
                        }
                       
                        return op;
					},
                    "cotiza_id,proveedor_id,estado_orden_p_id,estado_despacho_id,personal_id,proveedor_id,vendedor_id,proveedor_id,estado_orden_p_id,estado_orden_p_id"
                );
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public OrdenProduccion SingleSearch(OrdenProduccion entity)
		{
			try
			{
				List<OrdenProduccion> ordenProduccionList = Search(entity);

				return ordenProduccionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("ordenProduccion", entity.Codigo);
				parameters.AddParameter("cotizacionId", entity.CotizacionId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("RutaProduccionId", entity.RutaProduccionId);
				parameters.AddParameter("fechaEntrega", entity.FechaEntrega);
				parameters.AddParameter("estadoOrdenProduccionId", entity.EstadoOrdenProduccionId);
				parameters.AddParameter("incluyeInsumo", entity.IncluyeInsumo);
				parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
				parameters.AddParameter("observacionDespacho", entity.ObservacionDespacho);
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
		public int Update(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
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
		public void Delete(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstado(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("estadoOrdenProduccionId", entity.EstadoOrdenProduccionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoDespacho(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaDespacho", entity.FechaDespacho);
				parameters.AddParameter("observacionDespacho", entity.ObservacionDespacho);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoDespacho"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void EliminarSubProcesos(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarSubProcesos"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarFecha(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("fecha", entity.Fecha);
                parameters.AddParameter("usuarioAccionId ", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarFecha"), parameters);
            }
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void CerrarInsumo(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
                parameters.AddParameter("estadoDespachoId", entity.EstadoDespachoId);
                parameters.AddParameter("observacion", entity.ObservacionDespacho);
                parameters.AddParameter("usuarioAccionId", entity.PersonalId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_cerrarInsumo"), parameters);
            }
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void CambiarEstado(OrdenProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("estadoplanoMelId", entity.EstadoPlanoMelId);
				parameters.AddParameter("estadoplanoMdfId", entity.EstadoPlanoMdfId);
				parameters.AddParameter("comentarios", entity.Comentarios);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_cambiarEstado"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void EliminarInsumo(int id)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("OrdenProduccionId", id);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarInsumo"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		

    }
}
