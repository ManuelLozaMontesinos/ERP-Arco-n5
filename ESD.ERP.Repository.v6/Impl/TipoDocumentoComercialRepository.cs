using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoDocumentoComercialRepository : ITipoDocumentoComercialRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_doc_com";
		public TipoDocumentoComercial GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoDocumentoComercial)
					},
					x =>
					{
						TipoDocumentoComercial tdc = x[0] as TipoDocumentoComercial;

						return tdc;
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
		public List<TipoDocumentoComercial> Search(TipoDocumentoComercial entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("tipoDocumentoComercialList", entity.TipoDocumentoComercialList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoDocumentoComercial)
					},
					x =>
					{
						TipoDocumentoComercial tdc = x[0] as TipoDocumentoComercial;

						return tdc;
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
		public TipoDocumentoComercial SingleSearch(TipoDocumentoComercial entity)
		{
			try
			{
				List<TipoDocumentoComercial> tipoDocumentoComercialList = Search(entity);

				return tipoDocumentoComercialList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoDocumentoComercial entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoDocumentoComercial", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abrev", entity.Abreviatura);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
				parameters.AddParameter("esInterno", entity.EsInterno);
				parameters.AddParameter("esCompPago", entity.EsComprobantePago);
				parameters.AddParameter("esElectro", entity.EsElectronico);
				parameters.AddParameter("esCanjeable", entity.EsCanjeable);
				parameters.AddParameter("montoMaxSinProv", entity.MontoMaximoSinProveedor);
				parameters.AddParameter("validarProve", entity.ValidarProveedor);
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
		public int Update(TipoDocumentoComercial entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("tipoDocumentoComercial", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abrev", entity.Abreviatura);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
				parameters.AddParameter("esInterno", entity.EsInterno);
				parameters.AddParameter("esCompPago", entity.EsComprobantePago);
				parameters.AddParameter("esElectro", entity.EsElectronico);
				parameters.AddParameter("esCanjeable", entity.EsCanjeable);
				parameters.AddParameter("montoMaxSinProv", entity.MontoMaximoSinProveedor);
				parameters.AddParameter("validarProve", entity.ValidarProveedor);
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
		public void Delete(TipoDocumentoComercial entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public TipoDocumentoComercial ObtenerPorCodigo(string codigo)
      {
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(TipoDocumentoComercial)
					},
					x =>
					{
						TipoDocumentoComercial tdc = x[0] as TipoDocumentoComercial;

						return tdc;
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
