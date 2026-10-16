using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RolUsuarioBusiness : IRolUsuarioBusiness
	{
		private readonly IRolUsuarioRepository rolUsuarioRepository;

		public RolUsuarioBusiness(
			IRolUsuarioRepository rolUsuarioRepository
			)
		{
			this.rolUsuarioRepository = rolUsuarioRepository;
		}
		public RolUsuarioDTO GetById(int id)
			{
				try
				{
					RolUsuario rolUsuario = rolUsuarioRepository.GetById(id);

					RolUsuarioDTO rolUsuarioDTO = Mapper.Map<RolUsuarioDTO>(rolUsuario);

					return rolUsuarioDTO;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public List<RolUsuarioDTO> Search(RolUsuarioDTO dto)
			{
				try
				{
					List<RolUsuario> rolUsuarioList = rolUsuarioRepository.Search(Mapper.Map<RolUsuario>(dto));

					List<RolUsuarioDTO> rolUsuarioDTOList = Mapper.Map<List<RolUsuarioDTO>>(rolUsuarioList);

					return rolUsuarioDTOList;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public RolUsuarioDTO SingleSearch(RolUsuarioDTO dto)
			{
				try
				{
					RolUsuario rolUsuario = rolUsuarioRepository.SingleSearch(Mapper.Map<RolUsuario>(dto));

					RolUsuarioDTO rolUsuarioDTO = Mapper.Map<RolUsuarioDTO>(rolUsuario);

					return rolUsuarioDTO;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public int Insert(RolUsuarioDTO dto)
			{
				try
				{
					RolUsuario rolUsuario = Mapper.Map<RolUsuario>(dto);

					int newId = 0;
					using (TransactionScope tx = new TransactionScope())
					{
						newId = rolUsuarioRepository.Insert(rolUsuario);

						dto.RolUsuarioId = newId;
						tx.Complete();
					}

					return newId;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public int Update(RolUsuarioDTO dto)
			{
				try
				{
					RolUsuario rolUsuario = Mapper.Map<RolUsuario>(dto);

					int id = 0;
					using (TransactionScope tx = new TransactionScope())
					{
						id = rolUsuarioRepository.Update(rolUsuario);

						tx.Complete();
					}

					return id;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public void Delete(RolUsuarioDTO dto)
			{
				try
				{
					RolUsuario rolUsuario = Mapper.Map<RolUsuario>(dto);

					using (TransactionScope tx = new TransactionScope())
					{
						rolUsuarioRepository.Delete(rolUsuario);

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
