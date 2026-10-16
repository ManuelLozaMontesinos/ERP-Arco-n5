using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class IngresoTributoDescuentoRepository : IIngresoTributoDescuentoRepository
	{
		private const string SP_PRE_NAME = "sp_ing_trib_des";
		public IngresoTributoDescuento GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(IngresoTributoDescuento)
					},
					x =>
					{
						IngresoTributoDescuento itd = x[0] as IngresoTributoDescuento;

						return itd;
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
		public List<IngresoTributoDescuento> Search(IngresoTributoDescuento entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(IngresoTributoDescuento)
					},
					x =>
					{
						IngresoTributoDescuento itd = x[0] as IngresoTributoDescuento;

						return itd;
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
		public IngresoTributoDescuento SingleSearch(IngresoTributoDescuento entity)
		{
			try
			{
				List<IngresoTributoDescuento> ingresoTributoDescuentoList = Search(entity);

				return ingresoTributoDescuentoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(IngresoTributoDescuento entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ingresoTributoDescuento", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(IngresoTributoDescuento entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ingresoTributoDescuentoId", entity.IngresoTributoDescuentoId);
				parameters.AddParameter("ingresoTributoDescuento", entity.Codigo);
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
		public void Delete(IngresoTributoDescuento entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ingresoTributoDescuentoId", entity.IngresoTributoDescuentoId);
				parameters.AddParameter("estado", entity.Estado);

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
