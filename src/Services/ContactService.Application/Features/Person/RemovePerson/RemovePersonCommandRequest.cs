
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.RemoveProduct
{
    public class RemovePersonCommandRequest : IRequest<RemovePersonCommandResponse>
    {
        public string Id { get; set; }
        public string ContactId { get; set; }
    }
}
