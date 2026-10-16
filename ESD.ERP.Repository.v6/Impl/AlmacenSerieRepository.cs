using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class AlmacenSerieRepository : IAlmacenSerieRepository
	{
		private const string SP_PRE_NAME = "sp_almacen_serie";
		public AlmacenSerie GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(AlmacenSerie)
					},
					x =>
					{
						AlmacenSerie als = x[0] as AlmacenSerie;

						return als;
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
		public List<AlmacenSerie> Search(AlmacenSerie entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("almacenId", entity.AlmacenId);
			parameters.AddParameter("tipoDocumentoComercialId", entity.DocumentoComercialSerie?.TipoDocumentoComercialId);
			parameters.AddParameter("tipoDocumentoComercial", entity.DocumentoComercialSerie?.TipoDocumentoComercial?.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(AlmacenSerie),
						typeof(DocumentoComercialSerie),
						typeof(TipoDocumentoComercial)
					},
					x =>
					{
						AlmacenSerie als = x[0] as AlmacenSerie;
						DocumentoComercialSerie dcs = x[1] as DocumentoComercialSerie;
						TipoDocumentoComercial tdc = x[2] as TipoDocumentoComercial;

						als.DocumentoComercialSerie = dcs;
						als.DocumentoComercialSerieId = dcs.EntityId;
						dcs.TipoDocumentoComercial = tdc;
						dcs.TipoDocumentoComercialId = tdc.EntityId;

						return als;
					},
					"doc_com_serie_id,tipo_doc_com_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public AlmacenSerie SingleSearch(AlmacenSerie entity)
		{
			try
			{
				List<AlmacenSerie> almacenSerieList = Search(entity);

				return almacenSerieList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(AlmacenSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("documentoComercialSerieId", entity.DocumentoComercialSerieId);
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
		public int Update(AlmacenSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenSerieId", entity.AlmacenSerieId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("docComSerieId", entity.DocumentoComercialSerieId);
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
		public void Delete(AlmacenSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenSerieId", entity.AlmacenSerieId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarListaDocumentoComercialSerieId(AlmacenSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("documentoComercialSerieIdList", entity.DocumentoComercialSerieIdList);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarListaDocumentoComercialSerieId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
