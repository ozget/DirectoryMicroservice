using ContactService.Application.Repositories;
using ContactService.Domain;
using ContactService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Infrastructure.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(DirectoryDbContext context) : base(context)
        {
        }
    }
}
