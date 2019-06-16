using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nixfox.App{
	public class Config
	{
		public string BASE_URL;
	}
	public class NixConf{
		public Config Config;
		public NixConf() {
			Config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText("App/Config.json"));
		}
	}
}
