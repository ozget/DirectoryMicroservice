using ContactService.Application.Repositories;
using ContactService.Infrastructure.Context;
using ContactService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection service)
        {
            

            service.AddDbContext<DirectoryDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            service.AddScoped<IContactRepository, ContactRepository>();
            service.AddScoped<IPersonRepository, PersonRepository>();

        }
    }
}
