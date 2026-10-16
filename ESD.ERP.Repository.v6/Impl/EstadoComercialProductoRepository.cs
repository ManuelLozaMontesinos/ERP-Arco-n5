using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EstadoComercialProductoRepository : IEstadoComercialProductoRepository
	{
		private const string SP_PRE_NAME = "sp_estado_com_prod";
		public EstadoComercialProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EstadoComercialProducto)
					},
					x =>
					{
						EstadoComercialProducto ecp = x[0] as EstadoComercialProducto;

						return ecp;
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
		public List<EstadoComercialProducto> Search(EstadoComercialProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EstadoComercialProducto)
					},
					x =>
					{
						EstadoComercialProducto ecp = x[0] as EstadoComercialProducto;

						return ecp;
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
		public EstadoComercialProducto SingleSearch(EstadoComercialProducto entity)
		{
			try
			{
				List<EstadoComercialProducto> estadoComercialProductoList = Search(entity);

				return estadoComercialProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EstadoComercialProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoComercialProducto", entity.Codigo);
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
		public int Update(EstadoComercialProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoComercialProductoId", entity.EstadoComercialProductoId);
				parameters.AddParameter("estadoComercialProducto", entity.Codigo);
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
		public void Delete(EstadoComercialProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoComercialProductoId", entity.EstadoComercialProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public EstadoComercialProducto ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(EstadoComercialProducto)
					},
					x =>
					{
						EstadoComercialProducto ecp = x[0] as EstadoComercialProducto;

						return ecp;
					},
					string.Empty
				).FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
