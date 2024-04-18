using BusinessLayer.InterfaceBl;
using ModelLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ServicesBl
{
    public class UserServiceBl:IUserBl
    {
        private readonly IUser person;

        public UserServiceBl(IUser person)
        {
            this.person = person;
        }

        //Insertion
        public Task<int> Insertion(string firstname, string lastname, string emailid, string password)
        {
            return person.Insertion(firstname, lastname, emailid, password);
        }

        //forgot passsword
        public Task<String> ChangePasswordRequest(string Email)
        {
            return person.ChangePasswordRequest(Email);
        }
        public Task<string> ChangePassword(string otp, string password)
        {
            return person.ChangePassword(otp, password);
        }

        public Task<int> DeleteUserByEmail(string email)
        {
            return person.DeleteUserByEmail(email);
        }

        //get all users
        public Task<IEnumerable<User>> GetUsers()
        {
            return person.GetUsers();
        }

        //get users by email
        public Task<IEnumerable<User>> GetUsersByEmail(string email)
        {
            return person.GetUsersByEmail(email);
        }


        //login
        public Task<IEnumerable<User>> Login(string email, string password)
        {
            return person.Login(email, password);
        }
    }
}
