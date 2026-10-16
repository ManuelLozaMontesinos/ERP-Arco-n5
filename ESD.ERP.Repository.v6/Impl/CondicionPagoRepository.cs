using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CondicionPagoRepository : ICondicionPagoRepository
	{
		private const string SP_PRE_NAME = "sp_cond_pago";
		public CondicionPago GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(CondicionPago)
					},
					x =>
					{
						CondicionPago cp = x[0] as CondicionPago;

						return cp;
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
		public List<CondicionPago> Search(CondicionPago entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("condicionPago", entity.Codigo);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("tipoCondicionPago", entity?.TipoCondicionPago?.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(CondicionPago),
						typeof(TipoCondicionPago)
					},
					x =>
					{
						CondicionPago cp = x[0] as CondicionPago;
						TipoCondicionPago tcp = x[1] as TipoCondicionPago;

						cp.TipoCondicionPago = tcp;
						cp.TipoCondicionPagoId = tcp.EntityId;

						return cp;
					},
					"tipo_c_pago_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public CondicionPago SingleSearch(CondicionPago entity)
		{
			try
			{
				List<CondicionPago> condicionPagoList = Search(entity);

				return condicionPagoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(CondicionPago entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("condicionPago", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("tipoCondicionPagoId", entity.TipoCondicionPagoId);
				parameters.AddParameter("dia", entity.Dia);
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
		public int Update(CondicionPago entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("dia", entity.Dia);
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
		public void Delete(CondicionPago entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
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
