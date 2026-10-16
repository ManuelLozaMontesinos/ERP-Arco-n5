using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ParametroSistemaRepository : IParametroSistemaRepository
	{
		private const string SP_PRE_NAME = "sp_parametro_sistema";
		public ParametroSistema GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ParametroSistema),
						typeof(GrupoParametroSistema),
						typeof(TipoDato)
					},
					x =>
					{
						ParametroSistema ps = x[0] as ParametroSistema;
						GrupoParametroSistema gps = x[1] as GrupoParametroSistema;
						TipoDato td = x[2] as TipoDato;

						ps.GrupoParametroSistemaId = gps.EntityId;
						ps.GrupoParametroSistema = gps;
						ps.TipoDatoId = td.EntityId;
						ps.TipoDato = td;

						return ps;
					},
					"grupo_param_sistema_id,tipo_dato_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ParametroSistema> Search(ParametroSistema entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("grupoParametroSistema", entity.GrupoParametroSistema?.Codigo);
			parameters.AddParameter("parametroSistema", entity.Codigo);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ParametroSistema),
						typeof(GrupoParametroSistema),
						typeof(TipoDato)
					},
					x =>
					{
						ParametroSistema ps = x[0] as ParametroSistema;
						GrupoParametroSistema gps = x[1] as GrupoParametroSistema;
						TipoDato td = x[2] as TipoDato;

						ps.GrupoParametroSistemaId = gps.EntityId;
						ps.GrupoParametroSistema = gps;
						ps.TipoDatoId = td.EntityId;
						ps.TipoDato = td;

						return ps;
					},
					"grupo_param_sistema_id,tipo_dato_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ParametroSistema SingleSearch(ParametroSistema entity)
		{
			try
			{
				List<ParametroSistema> parametroSistemaList = Search(entity);

				return parametroSistemaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ParametroSistema entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoParamSistemaId", entity.GrupoParametroSistemaId);
				parameters.AddParameter("tipoDatoId", entity.TipoDatoId);
				parameters.AddParameter("parametroSistema", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("rangoNumDesde", entity.RangoNumericoDesde);
				parameters.AddParameter("rangoNumHasta", entity.RangoNumericoHasta);
				parameters.AddParameter("valorNumerico", entity.ValorNumerico);
				parameters.AddParameter("valorFecha", entity.ValorFecha);
				parameters.AddParameter("valorCadena", entity.ValorCadena);
				parameters.AddParameter("ordenGrupo", entity.OrdenGrupo);
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
		public int Update(ParametroSistema entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("parametroSistemaId", entity.ParametroSistemaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("rangoNumericoDesde", entity.RangoNumericoDesde);
				parameters.AddParameter("rangoNumericoHasta", entity.RangoNumericoHasta);
				parameters.AddParameter("valorNumerico", entity.ValorNumerico);
				parameters.AddParameter("valorFecha", entity.ValorFecha);
				parameters.AddParameter("valorCadena", entity.ValorCadena);
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
		public void Delete(ParametroSistema entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("parametroSistemaId", entity.ParametroSistemaId);
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
