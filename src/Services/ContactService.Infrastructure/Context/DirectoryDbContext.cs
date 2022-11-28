using ContactService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Infrastructure.Context
{
    public class DirectoryDbContext:DbContext
    {
        public DirectoryDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
