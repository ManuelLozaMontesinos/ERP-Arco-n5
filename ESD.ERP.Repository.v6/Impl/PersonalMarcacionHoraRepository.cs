using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalMarcacionHoraRepository : IPersonalMarcacionHoraRepository
	{
		private const string SP_PRE_NAME = "sp_perso_m_hora";
		public PersonalMarcacionHora GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalMarcacionHora)
					},
					x =>
					{
						PersonalMarcacionHora pmh = x[0] as PersonalMarcacionHora;

						return pmh;
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
		public List<PersonalMarcacionHora> Search(PersonalMarcacionHora entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalMarcacionHora)
					},
					x =>
					{
						PersonalMarcacionHora pmh = x[0] as PersonalMarcacionHora;

						return pmh;
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
		public PersonalMarcacionHora SingleSearch(PersonalMarcacionHora entity)
		{
			try
			{
				List<PersonalMarcacionHora> personalMarcacionHoraList = Search(entity);

				return personalMarcacionHoraList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalMarcacionHora entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalMarcacionId", entity.PersonalMarcacionId);
				parameters.AddParameter("roraRemunerativaId", entity.HoraRemunerativaId);
				parameters.AddParameter("cantidad", entity.Cantidad);
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
		public int Update(PersonalMarcacionHora entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalMarcacionHoraId", entity.PersonalMarcacionHoraId);
				parameters.AddParameter("personalMarcacionId", entity.PersonalMarcacionId);
				parameters.AddParameter("horaRemunerativaId", entity.HoraRemunerativaId);
				parameters.AddParameter("cantidad", entity.Cantidad);
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
		public void Delete(PersonalMarcacionHora entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalMarcacionHoraId", entity.PersonalMarcacionHoraId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(List<PersonalMarcacionHora> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<PersonalMarcacionHoraType> personalMarcacionHoraTypeList = new List<PersonalMarcacionHoraType>();
				if(list != null)
            {
					personalMarcacionHoraTypeList = (from s in list
																select new PersonalMarcacionHoraType
																{
																	PersonalMarcacionHoraId = s.PersonalMarcacionHoraId,
																	PersonalMarcacionId = s.PersonalMarcacionId,
																	HoraRemunerativaId = s.HoraRemunerativaId,
																	Cantidad = s.Cantidad,
																	Estado = s.Estado,
																	UsuarioCreacionId = s.UsuarioCreacionId,
																}).ToList();
				}

				parameters.AddTableParameter("personalMarcacionHoraList", "dbo.PersonalMarcacionHoraType", personalMarcacionHoraTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
