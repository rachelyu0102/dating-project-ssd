using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    public class ClientRepo
    {

        public ClientRepo() { }

        public Boolean clientExist(SignUp newClient)
        {
            DatingDBEntities1 db = new DatingDBEntities1();
            Client client = db.Clients.Where(c => c.email == newClient.Email).FirstOrDefault();

            if (client == null)
            {
                return true;
            }
            return false;
        }

        public Boolean LoginAuth(Login clientLogin)
        {
            DatingDBEntities1 db = new DatingDBEntities1();
            Client client = db.Clients.Where(c => c.email == clientLogin.email && c.password == clientLogin.password).FirstOrDefault();
            if (client == null)
            {
                return false;
            }

            return true;
        }

        public void Create(SignUp newClient)
        {

            Client client = new Client();
            client.email = newClient.Email;
            client.password = newClient.Password;
            client.userName = newClient.UserName;
            DatingDBEntities1 db = new DatingDBEntities1();
            db.Clients.Add(client);
            db.SaveChanges();

        }

        public ClientDetails getClientDetail(string email)
        {
            DatingDBEntities1 db = new DatingDBEntities1();
            Client client = db.Clients.Where(c => c.email == email).FirstOrDefault();
            
            Location location = db.Locations.Where(d => d.locationID == client.locationID).FirstOrDefault();

            var query = from c in db.Clients
                        where (c.email == email)
                        from i in c.Interests
                        select new
                        {
                            interest = i.interest1
                        };

            string Email = client.email;
            string UserName = client.userName;
            string Password = client.password;
            string Gender = client.gender;
            int? Age = client.age;
            string City;
            string Province;
            string Country;

            if (location == null)
            {
                City = "";
                Province = "";
                Country = "";
            }
            else
            {
                City = location.city;
                Province = location.province;
                Country = location.country;
            }

            string[] Interest = new string[3];
            int num = 0;

            foreach (var item in query)
            {
                Interest[num] = item.interest;
                num = num + 1;
            }

            ClientDetails clientDetials = new ClientDetails(Email, UserName, Password, Age, Gender, City, Province, Country, Interest[0], Interest[1], Interest[2]);
            return clientDetials;
        }


        public ClientProfile getClientProfile(string email)
        {
            ClientDetails clientDetail = new ClientDetails();
            clientDetail = getClientDetail(email);

            string Email = clientDetail.Email;
            string UserName = clientDetail.UserName;
            string Gender = clientDetail.Gender;
            int? Age = clientDetail.Age;
            string City = clientDetail.City; ;
            string Province = clientDetail.Province;
            string Country = clientDetail.Country;
            string[] Interest = new string[3];
            Interest[0] = clientDetail.Interest1;
            Interest[1] = clientDetail.Interest2;
            Interest[2] = clientDetail.Interest3;
            ClientProfile clientProfile = new ClientProfile(Email, UserName, Age, Gender, City, Province, Country, Interest[0], Interest[1],
            Interest[2]);
            return clientProfile;

        }


        public List<ClientLocation> GetClientsByLocation(string email)
        {
            DatingDBEntities1 db = new DatingDBEntities1();
            List<ClientLocation> LocationList = new List<ClientLocation>();
            Client client = db.Clients.Where(c => c.email == email).FirstOrDefault();
            string Email;
            string UserName;

            var query = from b in db.Clients
                        where (b.locationID == client.locationID && b.email != email)
                        select new
                        {
                            UserName = b.userName,
                            Email = b.email
                        };

            foreach (var item in query)
            {
                Email = item.Email;
                UserName = item.UserName;
                LocationList.Add(new ClientLocation(Email, UserName));
            }
            return LocationList;
        }
    }
}