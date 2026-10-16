using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ComprobanteProductoEntregaRepository : IComprobanteProductoEntregaRepository
	{
		private const string SP_PRE_NAME = "sp_comprob_p_ent";
		public ComprobanteProductoEntrega GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ComprobanteProductoEntrega)
					},
					x =>
					{
						ComprobanteProductoEntrega cpe = x[0] as ComprobanteProductoEntrega;

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
		public List<ComprobanteProductoEntrega> Search(ComprobanteProductoEntrega entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ComprobanteProductoEntrega),
						typeof(Personal),
						typeof(Proveedor)
					},
					x =>
					{
						ComprobanteProductoEntrega cpe = x[0] as ComprobanteProductoEntrega;
						Personal per = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;

						cpe.Personal = per;
						cpe.PersonalId = per.EntityId;
						per.Proveedor = prv;
						per.ProveedorId = prv.EntityId;

						return cpe;
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
		public ComprobanteProductoEntrega SingleSearch(ComprobanteProductoEntrega entity)
		{
			try
			{
				List<ComprobanteProductoEntrega> comprobanteProductoEntregaList = Search(entity);

				return comprobanteProductoEntregaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ComprobanteProductoEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("entregaDespacho", entity.EntregaDespacho);
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
		public int Update(ComprobanteProductoEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoEntregaId", entity.ComprobanteProductoEntregaId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("entregaDespacho", entity.EntregaDespacho);
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
		public void Delete(ComprobanteProductoEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoEntregaId", entity.ComprobanteProductoEntregaId);
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
