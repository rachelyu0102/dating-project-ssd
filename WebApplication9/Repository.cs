using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9
{
    public class Repository
    {
        SSDDatingEntities5 db = new SSDDatingEntities5();

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
      
        public List<Client> saveAllClients()
        {
            List<Client> savedClients = new List<Client>();
            
            savedClients = db.Clients.ToList();
            db.SaveChanges();
            return savedClients;
        }

      

    }
}