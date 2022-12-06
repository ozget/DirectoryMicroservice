using ContactService.Application.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContactService.Application.Features.Person.GetByIdProduct
{
    internal class GetByIdPersonQueryHandler : IRequestHandler<GetByIdPersonQueryRequest, GetByIdPersonQueryResponse>
    {

        readonly IPersonRepository _personRepository;
        public GetByIdPersonQueryHandler(IPersonRepository productReadRepository)
        {
            _personRepository = productReadRepository;
        }

        public async Task<GetByIdPersonQueryResponse> Handle(GetByIdPersonQueryRequest request, CancellationToken cancellationToken)
        {
           var product = await _personRepository.GetByIdAsync(request.Id, false);
            return new()
            {
                Name = product.Name,
                SurName = product.SurName,
                Firm = product.Firm
            };
        }
    }
}
