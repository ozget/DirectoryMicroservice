using ContactService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Application.Repositories
{
    public interface IContactRepository:IRepository<Contact>
    {
    }
}
