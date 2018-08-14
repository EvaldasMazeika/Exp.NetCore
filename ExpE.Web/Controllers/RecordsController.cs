using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ExpE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IRepository _repo;

        public RecordsController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("records/{id}")]
        public async Task<ActionResult<IEnumerable<Record>>> GetAllRecords(string id)
        {
            return await _repo.GetRecords(id);
        }

        [HttpGet]
        [Route("record/{id}")]
        public async Task<ActionResult<Record>> GetRecord(string id)
        {
            return await _repo.GetRecord(id);
        }

        [HttpPost]
        [Route("record")]
        public async Task<ActionResult<Record>> InsertRecord([FromBody] Record record)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            record.Id = ObjectId.GenerateNewId().ToString();

            var isAdded = await _repo.AddRecord(record);
            if (isAdded)
            {
                return CreatedAtAction(nameof(GetRecord), new { id = record.Id }, record);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("record")]
        public async Task<ActionResult<Record>> UpdateRecord([FromBody] Record record)
        {
            var isUpdated = await _repo.UpdateRecord(record);

            if (isUpdated)
            {
                return CreatedAtAction(nameof(GetRecord), new { id = record.Id }, record);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("record/{id}")]
        public async Task<ActionResult<bool>> DeleteRecord(string id)
        {
            return await _repo.DeleteRecord(id);
        }

        [HttpPost]
        [Route("autocomplete")]
        public async Task<IActionResult> AddWordsToAutoDictionaries([FromBody] AutoCompleteWords words)
        {
            await _repo.AddWordsToAutos(words);
            return Ok();
        }

    }
}