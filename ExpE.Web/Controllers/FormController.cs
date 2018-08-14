using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
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

        public FormController(IRepository repo)
        {
            _repo = repo;
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

    }
}