using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Calcom.Domain.Security
{
    class CustomRoleProvider : RoleProvider
    {
        CalComEntities db;
        public CustomRoleProvider()
        {
            db = new CalComEntities();
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName {get; set;}
        

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            Repository<Laboratuvarlar> labRepository = new Repository<Laboratuvarlar>(db);
            if (username == null || username == "")
                throw new ProviderException("User name cannot be empty or null.");

          
                           
            //db deki her bir rol öyle ki username'in rolleri tmpRoleName' e eklenir, aralara virguller eklenerek, 
            //bir tek lab var oyuzden simdilik boyle yaptık, admin icin de eklemek gerekirse 2 li yaparız.
            if (labRepository.Find(m => m.Isim == username) != null)
            {
                string[] a = new string[1];
                a[0] = "Labaratuvar";
                return a;
            }
            else
            return new string[0];
        }

        public override string[] GetUsersInRole(string roleName)
        {
            Repository<Laboratuvarlar> labRepository = new Repository<Laboratuvarlar>(db);
            if (roleName == "Laboratuvar")
                return labRepository.GetAll().Select(m => m.Isim).ToArray();
            else
                return new string[0];
        }

        //Authorize sınıfı olustur ordan role ve kullanıcıya ulas 
        public override bool IsUserInRole(string username, string roleName)
        {
            Repository<Laboratuvarlar> labRepository = new Repository<Laboratuvarlar>(db);

            //user name lab da var mı diye bakıldı
            return labRepository.Find(m => m.Isim == username) != null;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
   
}
