using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Calcom.Domain
{
    public class Repository<TObject> where TObject : class
    {
        protected DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public ICollection<TObject> GetAll()
        {
            return _context.Set<TObject>().ToList();
        }

        public async Task<ICollection<TObject>> GetAllAsync()
        {
            return await _context.Set<TObject>().ToListAsync();
        }

        public TObject Get(int id)
        {
            return _context.Set<TObject>().Find(id);
        }
        public TObject Get(long id)
        {
            return _context.Set<TObject>().Find(id);
        }
        public async Task<TObject> GetAsync(int id)
        {
            return await _context.Set<TObject>().FindAsync(id);
        }

        public TObject Find(Expression<Func<TObject, bool>> match)
        {
            return _context.Set<TObject>().SingleOrDefault(match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match)
        {
            return await _context.Set<TObject>().SingleOrDefaultAsync(match);
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match)
        {
            return _context.Set<TObject>().Where(match).ToList();
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match)
        {
            return await _context.Set<TObject>().Where(match).ToListAsync();
        }

        public TObject Add(TObject t)
        {
            _context.Set<TObject>().Add(t);
            _context.SaveChanges();
            return t;
        }

        public async Task<TObject> AddAsync(TObject t)
        {
            _context.Set<TObject>().Add(t);
            await _context.SaveChangesAsync();
            return t;
        }

        public TObject Update(TObject updated, long key)
        {
            if (updated == null)
                return null;

            TObject existing = _context.Set<TObject>().Find(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                _context.SaveChanges();
            }
            return existing;
        }

        public async Task<TObject> UpdateAsync(TObject updated, int key)
        {
            if (updated == null)
                return null;

            TObject existing = await _context.Set<TObject>().FindAsync(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
            return existing;
        }

        public void Delete(TObject t)
        {
            _context.Set<TObject>().Remove(t);
            _context.SaveChanges();
        }

        public async Task<int> DeleteAsync(TObject t)
        {
            _context.Set<TObject>().Remove(t);
            return await _context.SaveChangesAsync();
        }

        public int Count()
        {
            return _context.Set<TObject>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TObject>().CountAsync();
        }



        //public int GetId(string sql, params object[] parameters)
        //{
        //    ObjectContext objContext = ((IObjectContextAdapter)_context).ObjectContext;

        //    EntityConnection con = new EntityConnection("name=CalComEntities");

        //        con.Open();

        //        EntityCommand command = con.CreateCommand();
        //        command.CommandText = sql;
        //        foreach(var par in parameters)
        //        command.Parameters.Add((EntityParameter)par);
        //        EntityDataReader reader = command.ExecuteReader();
        //                int id = reader.GetInt32(0);
        //        return id;

        //}
        /*
         * Id yi almadan veritabanına ekleme
         */ 
        public int Insert(string Id, params object[] parameters)
        {
            string sql = "INSERT INTO [dbo].[" + typeof(TObject).Name + "] (";
            string[] sqlParameter = new string[parameters.Length];
            for (int j = 0; j < parameters.Length; j++)
            {
                sqlParameter[j] = ((SqlParameter)parameters[j]).ParameterName.Remove(0, 1);
                sql = sql + "[" + sqlParameter[j] + "] ,";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            sql = sql + ") VALUES ( ";

            for (int i = 1; i <= parameters.Length; i++)
            {
                sql = sql + " @" + sqlParameter[i - 1] + " , ";
            }

            sql = sql.Remove(sql.Length - 2, 1);
            sql = sql + ")";
            int result = _context.Database.ExecuteSqlCommand(sql, parameters);
            _context.SaveChanges();

          //  string sqlGet = "Select [" + Id + "] FROM [" + typeof(TObject).Name + "] WHERE ";
          //  for (int j = 0; j < parameters.Length; j++)
          //  {
          //      sqlParameter[j] = ((SqlParameter)parameters[j]).ParameterName.Remove(0, 1);
          //      sqlGet = sqlGet + "[" + sqlParameter[j] + "] = " + " @" + sqlParameter[j] + " AND ";
          //  }


          //  sqlGet = sqlGet.Remove(sqlGet.Length - 4, 3);
          //  object[] pars = new object[parameters.Length];
          //  int k = 0;
          //  foreach (object p in parameters)
          //      pars[k++] = new SqlParameter { ParameterName = ((SqlParameter)p).ParameterName, Value = ((SqlParameter)p).Value };
          //  //int id = GetId(sqlGet, pars);
          //  int id = 0;
          //var x =  _context.Set<TObject>().SqlQuery(sqlGet, pars);
          //foreach (var m in x)
          //    id = ((TObject) m).getId();
            return result;
        }

    }
}
