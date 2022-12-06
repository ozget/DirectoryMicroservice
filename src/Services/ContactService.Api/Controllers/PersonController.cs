using ContactService.Application.Features.Person.CreateProduct;
using ContactService.Application.Features.Person.GetAllProduct;
using ContactService.Application.Features.Person.GetByIdProduct;
using ContactService.Application.Features.Person.RemoveProduct;
using ContactService.Application.Features.Person.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace ContactService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        
        readonly IMediator _mediator;
        readonly ILogger<PersonController> _logger;

        public PersonController(IMediator mediator, ILogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllPersonQueryRequest getAllProductQueryRequest)
        {
            GetAllPersonQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }
        


        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdPersonQueryRequest getByIdPersonQueryRequest)
        {
            GetByIdPersonQueryResponse response = await _mediator.Send(getByIdPersonQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePersonCommandRequest createPersonCommandRequest)
        {
            CreatePersonCommanResponse createPersonCommanResponse = await _mediator.Send(createPersonCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdatePersonCommandRequest updatePersonCommandRequest)
        {
            UpdatePersonCommandResponse response = await _mediator.Send(updatePersonCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(RemovePersonCommandRequest removePersonCommandRequest)
        {
            RemovePersonCommandResponse response = await _mediator.Send(removePersonCommandRequest);
            return Ok();
        }
    }
}
