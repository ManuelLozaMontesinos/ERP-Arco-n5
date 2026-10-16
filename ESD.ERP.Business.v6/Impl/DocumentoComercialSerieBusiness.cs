using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class DocumentoComercialSerieBusiness : IDocumentoComercialSerieBusiness
	{
		private readonly IDocumentoComercialSerieRepository documentoComercialSerieRepository;

		public DocumentoComercialSerieBusiness(
			IDocumentoComercialSerieRepository documentoComercialSerieRepository
			)
		{
			this.documentoComercialSerieRepository = documentoComercialSerieRepository;
		}
		public DocumentoComercialSerieDTO GetById(int id)
		{
			try
			{
				DocumentoComercialSerie documentoComercialSerie = documentoComercialSerieRepository.GetById(id);

				DocumentoComercialSerieDTO documentoComercialSerieDTO = Mapper.Map<DocumentoComercialSerieDTO>(documentoComercialSerie);

				return documentoComercialSerieDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<DocumentoComercialSerieDTO> Search(DocumentoComercialSerieDTO dto)
		{
			try
			{
				List<DocumentoComercialSerie> documentoComercialSerieList = documentoComercialSerieRepository.Search(Mapper.Map<DocumentoComercialSerie>(dto));

				List<DocumentoComercialSerieDTO> documentoComercialSerieDTOList = Mapper.Map<List<DocumentoComercialSerieDTO>>(documentoComercialSerieList);

				return documentoComercialSerieDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public DocumentoComercialSerieDTO SingleSearch(DocumentoComercialSerieDTO dto)
		{
			try
			{
				DocumentoComercialSerie documentoComercialSerie = documentoComercialSerieRepository.SingleSearch(Mapper.Map<DocumentoComercialSerie>(dto));

				DocumentoComercialSerieDTO documentoComercialSerieDTO = Mapper.Map<DocumentoComercialSerieDTO>(documentoComercialSerie);

				return documentoComercialSerieDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(DocumentoComercialSerieDTO dto)
		{
			try
			{
				DocumentoComercialSerie documentoComercialSerie = Mapper.Map<DocumentoComercialSerie>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = documentoComercialSerieRepository.Insert(documentoComercialSerie);

					dto.DocumentoComercialSerieId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(DocumentoComercialSerieDTO dto)
		{
			try
			{
				DocumentoComercialSerie documentoComercialSerie = Mapper.Map<DocumentoComercialSerie>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = documentoComercialSerieRepository.Update(documentoComercialSerie);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(DocumentoComercialSerieDTO dto)
		{
			try
			{
				DocumentoComercialSerie documentoComercialSerie = Mapper.Map<DocumentoComercialSerie>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					documentoComercialSerieRepository.Delete(documentoComercialSerie);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public string ObtenerCorrelativo(DocumentoComercialSerieDTO dto)
		{

			DocumentoComercialSerie documentoComercialSerie = documentoComercialSerieRepository.SingleSearch(Mapper.Map<DocumentoComercialSerie>(dto));

				documentoComercialSerie.Correlativo += 1;
			documentoComercialSerieRepository.ActualizarCorrelativo(documentoComercialSerie);

			return documentoComercialSerie.Correlativo.ToString(documentoComercialSerie.Formato.Trim());
		}
		public string FormatearNumero(DocumentoComercialSerieDTO dto)
		{

			DocumentoComercialSerie documentoComercialSerie = documentoComercialSerieRepository.SingleSearch(Mapper.Map<DocumentoComercialSerie>(dto));

			return dto.Correlativo.ToString(documentoComercialSerie.Formato.Trim());
		}
	}
}
