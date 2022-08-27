using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using truYumWebApplication.Models;
using Microsoft.EntityFrameworkCore;


namespace truYumWebApplication
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<MenuItem> MenuItems { get;  set; }
    }
}
