﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Domain
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
