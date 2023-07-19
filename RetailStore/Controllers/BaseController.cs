using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RetailStore.Controllers;

/// <summary>
/// Base Controller for all controllers
/// </summary>
[ApiController]
[Route("api")]
public class BaseController: ControllerBase
{
    private IMediator _mediator;

    /// <summary>
    /// Mediator Instance
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

}
