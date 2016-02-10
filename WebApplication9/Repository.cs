using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication9.ViewModels;
using Microsoft.SqlServer;

namespace WebApplication9
{
    public class Repository
    {
        SSDDatingEntities11 db = new SSDDatingEntities11();

        public const string NAME = "Name";
        public const string AGE = "Age";
        public const string AGE_DESC = "Age_Desc";
        public const string ROLEID = "RoleId";


        //search a specifi cuser, filter by interest, gender
        IEnumerable<ClientDetailInfo> FilterClients(IEnumerable<ClientDetailInfo> clients, string searchString, string interestString, string genderString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(
                           c => c.client.UserName.ToUpper().Contains(searchString.ToUpper()));
                return clients;

            }
                          
             //filter by interest
            if (!String.IsNullOrEmpty(interestString)){

                clients = getClientsByInterest(interestString);

            }
            
            //filter by gender
            if (!String.IsNullOrEmpty(genderString)){

                clients = clients.Where(c => c.client.gender == genderString);

            }

            return clients;
        }

        //get clients DetialInfo by interest
        IEnumerable<ClientDetailInfo> getClientsByInterest(string interestString)
        {
            var query = from c_i in db.ClientInterests
                        where c_i.interest == interestString
                        select c_i.UserName;

           List <Client> clientList = new List<Client>();
            foreach(var username in query)
            {
                Client client = db.Clients.Find(username);
                clientList.Add(client);
            }
       
            List<ClientDetailInfo> AllClientsDetailsInfo = new List<ClientDetailInfo>();
        
            List<string> interests = new List<string>();
            foreach (Client client in clientList)
            {
                interests = (from c_i in db.ClientInterests where c_i.UserName == client.UserName select c_i.Interest1.interest1).ToList();

                AllClientsDetailsInfo.Add(new ClientDetailInfo(client, interests));
            }

           return AllClientsDetailsInfo;
        }

        //Sort users
        IEnumerable<ClientDetailInfo> SortClient(IEnumerable<ClientDetailInfo> clients, string sortOrder)
        {
            switch (sortOrder)
            {                                
                case NAME:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenBy(c => c.client.UserName);
                    break;
                case AGE:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenBy(c => c.client.birthdate);
                    break;
                case AGE_DESC:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenByDescending(c => c.client.birthdate);
                    break;
                default:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenBy(c => c.client.UserName);
                    break;
            }
            return clients;
        }


         //get all clients detail info, including their interests
        public IEnumerable<ClientDetailInfo> getAllClientsDetailsInfo(string searchString, string interestringString, string genderString, string sortOrder)
        {
            List<ClientDetailInfo> AllClientsDetailsInfoList = new List<ClientDetailInfo>();
           
            IEnumerable<Client> clients = db.Clients.ToList();

            List<string> interests = new List<string>();
            foreach(Client client in clients)
            {
                interests = (from c_i in db.ClientInterests where c_i.UserName == client.UserName select c_i.Interest1.interest1).ToList();

                AllClientsDetailsInfoList.Add(new ClientDetailInfo(client, interests));
            }

            IEnumerable<ClientDetailInfo> AllClientDetailsInfo = AllClientsDetailsInfoList.ToList();
            AllClientDetailsInfo = FilterClients(AllClientDetailsInfo, searchString, interestringString, genderString);
            AllClientDetailsInfo = SortClient(AllClientDetailsInfo, sortOrder);

            return AllClientDetailsInfo;
        }

        //get one client Detial Info, including interests
        public ClientDetailInfo getOneUserDetailInfo(string id)
        {
            Client client = db.Clients.Find(id);
            List<string> interests = new List<string>();
            interests = (from c_i in db.ClientInterests where c_i.UserName == client.UserName
                         select c_i.Interest1.interest1).ToList();

            ClientDetailInfo ClientDetailinfo = new ClientDetailInfo(client, interests);
            return ClientDetailinfo;
        }
        public ClientDetailInfo getUserDetailInfo(string id)
        {
            Client client = (from i in db.Clients
                            where i.UserId == id
                            select i).FirstOrDefault();
            List < string > interests = new List<string>();
            interests = (from i in db.Interests
                         select i.interest1).ToList() ;

            ClientDetailInfo ClientDetailinfo = new ClientDetailInfo(client, interests);
            return ClientDetailinfo;
        }







        public List<AspNetRole> getUserRole()
        {
            List<AspNetRole> roles = new List<AspNetRole>();
            roles = db.AspNetRoles.ToList();
            return roles;
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
       
        public void saveAvailableDate(String userName, DateTime availableDate, DateTime timepicker1)
        {
            string sqlFormattedAvailableDate = availableDate.ToString();
            string sqlFormattedTimePicker = timepicker1.ToString();

            Client client = db.Clients.Find(userName);


            client.availableDate = availableDate;
        //   client.timeStart = (TimeSpan) timepicker1;
            //client.availableDate.Value.Add(availableDate);
            //client.timeStart.Value.Add((DateTime)timepicker1);
            db.SaveChanges();
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

        //get a user's interests
       public List<ClientInterest> getUserInterests(string userName)
        {
            List<ClientInterest> clientInterests = new List<ClientInterest>();
            clientInterests = (from c_i in db.ClientInterests where c_i.UserName == userName select c_i).ToList();
            return clientInterests;           
        }     


       //get all interests
       public IEnumerable<Interest> getAllInterests()
        {
            IEnumerable<Interest> interests = db.Interests.ToList();
            return interests;
        } 



       

    }
}