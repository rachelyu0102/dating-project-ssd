using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9
{
    public class Repository
    {
        SSDDatingEntities3 db = new SSDDatingEntities3();

        public List<AspNetUser> getAllUsers() //Get all users
        {
            List<AspNetUser> users = new List<AspNetUser>();
            users = db.AspNetUsers.ToList();

            return users;
        }
        public List<Client> getAllClients() 
        {
            List<Client> clients = new List<Client>();
            clients = db.Clients.ToList();

            return clients; //have all client info
        }
        public List<AspNetRole> getUserRole()
        {
            List<AspNetRole> roles = new List<AspNetRole>();
            roles = db.AspNetRoles.ToList();

            return roles;
        }
      

      

    }
}