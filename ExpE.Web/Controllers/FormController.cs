using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ExpE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FormController(IRepository repo, IHostingEnvironment hostingEnvironment)
        {
            _repo = repo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("form")]
        public ActionResult<MyForm> PostForm([FromBody] MyForm form)
        {
            var isExists = _repo.ExistsFormName(form.Name);

            if (isExists)
                return BadRequest("name exists");

            form.Id = ObjectId.GenerateNewId().ToString();

            var isAdded = _repo.AddForm(form).Result;

            if (isAdded)
            {
                return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("form")]
        public async Task<ActionResult<bool>> UpdateForm([FromBody] MyForm form)
        {
            //DEPRECATED
            return await _repo.UpdateForm(form);
        }

        [HttpGet]
        [Route("form")]
        public async Task<IEnumerable<MyForm>> GetForms([FromQuery] string name)
        {
            if (name != null)
            {
                return await _repo.GetFormByName(name);
            }

            return await _repo.GetAllForms();
        }

        [HttpGet]
        [Route("form/{id}")]
        public async Task<ActionResult<MyForm>> GetForm(string id)
        {
            return await _repo.GetFormById(id);
        }

        [HttpDelete]
        [Route("form/{id}")]
        public async Task<ActionResult<bool>> DeleteForm(string id)
        {
            //check if form has any files
            string path = Path.Combine(_hostingEnvironment.WebRootPath, id);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            return await _repo.DeleteFormById(id);
        }

        [HttpPost]
        [Route("autocompletes")]
        public async Task<ActionResult<bool>> AddAutoCompletes([FromBody] AutoCompleteList autoCompleteList)
        {
            return await _repo.AddAutoCompletes(autoCompleteList);
        }

        [HttpGet]
        [Route("autocompletes/{formId}/{propertyKey}")]
        public async Task<ActionResult<AutoComplete>> GetAutoComplete(string formId, string propertyKey)
        {
            return await _repo.GetAutoComplete(formId, propertyKey);
        }

        [HttpPost]
        [Route("selectitems/{id}/{key}")]
        public async Task<ActionResult<bool>> AddSelectList(string id, string key, [FromBody] IEnumerable<DropDownOptions> dropDown)
        {
            return await _repo.AddSelectList(id, key, dropDown);
        }

        [HttpPut]
        [Route("selectitems/{formId}/{propertyKey}")]
        public async Task<ActionResult<bool>> UpdateSelectList(string formId, string propertyKey, [FromBody] IEnumerable<DropDownOptions> dropDown)
        {
            var result = await _repo.UpdateSelectList(formId, propertyKey, dropDown);

            return Ok();
        }

        [HttpPost]
        [Route("selectitem/{id}/{key}")]
        public async Task<ActionResult<bool>> AddSelectItem(string id, string key, [FromBody] DropDownOptions dropDown)
        {
            await _repo.AddSelectItem(id, key, dropDown);
            return Ok();
        }

        [HttpGet]
        [Route("selectitems/{id}/{key}")]
        public async Task<IEnumerable<DropDownOptions>> GetSelectList(string id, string key)
        {
            return await _repo.GetSelectList(id, key);
        }

        [HttpDelete]
        [Route("property/{formId}/{key}")]
        public async Task<ActionResult<bool>> DeleteProperty(string formId, string key)
        {
            return await _repo.DeleteProperty(formId, key);
        }

        [HttpPost]
        [Route("property/{formId}")]
        public async Task<ActionResult<Property>> AddProperty(string formId, [FromBody] Property property)
        {
            return await _repo.AddProperty(formId, property);
        }

        [HttpPut]
        [Route("property/{formId}")]
        public async Task<ActionResult<Property>> UpdateProperty(string formId, [FromBody] Property property)
        {
            return await _repo.UpdateProperty(formId, property);
        }

    }
}