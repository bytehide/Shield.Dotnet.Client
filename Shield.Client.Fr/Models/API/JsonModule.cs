using System.Collections.Generic;

namespace Shield.Client.Fr.Models.API
{
	public class JsonModule
	{
		public string Title { get; set; }
		public string Mdtoken { get; set; }
		public List<JsonNamespace> Children { get; set; }
	}
	public class JsonNamespace
	{
		public string Title { get; set; }
		public List<JsonType> Children { get; set; }
	}
	public class JsonType
	{
		public string Title { get; set; }
		public string Mdtoken { get; set; }
		public List<JsonSubType> Children { get; set; }
	}
	public class JsonSubType
	{
		public string Title { get; set; }
		public string Mdtoken { get; set; }
		public string Type { get; set; }
	}
}
