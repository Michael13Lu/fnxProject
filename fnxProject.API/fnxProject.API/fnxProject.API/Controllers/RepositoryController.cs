using fnxProject.API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

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
		public async Task<IActionResult> SearchRepositories(
			[FromQuery] string query,
			[FromQuery] string? language = null,
			[FromQuery] string? sort = null,
			[FromQuery] string? order = null)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return BadRequest("Query parameter is required.");
			}

			try
			{
				var client = httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Add("User-Agent", "fnxProject.API");

				// Формируем строку запроса
				var queryString = $"https://api.github.com/search/repositories?q={query}";

				if (!string.IsNullOrWhiteSpace(language))
				{
					queryString += $"+language:{language}";
				}

				if (!string.IsNullOrWhiteSpace(sort))
				{
					queryString += $"&sort={sort}";
				}

				if (!string.IsNullOrWhiteSpace(order))
				{
					queryString += $"&order={order}";
				}

				var response = await client.GetAsync(queryString);

				if (!response.IsSuccessStatusCode)
				{
					return StatusCode((int)response.StatusCode, "Error fetching data from GitHub.");
				}

				var content = await response.Content.ReadAsStringAsync();
				var jsonResponse = JsonDocument.Parse(content);

				var items = jsonResponse.RootElement.GetProperty("items");
				var results = new List<Repository>();

				foreach (var item in items.EnumerateArray())
				{
					results.Add(new Repository
					{
						Id = Guid.NewGuid(),
						Name = item.GetProperty("name").GetString(),
						OwnerAvatarUrl = item.GetProperty("owner").GetProperty("avatar_url").GetString(),
						IsBookmarked = false
					});
				}

				return Ok(results);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in SearchRepositories: {ex.Message}");
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}


		[HttpPost("bookmark")]
		public IActionResult AddBookmark([FromBody] object repository)
		{
			if (repository == null)
			{
				return BadRequest("Repository data is required.");
			}

			// Пример сохранения в сессии.
			HttpContext.Session.SetString("bookmarkedRepository", repository.ToString());

			return Ok("Repository bookmarked successfully.");
		}

		[HttpGet("bookmarks")]
		public IActionResult GetBookmarks()
		{
			var bookmarks = HttpContext.Session.GetString("bookmarkedRepository");

			if (string.IsNullOrEmpty(bookmarks))
			{
				return Ok("No bookmarks found.");
			}

			return Content(bookmarks, "application/json");
		}
	}
}
