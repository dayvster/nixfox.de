using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using nixfox.App;

namespace nixfox.Controllers
{
	public class URLResponse
	{
		public string url { get; set; }
		public string status { get; set; }
		public string token { get; set; }
	}
	public class HomeController : Controller
	{
		

		[HttpGet, Route("/")]
		public IActionResult Index() {
			return View();
		}

		[HttpPost, Route("/")]
		public IActionResult PostURL([FromBody] string url) {
			try {
				Shortener shortURL = new Shortener(url);
				return Json(shortURL.Token);
			}catch(Exception ex) {
				if (ex.Message == "URL already exists") {
					Response.StatusCode = 400;
					return Json(new URLResponse() { url = url, status = "URL already exists", token = new LiteDB.LiteDatabase("Data/Urls.db").GetCollection<NixURL>().Find(u=>u.URL == url).FirstOrDefault().Token });
				}
				if(new Uri(url).Host == HttpContext.Request.Host.Host){
					Response.StatusCode = 405;
					return Json(new URLResponse(){url = url, status = "Not allowed to redirect to "+HttpContext.Request.Host.Host+" to prevent request chaining", token = null});
				}
			}
			return StatusCode(500);
		}

		[HttpGet, Route("/{token}")]
		public IActionResult NixRedirect([FromRoute] string token) {
			return Redirect(new LiteDB.LiteDatabase("Data/Urls.db").GetCollection<NixURL>().FindOne(u => u.Token == token).URL);
		}
	}
}
