using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RetailStore.Controllers;

[ApiController]
[Route("api")]
public class BaseController: Controller
{
    protected IMediator? Mediator => HttpContext.RequestServices.GetService<IMediator>();
}
