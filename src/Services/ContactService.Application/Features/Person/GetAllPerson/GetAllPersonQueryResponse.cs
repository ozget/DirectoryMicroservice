using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.GetAllProduct
{
    public class GetAllPersonQueryResponse
    {
        public int TotalCount { get; set; }
        public object Persons { get; set; }
    }
}
