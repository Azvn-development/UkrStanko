using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;
using UkrStanko.Models.Security;

namespace UkrStanko.ViewModels.Users
{
    public class IndexViewModel
    {
        public List<AppUser> Users { get; set; } // список пользователей

        // Конструктор
        public IndexViewModel() { } // IndexViewModel
        public IndexViewModel(List<AppUser> users)
        {
            Users = users;
        } // IndexViewModel
    }
}
