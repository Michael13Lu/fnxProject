﻿using fnxProject.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace fnxProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize] // דורש JWT עבור גישה לכל המתודות
	public class RepositoryController : ControllerBase
	{
		private readonly IHttpClientFactory httpClientFactory;

		public RepositoryController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}

		// חיפוש
		[HttpGet("search")]
		public async Task<IActionResult> SearchRepositories(
			[FromQuery] string query,
			[FromQuery] string? language = null,
			[FromQuery] string? sort = null,
			[FromQuery] string? order = null)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return BadRequest("יש צורך בפרמטר של שאילתה.");
			}

			try
			{
				var client = httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Add("User-Agent", "fnxProject.API");

				// יצירת מחרוזת השאילתה
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
					return StatusCode((int)response.StatusCode, "שגיאה בקבלת נתונים מ-GitHub.");
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
						HtmlUrl = item.GetProperty("html_url").GetString(),
						Description = item.GetProperty("description").GetString(),
						OwnerAvatarUrl = item.GetProperty("owner").GetProperty("avatar_url").GetString(),
						IsBookmarked = false
					});
				}

				return Ok(results);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"שגיאה ב-SearchRepositories: {ex.Message}");
				return StatusCode(500, "אירעה שגיאה בעת עיבוד הבקשה.");
			}
		}

		[HttpPost("bookmark")]
		public IActionResult AddBookmark([FromBody] Repository repository)
		{
			Console.WriteLine("Session ID: " + HttpContext.Session.Id);
			Console.WriteLine("Request cookies: " + string.Join(", ", HttpContext.Request.Cookies));

			if (repository == null)
			{
				return BadRequest("Invalid repository data.");
			}

			// Копируем текущие закладки из сессии
			var bookmarksJson = HttpContext.Session.GetString("bookmarkedRepositories");
			var bookmarks = string.IsNullOrEmpty(bookmarksJson)
				? new List<Repository>()
				: JsonSerializer.Deserialize<List<Repository>>(bookmarksJson);

			if (bookmarks.Any(b => b.Id == repository.Id))
			{
				return BadRequest("Repository already bookmarked.");
			}

			bookmarks.Add(repository);

			// Сохраняем обновлённый список в сессии
			HttpContext.Session.SetString("bookmarkedRepositories", JsonSerializer.Serialize(bookmarks));

			Console.WriteLine("Current session data: " + HttpContext.Session.GetString("bookmarkedRepositories"));

			return Ok(bookmarks);
		}

		[HttpGet("bookmarks")]
		public IActionResult GetBookmarks()
		{
			Console.WriteLine("Session ID: " + HttpContext.Session.Id);
			Console.WriteLine("Request cookies: " + string.Join(", ", HttpContext.Request.Cookies));

			var bookmarksJson = HttpContext.Session.GetString("bookmarkedRepositories");
			Console.WriteLine("Fetching session data: " + bookmarksJson);

			if (string.IsNullOrEmpty(bookmarksJson))
			{
				return Ok(new List<Repository>());
			}

			var bookmarks = JsonSerializer.Deserialize<List<Repository>>(bookmarksJson);
			return Ok(bookmarks);
		}

		// מחיקת מועדף
		[HttpDelete("bookmark/{id}")]
		public IActionResult RemoveBookmark(Guid id)
		{
			// קבלת המועדפים הנוכחיים מהסשן
			var bookmarksJson = HttpContext.Session.GetString("bookmarkedRepositories");
			if (string.IsNullOrEmpty(bookmarksJson))
			{
				return NotFound("לא נמצאו מועדפים.");
			}

			var bookmarks = JsonSerializer.Deserialize<List<Repository>>(bookmarksJson);

			// מחיקת המועדף לפי ID
			var updatedBookmarks = bookmarks.Where(b => b.Id != id).ToList();

			// שמירת הרשימה המעודכנת בסשן
			HttpContext.Session.SetString("bookmarkedRepositories", JsonSerializer.Serialize(updatedBookmarks));

			return Ok(updatedBookmarks);
		}
	}
}
