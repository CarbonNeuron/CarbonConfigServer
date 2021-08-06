using System.Collections.Generic;
using CarbonConfigServer.Models;
using CarbonConfigServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarbonConfigServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly ConfigService _ConfigService;

        public ConfigController(ConfigService ConfigService)
        {
            _ConfigService = ConfigService;
        }

        [HttpGet]
        public ActionResult<List<AppConfig>> Get() =>
            _ConfigService.Get();

        [HttpGet("{id:length(24)}", Name = "GetConfig")]
        public ActionResult<AppConfig> Get(string id)
        {
            var config = _ConfigService.Get(id);

            if (config == null)
            {
                return NotFound();
            }

            return config;
        }

        [HttpPost]
        public ActionResult<AppConfig> Create(AppConfig config)
        {
            _ConfigService.Create(config);

            return CreatedAtRoute("GetConfig", new { id = config.Id.ToString() }, config);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, AppConfig ConfigIn)
        {
            var config = _ConfigService.Get(id);

            if (config == null)
            {
                return NotFound();
            }

            _ConfigService.Update(id, ConfigIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var config = _ConfigService.Get(id);

            if (config == null)
            {
                return NotFound();
            }

            _ConfigService.Remove(config.Id);

            return NoContent();
        }
    }
}