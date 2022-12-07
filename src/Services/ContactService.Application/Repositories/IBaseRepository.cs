﻿using ContactService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Application.Repositories
{
    public interface IBaseRepository<T> where T:BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
