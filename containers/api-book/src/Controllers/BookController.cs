using Book.Domain.CommandSide.Commands;
using Book.Domain.QuerySide.Queries;
using Book.Models.Dto;
using Framework.Web.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Book.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ApiController
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OkResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Save(SaveBookCommand book)
        {
            await _mediator.Send(book);
            return Ok();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] GetBookQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] ListBookFilteringQuery query)
        {
            var result = await _mediator.Send(query);
            return PagedOk(result);
        }
    }
}