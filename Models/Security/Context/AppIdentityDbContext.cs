using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UkrStanko.Models.Security.Context
{
    public class AppIdentityDbContext: IdentityDbContext<AppUser, AppRole, string>
    {
        //Конструктор
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options): base(options) {
            Database.EnsureCreated();
        } // AppIdentityDbContext
    }
}