
using ContactService.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.GetAllProduct
{
    public class GetAllPersonQueryHandler : IRequestHandler<GetAllPersonQueryRequest, GetAllPersonQueryResponse> //request tipinde veri alma, response geri döndürme
    {
        readonly IPersonRepository _personRepository;
        public GetAllPersonQueryHandler(IPersonRepository productReadRepository)
        {
            _personRepository = productReadRepository;
        }
        public async Task<GetAllPersonQueryResponse> Handle(GetAllPersonQueryRequest request, CancellationToken cancellationToken)
        {
            var totalCount = _personRepository.GetAll(false).Count();
            var persons = _personRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Firm,
                p.CreatedDate,
                p.Contacts
            }).Skip(request.Page * request.Size).Take(request.Size);
            return new() { TotalCount = totalCount, Persons = persons };
        }
    }
}
