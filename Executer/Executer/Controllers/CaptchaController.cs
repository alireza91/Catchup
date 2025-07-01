using Catchup.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Executer.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CaptchaController : ControllerBase
{
	private readonly ICatchup _catchup;
	public CaptchaController(ICatchup catchup)
	{
		_catchup = catchup;
	}

	[HttpGet("GetCaptcha")]

	public dynamic Get()
	{
		return _catchup.GetAnImageCaptcha();
	}
}
