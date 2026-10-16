using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenProduccionProductoEntregaRepository : IOrdenProduccionProductoEntregaRepository
	{
		private const string SP_PRE_NAME = "sp_comprob_p_ent";
		public OrdenProduccionProductoEntrega GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OrdenProduccionProductoEntrega)
					},
					x =>
					{
						OrdenProduccionProductoEntrega cpe = x[0] as OrdenProduccionProductoEntrega;

						return cpe;
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
		public List<OrdenProduccionProductoEntrega> Search(OrdenProduccionProductoEntrega entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("ordenProduccionProductoId", entity.OrdenProduccionProductoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OrdenProduccionProductoEntrega),
						typeof(Personal),
						typeof(Proveedor)
					},
					x =>
					{
						OrdenProduccionProductoEntrega oppe = x[0] as OrdenProduccionProductoEntrega;
						Personal per = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;

						oppe.Personal = per;
						oppe.PersonalId = per.EntityId;
						per.Proveedor = prv;
						per.ProveedorId = prv.EntityId;

						return oppe;
					},
					"personal_id,proveedor_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public OrdenProduccionProductoEntrega SingleSearch(OrdenProduccionProductoEntrega entity)
		{
			try
			{
				List<OrdenProduccionProductoEntrega> ordenProduccionProductoEntregaList = Search(entity);

				return ordenProduccionProductoEntregaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(OrdenProduccionProductoEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionProductoId", entity.OrdenProduccionProductoId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("entregadoA", entity.EntregadoA);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("personalId", entity.PersonalId);
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
		public int Update(OrdenProduccionProductoEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionProductoEntregaId", entity.OrdenProduccionProductoEntregaId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("entregadoA", entity.EntregadoA);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("personalId", entity.PersonalId);
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
		public void Delete(OrdenProduccionProductoEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionProductoEntregaId", entity.OrdenProduccionProductoEntregaId);
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
