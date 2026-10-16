using System.Collections.Generic;

namespace ESD.Core.Data
{
   public interface IGenericRepository<T> where T : class
   {
      T GetById(int id);
      List<T> Search(T entity);
      T SingleSearch(T entity);
      int Insert(T entity);
      int Update(T entity);
      void Delete(T entity);
   }
}