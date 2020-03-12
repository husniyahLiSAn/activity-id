using Data.Base;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Model
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }

        public User() { }

        public User(UserVM userVM)
        {
            //userVM.Id = Id;
            //Id = userVM.Id;
            Name = userVM.Name;
            UserName = userVM.Username;
            Password = userVM.Password;
            Email = userVM.Email;

            CreateDate = DateTime.Now;
            IsDelete = false;
        }
    }
}
