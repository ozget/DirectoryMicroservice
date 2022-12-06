
using ContactService.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.CreateProduct
{
    public class CreatePersonCommandRequest : IRequest<CreatePersonCommanResponse>
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Firm { get; set; }
    }
}
