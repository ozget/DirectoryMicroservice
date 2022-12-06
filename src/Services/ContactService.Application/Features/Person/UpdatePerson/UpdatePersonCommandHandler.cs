
using ContactService.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.UpdateProduct
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommandRequest, UpdatePersonCommandResponse>
    {
        readonly IPersonRepository _personRepository;
        readonly ILogger<UpdatePersonCommandHandler> _logger;

        public UpdatePersonCommandHandler(IPersonRepository personRepository, ILogger<UpdatePersonCommandHandler> logger)
        {
            _personRepository = personRepository;
         
            _logger = logger;
        }

        public async Task<UpdatePersonCommandResponse> Handle(UpdatePersonCommandRequest request, CancellationToken cancellationToken)
        {
           var  person = await _personRepository.GetByIdAsync(request.Id);
            person.SurName = request.SurName;
            person.Name = request.Name;
            person.Firm = request.Firm;
            await _personRepository.SaveAsync();
            _logger.LogInformation("Person güncellendi...");
            return new();
        }
    }
}
