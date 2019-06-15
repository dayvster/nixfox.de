using LiteDB;
using System;
using System.Linq;

namespace nixfox.App{
	public class NixURL{
		public Guid ID { get; set; }
		public string URL { get; set; }
		public string Token { get; set; }
		public int Clicked { get; set; } = 0;
		public DateTime Created { get; set; } = DateTime.Now;
	}

	public class Shortener{
		public string Token { get; set; } 
		private NixURL biturl;

		private Shortener GenerateToken() {
			string urlsafe = string.Empty;
			Enumerable.Range(48, 75).Where(i => i < 58 || i > 64 && i < 91 || i > 96).OrderBy(o => new Random().Next()).ToList().ForEach(i => urlsafe += Convert.ToChar(i));
			Token = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));
			return this;
		}
		public Shortener(string url) {
			var db = new LiteDatabase("Data/Urls.db");
			var urls = db.GetCollection<NixURL>();
			while (urls.Exists(u => u.Token == GenerateToken().Token)) ;
			biturl = new NixURL() { Token = Token, URL = url};
			if (urls.Exists(u => u.URL == url))
				throw new Exception("URL already exists");
			urls.Insert(biturl);
		}
	}
}
