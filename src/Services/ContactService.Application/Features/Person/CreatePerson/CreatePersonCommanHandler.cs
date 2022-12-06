
using ContactService.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.CreateProduct
{
    public class CreatePersonCommanHandler : IRequestHandler<CreatePersonCommandRequest, CreatePersonCommanResponse>
    {
        readonly IPersonRepository _personRepository;
        public CreatePersonCommanHandler(IPersonRepository repository)
        {
            _personRepository = repository;
        }

        public async Task<CreatePersonCommanResponse> Handle(CreatePersonCommandRequest request, CancellationToken cancellationToken)
        {
            await _personRepository.AddAsync(new()
            {
                Name = request.Name,
                Firm = request.Firm,
                SurName = request.SurName
            });
            await _personRepository.SaveAsync();
            return new();

        }
    }
}
