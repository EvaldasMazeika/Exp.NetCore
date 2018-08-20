using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using HeyRed.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MongoDB.Bson;

namespace ExpE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RecordsController(IRepository repo, IHostingEnvironment hostingEnvironment)
        {
            _repo = repo;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpGet]
        [Route("download/{formId}/{propertyName}/{title}")]
        public async Task<IActionResult> DownloadFile(string formId, string propertyName, string title)
        {
              string root = _hostingEnvironment.WebRootPath;
            //  root = String.Concat(root, $"/{formId}/{propertyName}");
            // IFileProvider provider = new PhysicalFileProvider(root);
            // IFileInfo fileInfo = provider.GetFileInfo(title);
            // var readStream = fileInfo.CreateReadStream();
            var path = Path.Combine(root,formId,propertyName, title);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, MimeGuesser.GuessMimeType(path), title);
        }

        [HttpPost]
        [Route("test")]
        public ActionResult<int> TestMe()
        {
            var file = Request.Form.Files;

            return Ok();
        }

        [HttpPost, DisableRequestSizeLimit]
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