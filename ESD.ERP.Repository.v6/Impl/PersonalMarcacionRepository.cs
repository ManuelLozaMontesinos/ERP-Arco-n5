using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalMarcacionRepository : IPersonalMarcacionRepository
	{
		private const string SP_PRE_NAME = "sp_perso_marca";
		public PersonalMarcacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalMarcacion)
					},
					x =>
					{
						PersonalMarcacion pm = x[0] as PersonalMarcacion;

						return pm;
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
		public List<PersonalMarcacion> Search(PersonalMarcacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("personalId", entity.PersonalId);
			parameters.AddParameter("fecha", entity.Fecha);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalMarcacion)
					},
					x =>
					{
						PersonalMarcacion pm = x[0] as PersonalMarcacion;

						return pm;
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
		public PersonalMarcacion SingleSearch(PersonalMarcacion entity)
		{
			try
			{
				List<PersonalMarcacion> personalMarcacionList = Search(entity);

				return personalMarcacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("horaMinuto", entity.HoraMinuto);
				parameters.AddParameter("ignorarTolerancia", entity.IgnorarTolerancia);
				parameters.AddParameter("esFalta", entity.EsFalta);
				parameters.AddParameter("esRemunerado", entity.EsRemunerado);
				parameters.AddParameter("verificado", entity.Verificado);
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
		public int Update(PersonalMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalMarcacionId", entity.PersonalMarcacionId);
				parameters.AddParameter("ignorarTolerancia", entity.IgnorarTolerancia);
				parameters.AddParameter("esFalta", entity.EsFalta);
				parameters.AddParameter("esRemunerado", entity.EsRemunerado);
				parameters.AddParameter("verificado", entity.Verificado);
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
		public void Delete(PersonalMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalMarcacionId", entity.PersonalMarcacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<dynamic> ListarMarcacionHoraPivot(PersonalMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("empresaId", entity.Personal.EmpresaSede.EmpresaId);
			parameters.AddParameter("personalMarcacionId", entity.PersonalMarcacionId);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("containsRazonSocial", entity.Personal?.Proveedor?.RazonSocial);

			try
			{
				List<dynamic> resultList = DbFactory.Query<dynamic>(DbCommand.Create(SP_PRE_NAME, "_listarPivot"), parameters);

				return resultList;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void SincronizarMarcacion(PersonalMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.Personal.EmpresaSede.EmpresaId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaDesde", entity.FechaDesde);
				parameters.AddParameter("fechaHasta", entity.FechaHasta);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_sincronizarMarcacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Personal> ListarPersonalConHoras(PersonalMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("empresaId", entity.Personal.EmpresaSede.EmpresaId);
			parameters.AddParameter("razonSocial", entity.Personal.Proveedor.RazonSocial);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPersonalConHoras"), parameters,
					new[]
					{
						typeof(Personal),
						typeof(Proveedor),
						typeof(TipoDocumentoIdentidad)
					},
					x =>
					{
						Personal p = x[0] as Personal;
						Proveedor prv = x[1] as Proveedor;
						TipoDocumentoIdentidad tdi = x[2] as TipoDocumentoIdentidad;

						p.Proveedor = prv;
						p.ProveedorId = prv.EntityId;
						prv.TipoDocumentoIdentidad = tdi;
						prv.TipoDocumentoIdentidadId = tdi.EntityId;

						return p;
					},
					"personal_id,proveedor_id,tipo_doc_iden_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}
