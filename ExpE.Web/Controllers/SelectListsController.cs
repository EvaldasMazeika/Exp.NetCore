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
    public class SelectListsController : ControllerBase
    {
        private readonly IRepository _repo;

        public SelectListsController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("{key}")]
        public async Task<ActionResult> AddSelectList(string formId, string key, [FromBody] IEnumerable<DropDownOptions> dropDown)
        {
            await _repo.AddSelectList(formId, key, dropDown);

            return NoContent();
        }

        [HttpPut]
        [Route("{key}")]
        public async Task<ActionResult> UpdateSelectList(string formId, string key, [FromBody] IEnumerable<DropDownOptions> dropDown)
        {
            await _repo.UpdateSelectList(formId, key, dropDown);

            return NoContent();
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<IEnumerable<DropDownOptions>> GetSelectList(string formId, string key)
        {
            return await _repo.GetSelectList(formId, key);
        }

        [HttpPost]
        [Route("{key}/item")]
        public async Task<ActionResult<bool>> AddSelectItem(string formId, string key, [FromBody] DropDownOptions dropDown)
        {
            await _repo.AddSelectItem(formId, key, dropDown);

            return NoContent();
        }

    }
}