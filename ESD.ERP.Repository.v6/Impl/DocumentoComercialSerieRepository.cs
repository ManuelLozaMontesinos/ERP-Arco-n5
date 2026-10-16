using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class DocumentoComercialSerieRepository : IDocumentoComercialSerieRepository
	{
		private const string SP_PRE_NAME = "sp_doc_com_serie";
		public DocumentoComercialSerie GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(DocumentoComercialSerie),
						typeof(TipoDocumentoComercial)
					},
					x =>
					{
						DocumentoComercialSerie dcs = x[0] as DocumentoComercialSerie;
						TipoDocumentoComercial tdc = x[1] as TipoDocumentoComercial;

						dcs.TipoDocumentoComercial = tdc;
						dcs.TipoDocumentoComercialId = tdc.EntityId;

						return dcs;
					},
					"tipo_doc_com_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<DocumentoComercialSerie> Search(DocumentoComercialSerie entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaSede.EmpresaId);
			parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
			parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
			parameters.AddParameter("tipoDocumentoComercial", entity?.TipoDocumentoComercial?.Codigo);
			parameters.AddParameter("serie", entity.Serie);
			parameters.AddParameter("tipoDocumentoComercialRelacionadoId", entity.TipoDocumentoComercialRelacionadoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(DocumentoComercialSerie),
						typeof(EmpresaSede),
						typeof(TipoDocumentoComercial),
						typeof(Almacen)
					},
					x =>
					{
						DocumentoComercialSerie dcs = x[0] as DocumentoComercialSerie;
						EmpresaSede es = x[1] as EmpresaSede;
						TipoDocumentoComercial tdc = x[2] as TipoDocumentoComercial;
						Almacen a = x[3] as Almacen;

						dcs.EmpresaSede = es;
						dcs.EmpresaSedeId = es.EntityId;
						dcs.TipoDocumentoComercial = tdc;
						dcs.TipoDocumentoComercialId = tdc.EntityId;
						dcs.Almacen = a;
						dcs.AlmacenId = a?.EntityId;

						return dcs;
					},
					"empresa_sede_id,tipo_doc_com_id,almacen_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public DocumentoComercialSerie SingleSearch(DocumentoComercialSerie entity)
		{
			try
			{
				List<DocumentoComercialSerie> documentoComercialSerieList = Search(entity);

				return documentoComercialSerieList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(DocumentoComercialSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("correlativo", entity.Correlativo);
				parameters.AddParameter("esContingencia", entity.EsContingencia);
				parameters.AddParameter("envioEnLinea", entity.EnvioEnLinea);
				parameters.AddParameter("formato", entity.Formato);
				parameters.AddParameter("maximoItemImpresion", entity.MaximoItemImpresion);
				parameters.AddParameter("archivoFormato", entity.ArchivoFormato);
				parameters.AddParameter("tipoDocumentoComercialRelacionadoId", entity.TipoDocumentoComercialRelacionadoId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
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
		public int Update(DocumentoComercialSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("documentoComercialSerieId", entity.DocumentoComercialSerieId);
				parameters.AddParameter("esContingencia", entity.EsContingencia);
				parameters.AddParameter("envioEnLinea", entity.EnvioEnLinea);
				parameters.AddParameter("formato", entity.Formato);
				parameters.AddParameter("maximoItemImpresion", entity.MaximoItemImpresion);
				parameters.AddParameter("archivoFormato", entity.ArchivoFormato);
				parameters.AddParameter("tipoDocumentoComercialRelacionadoId", entity.TipoDocumentoComercialRelacionadoId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
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
		public void Delete(DocumentoComercialSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("documentoComercialSerieId", entity.DocumentoComercialSerieId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarCorrelativo(DocumentoComercialSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("documentoComercialSerieId", entity.DocumentoComercialSerieId);
				parameters.AddParameter("correlativo", entity.Correlativo);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCorrelativo"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
