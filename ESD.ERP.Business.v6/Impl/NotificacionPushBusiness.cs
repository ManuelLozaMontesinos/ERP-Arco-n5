using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class NotificacionPushBusiness : INotificacionPushBusiness
	{
		private readonly INotificacionPushRepository notificacionPushRepository;

		public NotificacionPushBusiness(
			INotificacionPushRepository notificacionPushRepository
			)
		{
			this.notificacionPushRepository = notificacionPushRepository;
		}
		public NotificacionPushDTO GetById(int id)
		{
			try
			{
				NotificacionPush notificacionPush = notificacionPushRepository.GetById(id);

				NotificacionPushDTO notificacionPushDTO = Mapper.Map<NotificacionPushDTO>(notificacionPush);

				return notificacionPushDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<NotificacionPushDTO> Search(NotificacionPushDTO dto)
		{
			try
			{
				List<NotificacionPush> notificacionPushList = notificacionPushRepository.Search(Mapper.Map<NotificacionPush>(dto));

				List<NotificacionPushDTO> notificacionPushDTOList = Mapper.Map<List<NotificacionPushDTO>>(notificacionPushList);

				return notificacionPushDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public NotificacionPushDTO SingleSearch(NotificacionPushDTO dto)
		{
			try
			{
				NotificacionPush notificacionPush = notificacionPushRepository.SingleSearch(Mapper.Map<NotificacionPush>(dto));

				NotificacionPushDTO notificacionPushDTO = Mapper.Map<NotificacionPushDTO>(notificacionPush);

				return notificacionPushDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(NotificacionPushDTO dto)
		{
			try
			{
				NotificacionPush notificacionPush = Mapper.Map<NotificacionPush>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = notificacionPushRepository.Insert(notificacionPush);

					dto.NotificacionPushId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(NotificacionPushDTO dto)
		{
			try
			{
				NotificacionPush notificacionPush = Mapper.Map<NotificacionPush>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = notificacionPushRepository.Update(notificacionPush);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(NotificacionPushDTO dto)
		{
			try
			{
				NotificacionPush notificacionPush = Mapper.Map<NotificacionPush>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					notificacionPushRepository.Delete(notificacionPush);

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
