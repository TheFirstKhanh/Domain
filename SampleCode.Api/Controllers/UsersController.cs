using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SampleCode.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("summary")]
    public async  Task<ActionResult<GetUserSummaryQueryResponse>> GetData()           
    {
        GetUserSummaryQueryResponse? getUserSummaryQueryResponse = null;

        try 
        {
            getUserSummaryQueryResponse = await _mediator.Send(new GetUserSummaryQuery());

            if (getUserSummaryQueryResponse == null) 
            {
                return StatusCode(404);
            }

        }
        catch (Exception ex) 
        {
            return StatusCode(500, ex.Message);
        }

        return Ok(getUserSummaryQueryResponse);
    }
}