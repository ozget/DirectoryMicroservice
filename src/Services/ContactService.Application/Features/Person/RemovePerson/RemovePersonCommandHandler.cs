
using ContactService.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.RemoveProduct
{
    public class RemoveProductCommandHandler : IRequestHandler<RemovePersonCommandRequest, RemovePersonCommandResponse>
    {
        readonly IPersonRepository _personRepository;

        public RemoveProductCommandHandler(IPersonRepository productWriteRepository)
        {
            _personRepository = productWriteRepository;
        }

        public async Task<RemovePersonCommandResponse> Handle(RemovePersonCommandRequest request, CancellationToken cancellationToken)
        {
            await _personRepository.RemoveAsync(request.Id);
            await _personRepository.SaveAsync();
            return new();
        }
    }
}
