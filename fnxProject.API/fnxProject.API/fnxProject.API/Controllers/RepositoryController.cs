using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace fnxProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RepositoryController : ControllerBase
	{
		private readonly IHttpClientFactory httpClientFactory;

		public RepositoryController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchRepositories(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return BadRequest("Query parameter is required.");
			}

			var client = httpClientFactory.CreateClient();
			client.DefaultRequestHeaders.Add("User-Agent", "fnxProject.API");

			var response = await client.GetAsync($"https://api.github.com/search/repositories?q={query}");
			if (!response.IsSuccessStatusCode)
			{
				return StatusCode((int)response.StatusCode, "Error fetching data from GitHub.");
			}

			var content = await response.Content.ReadAsStringAsync();
			return Content(content, "application/json");
		}
	}
}
