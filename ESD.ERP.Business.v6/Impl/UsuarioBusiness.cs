using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.Core.Cryptography;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class UsuarioBusiness : IUsuarioBusiness
   {
      private readonly IUsuarioRepository usuarioRepository;
      private readonly IProveedorRepository proveedorRepository;

      public UsuarioBusiness(
         IUsuarioRepository usuarioRepository,
         IProveedorRepository proveedorRepository
         )
      {
         this.usuarioRepository = usuarioRepository;
         this.proveedorRepository = proveedorRepository;
      }
      public UsuarioDTO GetById(int id)
      {
         try
         {
            Usuario usuario = usuarioRepository.GetById(id);

            UsuarioDTO usuarioDTO = Mapper.Map<UsuarioDTO>(usuario);

            return usuarioDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<UsuarioDTO> Search(UsuarioDTO dto)
      {
         try
         {
            List<Usuario> usuarioList = usuarioRepository.Search(Mapper.Map<Usuario>(dto));

            List<UsuarioDTO> usuarioDTOList = Mapper.Map<List<UsuarioDTO>>(usuarioList);

            return usuarioDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public UsuarioDTO SingleSearch(UsuarioDTO dto)
      {
         try
         {
            Usuario usuario = usuarioRepository.SingleSearch(Mapper.Map<Usuario>(dto));

            UsuarioDTO usuarioDTO = Mapper.Map<UsuarioDTO>(usuario);

            return usuarioDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(UsuarioDTO dto)
      {
         try
         {
            Usuario usuario = usuarioRepository.ObtenerPorCodigo(dto.UsuarioLogon);

            if (usuario != null)
               throw new Exception(SR.GetString("EX_usuario_ya_registrado", usuario.UsuarioLogon));

            Cryptor cryptor = new Cryptor();

            Guid guid = Guid.NewGuid();
            dto.ClavePublica = guid.ToByteArray();
            dto.Password = cryptor.Encrypt(dto.Password, dto.ClavePublica);

            usuario = Mapper.Map<Usuario>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = usuarioRepository.Insert(usuario);
               usuario.UsuarioId = newId;

               usuarioRepository.ActualizarEstado(usuario);

               if (dto.ProveedorId.HasValue)
               {
                  Proveedor proveedor = proveedorRepository.SingleSearch(new Proveedor()
                  {
                     EmpresaId = dto.ProveedorDTO.EmpresaId,
                     UsuarioId = newId
                  });

                  if(proveedor != null)
                  {
                     if(proveedor.EntityId != dto.ProveedorDTO.Id)
                     {
                        proveedor.UsuarioId = null;
                        proveedorRepository.ActualizarUsuario(proveedor);

                        proveedor.ProveedorId = dto.ProveedorDTO.Id;
                        proveedor.UsuarioId = usuario.EntityId;
                        proveedorRepository.ActualizarUsuario(proveedor);
                     }
                  }
                  else
                  {
                     proveedor = new Proveedor();
                     proveedor.ProveedorId = dto.ProveedorDTO.Id;
                     proveedor.UsuarioId = newId;
                     proveedorRepository.ActualizarUsuario(proveedor);
                  }
               }

               dto.UsuarioId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(UsuarioDTO dto)
      {
         try
         {
            Usuario usuario = Mapper.Map<Usuario>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = usuarioRepository.Update(usuario);
               usuarioRepository.ActualizarEstado(usuario);

               if (dto.ProveedorId.HasValue)
               {
                  Proveedor proveedor = proveedorRepository.SingleSearch(new Proveedor()
                  {
                     EmpresaId = dto.ProveedorDTO.EmpresaId,
                     UsuarioId = usuario.EntityId
                  });

                  if (proveedor != null)
                  {
                     if (proveedor.EntityId != dto.ProveedorDTO.Id)
                     {
                        proveedor.UsuarioId = null;
                        proveedorRepository.ActualizarUsuario(proveedor);

                        proveedor.ProveedorId = dto.ProveedorDTO.Id;
                        proveedor.UsuarioId = usuario.EntityId;
                        proveedorRepository.ActualizarUsuario(proveedor);
                     }
                  }
                  else
                  {
                     proveedor = new Proveedor();
                     proveedor.ProveedorId = dto.ProveedorDTO.Id;
                     proveedor.UsuarioId = usuario.EntityId;
                     proveedorRepository.ActualizarUsuario(proveedor);
                  }
               }

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(UsuarioDTO dto)
      {
         try
         {
            Usuario usuario = Mapper.Map<Usuario>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               usuarioRepository.Delete(usuario);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public UsuarioDTO ObtenerPorCodigo(string codigo)
      {
         try
         {
            Usuario usuario = usuarioRepository.ObtenerPorCodigo(codigo);

            UsuarioDTO usuarioDTO = Mapper.Map<UsuarioDTO>(usuario);

            return usuarioDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void ActualizarEstado(UsuarioDTO dto)
      {
         try
         {
            Usuario usuario = Mapper.Map<Usuario>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               usuarioRepository.ActualizarEstado(usuario);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void ActualizarContrasena(UsuarioDTO dto)
      {
         try
         {
            Usuario usuario = usuarioRepository.GetById(dto.Id);

            Cryptor cryptor = new Cryptor();

            dto.Password = cryptor.Encrypt(dto.Password, usuario.ClavePublica);

            usuario = Mapper.Map<Usuario>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               usuarioRepository.ActualizarContrasena(usuario);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void ActualizarContrasenaAnterior(UsuarioDTO dto)
      {
         try
         {

            Cryptor cryptor = new Cryptor();

            UsuarioDTO usuarioDTO = GetById(dto.Id);
            usuarioDTO.CambiarContrasenaProximo = SiNoConstante.N;

            string decryptPassword = cryptor.Decrypt(usuarioDTO.Password, usuarioDTO.ClavePublica);

            if (decryptPassword != dto.ContrasenaAnterior)
               throw new Exception(SR.GetString("EX_usuario_contrasena_anterior_incorrecta"));

            Usuario usuario = usuarioRepository.GetById(dto.Id);

            dto.Password = cryptor.Encrypt(dto.Password, usuario.ClavePublica);

            usuario = Mapper.Map<Usuario>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               usuario.CambiarContrasenaProximo = SiNoConstante.N;
               usuarioRepository.ActualizarContrasena(usuario);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public bool ValidarContrasena(UsuarioDTO dto)
      {
         try
         {

            Cryptor cryptor = new Cryptor();

            UsuarioDTO usuarioDTO = GetById(dto.Id);

            string decryptPassword = cryptor.Decrypt(usuarioDTO.Password, usuarioDTO.ClavePublica);

            if (decryptPassword == dto.Password)
               return true;

            return false;
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}