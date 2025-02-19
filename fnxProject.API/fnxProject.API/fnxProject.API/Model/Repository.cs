﻿namespace fnxProject.API.Model
{
	public class Repository
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string HtmlUrl { get; set; }
		public string Description { get; set; }
		public string OwnerAvatarUrl { get; set; }
		public bool IsBookmarked { get; set; }
	}
}
