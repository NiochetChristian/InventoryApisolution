using InventoryClientAPI.Models;
using InventoryClientAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InventoryClientAPI.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class TokenController : Controller {
		AppSettings appSettings = new AppSettings();

		public TokenController(IOptions<AppSettings> _appSettings) {
			appSettings = _appSettings.Value;
		}

		[HttpGet]
		public IActionResult GenerateToken() {
			var response = Tools.GenerateTokenBearer(appSettings);
			return Ok(response);
		}
	}
}
