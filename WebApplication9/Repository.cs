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


        //get one client

        public Client getUserProfile(string id)
        {
            Client client = db.Clients.Find(id);
            return client;
        }

        //Get Asp USER
        public AspNetUser GetUser(string username)
        {
            AspNetUser client = (from i in db.AspNetUsers
                            where i.UserName == username
                            select i).FirstOrDefault();
            return client;
        }

        public void updatgeProfile(Client clientUpdate, List<ClientInterest> clientInterestUpdate)
        {

            //udpate client table
            Client client = db.Clients.Find(clientUpdate.ID);
            client.locationID = clientUpdate.locationID;
            client.email = clientUpdate.email;
            client.gender = clientUpdate.gender;
            client.userName = clientUpdate.userName;
            client.birthdate = clientUpdate.birthdate;


            //delete old clientInterest info, insert new clientInterest info
            var queryInterests = from c_i in db.ClientInterests where c_i.ID == clientUpdate.ID select c_i;

            if (queryInterests != null)
            {
                foreach (var interest in queryInterests)
                {
                    db.ClientInterests.Remove(interest);

                }
            }

            foreach (var interest in clientInterestUpdate)
            {

                db.ClientInterests.Add(interest);

            }

            db.SaveChanges();

        }




    }
}