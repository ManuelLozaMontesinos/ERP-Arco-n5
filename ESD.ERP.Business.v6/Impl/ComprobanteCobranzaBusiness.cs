using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ComprobanteCobranzaBusiness : IComprobanteCobranzaBusiness
	{
		private readonly IComprobanteCobranzaRepository comprobanteCobranzaRepository;
		private readonly IComprobanteRepository comprobanteRepository;

		public ComprobanteCobranzaBusiness(
			IComprobanteCobranzaRepository comprobanteCobranzaRepository,
			IComprobanteRepository comprobanteRepository
			)
		{
			this.comprobanteCobranzaRepository = comprobanteCobranzaRepository;
			this.comprobanteRepository = comprobanteRepository;
		}
		public ComprobanteCobranzaDTO GetById(int id)
		{
			try
			{
				ComprobanteCobranza comprobanteCobranza = comprobanteCobranzaRepository.GetById(id);

				ComprobanteCobranzaDTO comprobanteCobranzaDTO = Mapper.Map<ComprobanteCobranzaDTO>(comprobanteCobranza);

				return comprobanteCobranzaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteCobranzaDTO> Search(ComprobanteCobranzaDTO dto)
		{
			try
			{
				List<ComprobanteCobranza> comprobanteCobranzaList = comprobanteCobranzaRepository.Search(Mapper.Map<ComprobanteCobranza>(dto));

				List<ComprobanteCobranzaDTO> comprobanteCobranzaDTOList = Mapper.Map<List<ComprobanteCobranzaDTO>>(comprobanteCobranzaList);

				return comprobanteCobranzaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ComprobanteCobranzaDTO SingleSearch(ComprobanteCobranzaDTO dto)
		{
			try
			{
				ComprobanteCobranza comprobanteCobranza = comprobanteCobranzaRepository.SingleSearch(Mapper.Map<ComprobanteCobranza>(dto));

				ComprobanteCobranzaDTO comprobanteCobranzaDTO = Mapper.Map<ComprobanteCobranzaDTO>(comprobanteCobranza);

				return comprobanteCobranzaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ComprobanteCobranzaDTO dto)
		{
			try
			{
				ComprobanteCobranza comprobanteCobranza = Mapper.Map<ComprobanteCobranza>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = comprobanteCobranzaRepository.Insert(comprobanteCobranza);

					dto.ComprobanteCobranzaId = newId;

					actualizarBancarizado(dto.ComprobanteId, dto.UsuarioCreacionId);
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ComprobanteCobranzaDTO dto)
		{
			try
			{
				ComprobanteCobranza comprobanteCobranza = Mapper.Map<ComprobanteCobranza>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = comprobanteCobranzaRepository.Update(comprobanteCobranza);

					actualizarBancarizado(dto.ComprobanteId, dto.UsuarioCreacionId);
					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ComprobanteCobranzaDTO dto)
		{
			try
			{
				ComprobanteCobranza comprobanteCobranza = comprobanteCobranzaRepository.GetById(dto.ComprobanteCobranzaId);
				comprobanteCobranza.UsuarioCreacionId = dto.UsuarioCreacionId;

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteCobranzaRepository.Delete(comprobanteCobranza);

					actualizarBancarizado(comprobanteCobranza.ComprobanteId, comprobanteCobranza.UsuarioCreacionId);
					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void actualizarBancarizado(int comprobanteId, int usuarioAccionId)
      {

			List<ComprobanteCobranza> comprobanteCobranzaList = comprobanteCobranzaRepository.Search(new ComprobanteCobranza() { 
				ComprobanteId = comprobanteId,
				Estado = EstadoConstante.ACTIVO
			});

			string bancarizado = comprobanteCobranzaList.Count > 0 ? SiNoConstante.S : SiNoConstante.N;

			comprobanteRepository.ActualizarBancarizado(new Comprobante()
			{
				ComprobanteId = comprobanteId,
				Bancarizado = bancarizado,
				UsuarioCreacionId = usuarioAccionId
			});
      }
	}
}
