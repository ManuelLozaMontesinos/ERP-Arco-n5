using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using System.Linq;

namespace ESD.ERP.Business.Impl
{
   public class GuiaRemisionBusiness : IGuiaRemisionBusiness
   {
      private readonly IGuiaRemisionRepository guiaRemisionRepository;
      private readonly IGuiaRemisionProductoRepository guiaRemisionProductoRepository;
      private readonly IMovimientoAlmacenRepository movimientoAlmacenRepository;
      private readonly IMovimientoAlmacenProductoRepository movimientoAlmacenProductoRepository;
      private readonly IComprobanteRepository comprobanteRepository;
      private readonly IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository;
      private readonly IDocumentoComercialSerieBusiness documentoComercialSerieBusiness;
      private readonly IEstadoGuiaRemisionRepository estadoGuiaRemisionRepository;
      private readonly IEstadoAprobacionRepository estadoAprobacionRepository;
      private readonly IProveedorRepository proveedorRepository;
      public GuiaRemisionBusiness(
         IGuiaRemisionRepository guiaRemisionRepository,
         IGuiaRemisionProductoRepository guiaRemisionProductoRepository,
         IMovimientoAlmacenRepository movimientoAlmacenRepository,
         IMovimientoAlmacenProductoRepository movimientoAlmacenProductoRepository,
         IComprobanteRepository comprobanteRepository,
         IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository,
         IDocumentoComercialSerieBusiness documentoComercialSerieBusiness,
         IEstadoGuiaRemisionRepository estadoGuiaRemisionRepository,
         IEstadoAprobacionRepository estadoAprobacionRepository,
         IProveedorRepository proveedorRepository
         )
      {
         this.guiaRemisionRepository = guiaRemisionRepository;
         this.guiaRemisionProductoRepository = guiaRemisionProductoRepository;
         this.movimientoAlmacenRepository = movimientoAlmacenRepository;
         this.movimientoAlmacenProductoRepository = movimientoAlmacenProductoRepository;
         this.comprobanteRepository = comprobanteRepository;
         this.periodoEmpresaSedeRepository = periodoEmpresaSedeRepository;
         this.documentoComercialSerieBusiness = documentoComercialSerieBusiness;
         this.estadoGuiaRemisionRepository = estadoGuiaRemisionRepository;
         this.estadoAprobacionRepository = estadoAprobacionRepository;
         this.proveedorRepository = proveedorRepository;
      }
      public GuiaRemisionDTO GetById(int id)
      {
         try
         {
            GuiaRemision guiaRemision = guiaRemisionRepository.GetById(id);

            GuiaRemisionDTO guiaRemisionDTO = Mapper.Map<GuiaRemisionDTO>(guiaRemision);

            return guiaRemisionDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<GuiaRemisionDTO> Search(GuiaRemisionDTO dto)
      {
         try
         {
            List<GuiaRemision> guiaRemisionList = guiaRemisionRepository.Search(Mapper.Map<GuiaRemision>(dto));

            List<GuiaRemisionDTO> guiaRemisionDTOList = Mapper.Map<List<GuiaRemisionDTO>>(guiaRemisionList);

            return guiaRemisionDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public GuiaRemisionDTO SingleSearch(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = guiaRemisionRepository.SingleSearch(Mapper.Map<GuiaRemision>(dto));

            GuiaRemisionDTO guiaRemisionDTO = Mapper.Map<GuiaRemisionDTO>(guiaRemision);

            return guiaRemisionDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            int newId = 0;
            using (TransactionScope tx = new())
            {
               guiaRemision = Mapper.Map<GuiaRemision>(dto);

               newId = guiaRemisionRepository.Insert(guiaRemision);
                   
               if (dto.MovimientoAlmacenId.HasValue)
               {
                  List<MovimientoAlmacenProducto> movimientoAlmacenProductoList = movimientoAlmacenProductoRepository.Search(new MovimientoAlmacenProducto()
                  {
                     MovimientoAlmacenId = guiaRemision.MovimientoAlmacenId.Value,
                     Estado = EstadoConstante.ACTIVO
                  });

                  List<GuiaRemisionProducto> guiaRemisionProductoList = (from g in movimientoAlmacenProductoList
                                                                         select new GuiaRemisionProducto()
                                                                         {
                                                                            GuiaRemisionId = newId,
                                                                            MovimientoAlmacenProductoId = g.MovimientoAlmacenProductoId,
                                                                            CodigoProducto = string.Empty,
                                                                            Descripcion = g.Descripcion,
                                                                            Cantidad = g.Cantidad,
                                                                            PesoNeto = g.PesoNeto != decimal.Zero ? g.PesoNeto : g.Producto.PesoNeto,
                                                                            PesoBruto = g.PesoBruto != decimal.Zero ? g.PesoBruto : g.Producto.PesoBruto,
                                                                            Estado = g.Estado,
                                                                            UsuarioCreacionId = dto.UsuarioCreacionId
                                                                         }).ToList();

                  guiaRemisionProductoRepository.ActualizarLote(guiaRemisionProductoList);
               }
                            
               dto.GuiaRemisionId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            int id = 0;
            using (TransactionScope tx = new())
            {
               id = guiaRemisionRepository.Update(guiaRemision);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            EstadoGuiaRemision estadoGuiaRemision = estadoGuiaRemisionRepository.ObtenerPorCodigo(EstadoGuiaRemisionConstante.ANULADO);

            using (TransactionScope tx = new())
            {
               guiaRemision.EstadoGuiaRemisionId = estadoGuiaRemision.EntityId;
               guiaRemisionRepository.Delete(guiaRemision);

               guiaRemisionProductoRepository.EliminarPorGuiaRemisionId(new GuiaRemisionProducto()
               {
                  GuiaRemisionId = guiaRemision.EntityId,
                  UsuarioCreacionId = guiaRemision.UsuarioCreacionId
               });

               movimientoAlmacenRepository.ActualizarGuiaRemision(new MovimientoAlmacen()
               {
                  MovimientoAlmacenId = dto.MovimientoAlmacenId.Value,
                  NumeroGuiaRemision = string.Empty,
                  TieneGuiaRemision = SiNoConstante.N,
                  UsuarioCreacionId = dto.UsuarioCreacionId
               });

               if (dto.MovimientoAlmacenDTO.ComprobanteId.HasValue)
               {
                  comprobanteRepository.ActualizarNumeroGuiaRemision(new Comprobante()
                  {
                     ComprobanteId = dto.MovimientoAlmacenDTO.ComprobanteId.Value,
                     NumeroGuiaRemision = string.Empty,
                     UsuarioCreacionId = dto.UsuarioCreacionId
                  });
               }

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Emitir(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            EstadoGuiaRemision estadoGuiaRemision = estadoGuiaRemisionRepository.ObtenerPorCodigo(EstadoGuiaRemisionConstante.EMITIDO);
            EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);

            MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.GetById(dto.MovimientoAlmacenId.Value);

            int id = 0;
            using (TransactionScope tx = new())
            {
               id = guiaRemisionRepository.Update(guiaRemision);

               guiaRemision.EstadoAprobacionId = estadoAprobacion.EntityId;
               guiaRemision.EstadoGuiaRemisionId = estadoGuiaRemision.EntityId;
               guiaRemisionRepository.ActualizarEstado(guiaRemision);

               guiaRemisionRepository.ActualizarEstadoAprobacion(new GuiaRemision()
               {
                  GuiaRemisionId = dto.GuiaRemisionId,
                  EstadoAprobacionId = estadoAprobacion.EntityId
               });

               if(movimientoAlmacen.GuiaRemisionEnModificacion == SiNoConstante.S)
               {
                  movimientoAlmacenRepository.ActualizarTieneGuiaRemision(new MovimientoAlmacen()
                  {
                     MovimientoAlmacenId = guiaRemision.MovimientoAlmacenId.Value,
                     TieneGuiaRemision = SiNoConstante.S,
                     GuiaRemisionEnModificacion = SiNoConstante.N,
                     UsuarioCreacionId = guiaRemision.UsuarioCreacionId
                  });
               }

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void EmitirSunat(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            List<GuiaRemisionProducto> guiaRemisionProductoList = guiaRemisionProductoRepository.Search(new GuiaRemisionProducto()
            {
               GuiaRemisionId = dto.GuiaRemisionId,
               Estado = EstadoConstante.ACTIVO
            });

            if (guiaRemisionProductoList.Count == 0)
               throw new Exception(SR.GetString("EX_guia_remision_sin_items"));

            MovimientoAlmacen movimientoAlmacen = null;

            if(dto.MovimientoAlmacenId.HasValue)
               movimientoAlmacen = movimientoAlmacenRepository.GetById(dto.MovimientoAlmacenId.Value);

            PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(guiaRemision.PeriodoEmpresaSedeId);

            using (TransactionScope tx = new())
            {
               if(string.IsNullOrEmpty(guiaRemision.Numero.Trim()))
                  guiaRemision.Numero = documentoComercialSerieBusiness.ObtenerCorrelativo(new DocumentoComercialSerieDTO()
                  {
                     EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId },
                     TipoDocumentoComercialId = guiaRemision.TipoDocumentoComercialId,
                     Serie = guiaRemision.Serie
                  });

               guiaRemisionRepository.ActualizarEmision(guiaRemision);

               if(movimientoAlmacen != null)
               {
                  string numeroGuiaRemision = string.Concat(guiaRemision.Serie, SeparadorConstante.GUION, guiaRemision.Numero);

                  movimientoAlmacenRepository.ActualizarGuiaRemision(new MovimientoAlmacen()
                  {
                     MovimientoAlmacenId = movimientoAlmacen.MovimientoAlmacenId,
                     NumeroGuiaRemision = numeroGuiaRemision,
                     TieneGuiaRemision = SiNoConstante.S,
                     UsuarioCreacionId = guiaRemision.UsuarioCreacionId
                  });

                  if (movimientoAlmacen.ComprobanteId.HasValue)
                     comprobanteRepository.ActualizarNumeroGuiaRemision(new Comprobante()
                     {
                        ComprobanteId = movimientoAlmacen.ComprobanteId.Value,
                        NumeroGuiaRemision = numeroGuiaRemision,
                        UsuarioCreacionId = guiaRemision.UsuarioCreacionId
                     });
               }

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void GenerarDesdeComprobante(GuiaRemisionDTO dto)
      {

         try
         {

            MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.SingleSearch(new MovimientoAlmacen() { 
               PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
               AlmacenId = dto.MovimientoAlmacenDTO.AlmacenId,
               TransaccionAlmacen = new TransaccionAlmacen() { Codigo = TransaccionAlmacenConstante.SALIDA_POR_VENTAS},
               ComprobanteId = dto.MovimientoAlmacenDTO.ComprobanteId.Value
            });

            dto.MovimientoAlmacenId = movimientoAlmacen.EntityId;
            dto.ProveedorDestinoId = movimientoAlmacen.ProveedorId.Value;
            dto.CodigoSunatPartida = movimientoAlmacen.Almacen.CodigoSunat;

            PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(dto.PeriodoEmpresaSedeId);
            Proveedor proveedor = proveedorRepository.SingleSearch(new Proveedor()
            {
               EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId,
               NumeroRuc = periodoEmpresaSede.EmpresaSede.Empresa.RUC
            });

            dto.ProveedorId = proveedor.EntityId;

            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            EstadoGuiaRemision estadoGuiaRemision = estadoGuiaRemisionRepository.ObtenerPorCodigo(EstadoGuiaRemisionConstante.EN_EDICION);
            EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);

            int newId = 0;
            using (TransactionScope tx = new())
            {

               guiaRemision = Mapper.Map<GuiaRemision>(dto);
               guiaRemision.EstadoAprobacionId = estadoAprobacion.EntityId;
               guiaRemision.EstadoGuiaRemisionId = estadoGuiaRemision.EntityId;
               guiaRemision.CodigoSunatLlegada = string.Empty;
               guiaRemision.Observacion = string.Empty;
               guiaRemision.ObservacionAnulacion = string.Empty;

               newId = guiaRemisionRepository.Insert(guiaRemision);

               List<MovimientoAlmacenProducto> movimientoAlmacenProductoList = movimientoAlmacenProductoRepository.Search(new MovimientoAlmacenProducto()
               {
                  MovimientoAlmacenId = guiaRemision.MovimientoAlmacenId.Value,
                  Estado = EstadoConstante.ACTIVO
               });

               List<GuiaRemisionProducto> guiaRemisionProductoList = (from g in movimientoAlmacenProductoList
                                                                      select new GuiaRemisionProducto()
                                                                      {
                                                                         GuiaRemisionId = newId,
                                                                         MovimientoAlmacenProductoId = g.MovimientoAlmacenProductoId,
                                                                         CodigoProducto = string.Empty,
                                                                         Descripcion = g.Descripcion,
                                                                         Cantidad = g.Cantidad,
                                                                         Estado = g.Estado,
                                                                         UsuarioCreacionId = dto.UsuarioCreacionId
                                                                      }).ToList();

               guiaRemisionProductoRepository.ActualizarLote(guiaRemisionProductoList);
                
               guiaRemisionRepository.ActualizarPeso(new GuiaRemision()
               {
                   MovimientoAlmacenId = dto.MovimientoAlmacenId,
                   GuiaRemisionId = dto.GuiaRemisionId
               });
                 
               tx.Complete();
            }

         }
         catch (Exception)
         {
            throw;
         }
      }
      public void SolicitarModificacion(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);
            EstadoGuiaRemision estadoGuiaRemision = estadoGuiaRemisionRepository.ObtenerPorCodigo(EstadoGuiaRemisionConstante.MODIFICACION);

            using (TransactionScope tx = new())
            {
               guiaRemisionRepository.ActualizarEstado(new GuiaRemision()
               {
                  GuiaRemisionId = dto.GuiaRemisionId,
                  EstadoGuiaRemisionAnteriorId = guiaRemision.EstadoGuiaRemisionId,
                  EstadoGuiaRemisionId = estadoGuiaRemision.EstadoGuiaRemisionId,
                  UsuarioCreacionId = dto.UsuarioCreacionId,
               });

               guiaRemisionRepository.ActualizarEstadoAprobacion(new GuiaRemision()
               {
                  GuiaRemisionId = dto.GuiaRemisionId,
                  EstadoAprobacionId = estadoAprobacion.EntityId
               });

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Aprobar(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = guiaRemisionRepository.GetById(dto.Id);

            EstadoAprobacion estadoAprobacion = null;

            if (guiaRemision.EstadoGuiaRemision.Codigo != EstadoGuiaRemisionConstante.MODIFICACION)
               estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);

            using (TransactionScope tx = new())
            {

               guiaRemisionRepository.ActualizarEstadoAprobacion(new GuiaRemision()
               {
                  GuiaRemisionId = dto.GuiaRemisionId,
                  EstadoAprobacionId = estadoAprobacion?.EntityId
               });

               movimientoAlmacenRepository.ActualizarTieneGuiaRemision(new MovimientoAlmacen()
               {
                  MovimientoAlmacenId = guiaRemision.MovimientoAlmacenId.Value,
                  TieneGuiaRemision = SiNoConstante.N,
                  GuiaRemisionEnModificacion = SiNoConstante.S,
                  UsuarioCreacionId = guiaRemision.UsuarioCreacionId
               });

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Desaprobar(GuiaRemisionDTO dto)
      {
         GuiaRemision guiaRemision = guiaRemisionRepository.GetById(dto.Id);

         EstadoAprobacion estadoAprobacion = null;

         if (guiaRemision.EstadoGuiaRemision.Codigo == EstadoOrdenCompraConstante.MODIFICACION)
            estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);
         else
            estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);

         using (TransactionScope tx = new())
         {
            if (guiaRemision.EstadoGuiaRemision.Codigo == EstadoGuiaRemisionConstante.MODIFICACION)
            {
               guiaRemisionRepository.ActualizarEstado(new GuiaRemision()
               {
                  GuiaRemisionId = dto.GuiaRemisionId,
                  EstadoGuiaRemisionAnteriorId = null,
                  EstadoGuiaRemisionId = guiaRemision.EstadoGuiaRemisionAnteriorId.Value,
                  UsuarioCreacionId = dto.UsuarioCreacionId,
               });
            }

            guiaRemisionRepository.ActualizarEstadoAprobacion(new GuiaRemision()
            {
               GuiaRemisionId = dto.GuiaRemisionId,
               EstadoAprobacionId = estadoAprobacion.EntityId
            });

            tx.Complete();
         }
      }
      public void ActualizarEstado(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            using (TransactionScope tx = new())
            {
               guiaRemisionRepository.ActualizarEstado(guiaRemision);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void ActualizarEmisionElectronica(GuiaRemisionDTO dto)
      {
         try
         {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            using (TransactionScope tx = new())
            {
               guiaRemisionRepository.ActualizarEmisionElectronica(guiaRemision);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void ActualizarPeso(GuiaRemisionDTO dto)
      {
        try
        {
            GuiaRemision guiaRemision = Mapper.Map<GuiaRemision>(dto);

            using (TransactionScope tx = new())
                {
                    guiaRemisionRepository.ActualizarPeso(new GuiaRemision()
                    {
                        MovimientoAlmacenId = dto.MovimientoAlmacenId
                    });

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
