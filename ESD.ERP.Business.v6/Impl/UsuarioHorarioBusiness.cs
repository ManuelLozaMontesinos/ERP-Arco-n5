using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class UsuarioHorarioBusiness : IUsuarioHorarioBusiness
	{
		private readonly IUsuarioHorarioRepository usuarioHorarioRepository;

		public UsuarioHorarioBusiness(
			IUsuarioHorarioRepository usuarioHorarioRepository
			)
		{
			this.usuarioHorarioRepository = usuarioHorarioRepository;
		}
		public UsuarioHorarioDTO GetById(int id)
		{
			try
			{
				UsuarioHorario usuarioHorario = usuarioHorarioRepository.GetById(id);

				UsuarioHorarioDTO usuarioHorarioDTO = Mapper.Map<UsuarioHorarioDTO>(usuarioHorario);

				return usuarioHorarioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<UsuarioHorarioDTO> Search(UsuarioHorarioDTO dto)
		{
			try
			{
				List<UsuarioHorario> usuarioHorarioList = usuarioHorarioRepository.Search(Mapper.Map<UsuarioHorario>(dto));

				List<UsuarioHorarioDTO> usuarioHorarioDTOList = Mapper.Map<List<UsuarioHorarioDTO>>(usuarioHorarioList);

				return usuarioHorarioDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public UsuarioHorarioDTO SingleSearch(UsuarioHorarioDTO dto)
		{
			try
			{
				UsuarioHorario usuarioHorario = usuarioHorarioRepository.SingleSearch(Mapper.Map<UsuarioHorario>(dto));

				UsuarioHorarioDTO usuarioHorarioDTO = Mapper.Map<UsuarioHorarioDTO>(usuarioHorario);

				return usuarioHorarioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(UsuarioHorarioDTO dto)
		{
			try
			{
				UsuarioHorario usuarioHorario = Mapper.Map<UsuarioHorario>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = usuarioHorarioRepository.Insert(usuarioHorario);

					dto.UsuarioHorarioId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(UsuarioHorarioDTO dto)
		{
			try
			{
				UsuarioHorario usuarioHorario = Mapper.Map<UsuarioHorario>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = usuarioHorarioRepository.Update(usuarioHorario);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(UsuarioHorarioDTO dto)
		{
			try
			{
				UsuarioHorario usuarioHorario = Mapper.Map<UsuarioHorario>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					usuarioHorarioRepository.Delete(usuarioHorario);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<UsuarioHorarioDTO> ListarPorUsuarioId(UsuarioHorarioDTO dto)
      {
			try
			{
				List<UsuarioHorario> usuarioHorarioList = usuarioHorarioRepository.ListarPorUsuarioId(Mapper.Map<UsuarioHorario>(dto));

				List<UsuarioHorarioDTO> usuarioHorarioDTOList = Mapper.Map<List<UsuarioHorarioDTO>>(usuarioHorarioList);

				return usuarioHorarioDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarLote(List<UsuarioHorarioDTO> list)
		{
			try
			{
				List<UsuarioHorario> usuarioHorarioList = Mapper.Map<List<UsuarioHorario>>(list);

				using (TransactionScope tx = new())
				{
					usuarioHorarioRepository.ActualizarLote(usuarioHorarioList);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
    }	
}
