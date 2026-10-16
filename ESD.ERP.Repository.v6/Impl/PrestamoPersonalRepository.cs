using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PrestamoPersonalRepository : IPrestamoPersonalRepository
	{
		private const string SP_PRE_NAME = "sp_presta_perso";
		public PrestamoPersonal GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PrestamoPersonal),
						typeof(Personal),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(EstadoPrestamo)
					},
					x =>
					{
						PrestamoPersonal pp = x[0] as PrestamoPersonal;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;
						Moneda m = x[3] as Moneda;
						EstadoPrestamo ep = x[4] as EstadoPrestamo;

						pp.Personal = p;
						pp.PersonalId = p.EntityId;
						p.ProveedorId = prv.EntityId;
						p.Proveedor = prv;
						pp.Moneda = m;
						pp.MonedaId = m.EntityId;
						pp.EstadoPrestamo = ep;
						pp.EstadoPrestamoId = ep.EntityId;

						return pp;
					},
					"personal_id,proveedor_id,moneda_id,estado_presta_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PrestamoPersonal> Search(PrestamoPersonal entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("containsRazonSocial", entity.Personal?.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Personal?.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PrestamoPersonal),
						typeof(Personal),
						typeof(Proveedor),
						typeof(TipoDocumentoIdentidad),
						typeof(Moneda),
						typeof(TipoPrestamo),
						typeof(EstadoPrestamo)
					},
					x =>
					{
						PrestamoPersonal pp = x[0] as PrestamoPersonal;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;
						TipoDocumentoIdentidad tdi = x[3] as TipoDocumentoIdentidad;
						Moneda m = x[4] as Moneda;
						TipoPrestamo tp = x[5] as TipoPrestamo;
						EstadoPrestamo ep = x[6] as EstadoPrestamo;

						pp.Personal = p;
						pp.PersonalId = p.EntityId;
						p.ProveedorId = prv.EntityId;
						p.Proveedor = prv;
						prv.TipoDocumentoIdentidad = tdi;
						prv.TipoDocumentoIdentidadId = tdi.EntityId;
						pp.Moneda = m;
						pp.MonedaId = m.EntityId;
						pp.TipoPrestamo = tp;
						pp.TipoPrestamoId = tp.EntityId;
						pp.EstadoPrestamo = ep;
						pp.EstadoPrestamoId = ep.EntityId;

						return pp;
					},
					"personal_id,proveedor_id,tipo_doc_iden_id,moneda_id,tipo_prestamo_id,estado_presta_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PrestamoPersonal SingleSearch(PrestamoPersonal entity)
		{
			try
			{
				List<PrestamoPersonal> prestamoPersonalList = Search(entity);

				return prestamoPersonalList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PrestamoPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("prestamoPersonal", entity.Codigo);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTipoCambio", entity.MontoTipoCambio);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("cantidadCuotas", entity.CantidadCuotas);
				parameters.AddParameter("fechaInicio", entity.FechaInicio);
				parameters.AddParameter("diasCuota", entity.DiasCuota);
				parameters.AddParameter("tipoPrestamoId", entity.TipoPrestamoId);
				parameters.AddParameter("estadoPrestamoId", entity.EstadoPrestamoId);
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
		public int Update(PrestamoPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("prestamoPersonalId", entity.PrestamoPersonalId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTipoCambio", entity.MontoTipoCambio);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("cantidadCuotas", entity.CantidadCuotas);
				parameters.AddParameter("fechaInicio", entity.FechaInicio);
				parameters.AddParameter("diasCuota", entity.DiasCuota);
				parameters.AddParameter("tipoPrestamoId", entity.TipoPrestamoId);
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
		public void Delete(PrestamoPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("prestamoPersonalId", entity.PrestamoPersonalId);
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
