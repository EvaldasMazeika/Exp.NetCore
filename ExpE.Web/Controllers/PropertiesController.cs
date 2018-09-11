using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpE.Web.Controllers
{
    [Route("api/forms/{formId}/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IRepository _repo;

        public PropertiesController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpDelete]
        [Route("{key}")]
        public async Task<ActionResult> DeleteProperty(string formId, string key)
        {
            await _repo.DeleteProperty(formId, key);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Property>> AddProperty(string formId, [FromBody] Property property)
        {
            var result = await _repo.AddProperty(formId, property);

            return CreatedAtAction(nameof(GetProperty), new { formId, key = result.Key }, result);
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<ActionResult<Property>> GetProperty(string formId, string key)
        {
            var property = await _repo.GetProperty(formId, key);
            if (property == null)
                return NotFound();

            return property;
        }

        [HttpPut]
        public async Task<ActionResult<Property>> UpdateProperty(string formId, [FromBody] Property property)
        {
            return await _repo.UpdateProperty(formId, property);
        }
    }
}