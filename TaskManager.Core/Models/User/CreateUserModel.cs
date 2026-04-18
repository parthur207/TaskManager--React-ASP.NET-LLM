using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.User
{
    public class CreateUserModel
    {
        public CreateUserModel(string name, string email, string password, string passwordConfirmation)
        {
            Name = name;
            Email = email;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public string PasswordConfirmation { get; set; }
    }
}
