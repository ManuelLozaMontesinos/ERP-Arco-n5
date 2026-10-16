using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OportunidadVentaRepository : IOportunidadVentaRepository
	{
		private const string SP_PRE_NAME = "sp_oportu_venta";
		public OportunidadVenta GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OportunidadVenta)
					},
					x =>
					{
						OportunidadVenta ov = x[0] as OportunidadVenta;

						return ov;
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
		public List<OportunidadVenta> Search(OportunidadVenta entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OportunidadVenta)
					},
					x =>
					{
						OportunidadVenta ov = x[0] as OportunidadVenta;

						return ov;
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
		public OportunidadVenta SingleSearch(OportunidadVenta entity)
		{
			try
			{
				List<OportunidadVenta> oportunidadVentaList = Search(entity);

				return oportunidadVentaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(OportunidadVenta entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
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
		public int Update(OportunidadVenta entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("oportunidadVentaId", entity.OportunidadVentaId);
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
		public void Delete(OportunidadVenta entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("oportunidadVentaId", entity.OportunidadVentaId);
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
