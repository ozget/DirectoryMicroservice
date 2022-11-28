using ContactService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DirectoryDbContext>
    {
        public DirectoryDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<DirectoryDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
