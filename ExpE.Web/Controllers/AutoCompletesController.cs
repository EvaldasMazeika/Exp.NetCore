using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpE.Web.Controllers
{
    [Route("api/forms/{formId}/[controller]")]
    [ApiController]
    public class AutoCompletesController : ControllerBase
    {
        private readonly IRepository _repo;

        public AutoCompletesController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult> AddAutoCompletes(string formId, [FromBody] AutoCompleteList autoCompleteList)
        {
            await _repo.AddAutoCompletes(autoCompleteList);

            return NoContent();
        }

        [HttpGet]
        [Route("{propertyKey}")]
        public async Task<ActionResult<AutoComplete>> GetAutoComplete(string formId, string propertyKey)
        {
            return await _repo.GetAutoComplete(formId, propertyKey);
        }

        [HttpPost]
        [Route("words")]
        public async Task<IActionResult> AddWordsToAutoDictionaries([FromBody] AutoCompleteWords words)
        {
            await _repo.AddWordsToAutos(words);
            return NoContent();
        }
    }
}