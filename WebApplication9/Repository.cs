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



        public void saveClientInfo(Client clientInfo, string interest1, string interest2, string interest3)
        {

            db.Clients.Add(clientInfo);


            ClientInterest clientInterest1 = new ClientInterest();
            clientInterest1.UserName = clientInfo.UserName;
          
            clientInterest1.interest = interest1;
            db.ClientInterests.Add(clientInterest1);

            ClientInterest clientInterest2 = new ClientInterest();
            clientInterest2.UserName = clientInfo.UserName;
           
            clientInterest2.interest = interest2;
            db.ClientInterests.Add(clientInterest2);

            ClientInterest clientInterest3 = new ClientInterest();
            clientInterest3.UserName = clientInfo.UserName;
          
            clientInterest3.interest = interest3;
            db.ClientInterests.Add(clientInterest3);

            db.SaveChanges();



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

        public void updatgeProfile(Client clientUpdate, string interest1, string interest2, string interest3)
        {

            //udpate client table
            Client client = db.Clients.Find(clientUpdate.UserName);
            client.email = clientUpdate.email;
            client.gender = clientUpdate.gender;
            client.UserName = clientUpdate.UserName;
            client.birthdate = clientUpdate.birthdate;
            client.country = clientUpdate.country;
            client.province = clientUpdate.province;
            client.city = clientUpdate.city;


            //delete old clientInterest info, insert new clientInterest info
            var queryInterests = from c_i in db.ClientInterests where c_i.UserName == clientUpdate.UserName select c_i;

            if (queryInterests != null)
            {
                foreach (var interest in queryInterests)
                {
                    db.ClientInterests.Remove(interest);

                }
            }


            ClientInterest clientInterest1 = new ClientInterest();
            clientInterest1.UserName = clientUpdate.UserName;
            clientInterest1.interest = interest1;


            ClientInterest clientInterest2 = new ClientInterest();
            clientInterest2.UserName = clientUpdate.UserName;
            clientInterest2.interest = interest2;

            ClientInterest clientInterest3 = new ClientInterest();
            clientInterest3.UserName = clientUpdate.UserName;
            clientInterest3.interest = interest3;




            db.SaveChanges();

        }


      

    }
}