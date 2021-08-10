using System;
using System.Collections.Generic;
using System.Text;
using CarbonConfigServer.Models;
using CarbonConfigServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarbonConfigServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly ConfigService _ConfigService;

        public ConfigController(ConfigService ConfigService)
        {
            _ConfigService = ConfigService;
        }
        
        
        [HttpGet("{id:length(24)}/AppInfo", Name = "AppInfo")]
        public ActionResult<AppInfo> GetAppInfo(string id)
        {
            var config = _ConfigService.Get(id);

            if (config == null)
            {
                return NotFound();
            }

            return new AppInfo(){AppName = config.AppName};
        }
        
        [HttpPut("{id:length(24)}/AppInfo/{name}", Name = "SetAppInfo")]
        public ActionResult<AppInfo> GetAppInfo(string id, string name)
        {
            var config = _ConfigService.Get(id);

            if (config == null)
            {
                return NotFound();
            }

            config.AppName = name;
            _ConfigService.Update(id, config);

            return new AppInfo(){AppName = config.AppName};
        }

        [HttpPost("CreateApp", Name = "CreateApp")]
        public ActionResult<AppConfig> CreateApp(string name)
        {
            var a = new AppConfig()
            {
                AppName = name,
                Config = new Dictionary<string, object>(),
                LastSeen = DateTime.UtcNow,
                AppToken = RandomString(128)
            };
            return _ConfigService.Create(a);
        }

        [HttpGet("{id:length(24)}/Config/{AuthToken}", Name = "GetConfig")]
        public ActionResult<Dictionary<string, object>> GetConfig(string id, string AuthToken)
        {
            var config = _ConfigService.Get(id);
            if (config == null)
            {
                return NotFound();
            }

            if (AuthToken != config.AppToken)
            {
                return Unauthorized();
            }
            return config.Config;
        }
        [HttpPut("{id:length(24)}/Config", Name = "PutConfig")]
        public ActionResult<Dictionary<string, object>> PutConfig(string id, string AuthToken, Dictionary<string, object> newConfig)
        {
            var config = _ConfigService.Get(id);
            if (config == null)
            {
                return NotFound();
            }

            if (config.AppToken != AuthToken)
            {
                return Unauthorized();
            }

            if (config.Config is null)
            {
                config.Config = new Dictionary<string, object>();
            }

            config.Config = newConfig;
            
            _ConfigService.Update(id, config);
            return config.Config;
        }


        [HttpPatch("{id:length(24)}/Config", Name = "PatchConfig")]
        public ActionResult<Dictionary<string, object>> PatchConfig(string id, string AuthToken, Dictionary<string, object> ConfigPatch)
        {
            var config = _ConfigService.Get(id);
            if (config == null)
            {
                return NotFound();
            }

            if (config.AppToken != AuthToken)
            {
                return Unauthorized();
            }

            if (config.Config is null)
            {
                config.Config = new Dictionary<string, object>();
            }

            foreach (var (param, value) in ConfigPatch)
            {
                config.Config[param] = value;
            }
            
            _ConfigService.Update(id, config);
            return config.Config;
        }


        [HttpDelete("{id:length(24)}/{AuthToken}")]
        public IActionResult Delete(string id, string AuthToken)
        {
            var config = _ConfigService.Get(id);

            if (config == null)
            {
                return NotFound();
            }
            if (config.AppToken != AuthToken)
            {
                return Unauthorized();
            }

            _ConfigService.Remove(config.Id);

            return NoContent();
        }
        
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch ;
            for(int i=0; i<size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))) ;
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}