using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class VendedorSerieRepository : IVendedorSerieRepository
	{
		private const string SP_PRE_NAME = "sp_vend_serie";
		public VendedorSerie GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(VendedorSerie)
					},
					x =>
					{
						VendedorSerie vs = x[0] as VendedorSerie;

						return vs;
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
		public List<VendedorSerie> Search(VendedorSerie entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("vendedorId", entity.VendedorId);
			parameters.AddParameter("documentoComercialSerieEmpresaSedeId", entity.DocumentoComercialSerie?.EmpresaSedeId);
			parameters.AddParameter("tipoDocumentoComercialId", entity.DocumentoComercialSerie?.TipoDocumentoComercialId);
			parameters.AddParameter("tipoDocumentoComercial", entity.DocumentoComercialSerie?.TipoDocumentoComercial?.Codigo);
			parameters.AddParameter("esComprobantePago", entity.DocumentoComercialSerie?.TipoDocumentoComercial?.EsComprobantePago);
			parameters.AddParameter("esInterno", entity.DocumentoComercialSerie?.TipoDocumentoComercial?.EsInterno);
			parameters.AddParameter("tipoDocumentoComercialRelacionadoId", entity.DocumentoComercialSerie?.TipoDocumentoComercialRelacionadoId);
			parameters.AddParameter("almacenIdNotNull", entity.DocumentoComercialSerie?.AlmacenIdNotNull);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(VendedorSerie),
						typeof(DocumentoComercialSerie),
						typeof(Almacen),
						typeof(EmpresaSede),
						typeof(TipoDocumentoComercial)
					},
					x =>
					{
						VendedorSerie vs = x[0] as VendedorSerie;
						DocumentoComercialSerie dcs = x[1] as DocumentoComercialSerie;
						Almacen a = x[2] as Almacen;
						EmpresaSede es = x[3] as EmpresaSede;
						TipoDocumentoComercial tdc = x[4] as TipoDocumentoComercial;

						vs.DocumentoComercialSerie = dcs;
						vs.DocumentoComercialSerieId = dcs.EntityId;
						dcs.Almacen = a;
						dcs.AlmacenId = a?.EntityId;
						dcs.EmpresaSede = es;
						dcs.EmpresaSedeId = es.EntityId;
						dcs.TipoDocumentoComercial = tdc;
						dcs.TipoDocumentoComercialId = tdc.EntityId;

						return vs;
					},
					"doc_com_serie_id,almacen_id,empresa_sede_id,tipo_doc_com_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public VendedorSerie SingleSearch(VendedorSerie entity)
		{
			try
			{
				List<VendedorSerie> vendedorSerieList = Search(entity);

				return vendedorSerieList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(VendedorSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vendedorId", entity.VendedorId);
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
		public int Update(VendedorSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vendedorSerieId", entity.VendedorSerieId);
				parameters.AddParameter("estado", entity.Estado);
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
		public void Delete(VendedorSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vendedorSerieId", entity.VendedorSerieId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarListaDocumentoComercialSerieId(VendedorSerie entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vendedorId", entity.VendedorId);
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
