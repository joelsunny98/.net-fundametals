using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RetailStore.Controllers;

/// <summary>
/// Base Controller for all controllers
/// </summary>
[ApiController]
[Route("api")]
public class BaseController : ControllerBase
{
    private IMediator _mediator;

    /// <summary>
    /// Gets the Mediator instance for handling commands and queries.
    /// </summary>
    /// <remarks>
    /// The Mediator is a central messaging component that supports the implementation of the Mediator design pattern.
    /// It acts as a communication hub between different components and handles the execution of commands and queries.
    /// </remarks>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
