
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.GetByIdProduct
{
    public class GetByIdPersonQueryRequest : IRequest<GetByIdPersonQueryResponse>
    {
        public string Id { get; set; }
    }
}
