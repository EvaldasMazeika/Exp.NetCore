using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
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

        [HttpPost]
        [Route("upload/{formId}/{recordId}/{propertyName}")]
        public async Task<IActionResult> UploadFile(string formId, string recordId, string propertyName)
        {
            try
            {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, formId, recordId, propertyName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    Directory.Delete(path, true);
                    Directory.CreateDirectory(path);
                }
                var files = Request.Form.Files;

                foreach (var item in files)
                {
                    string fileName = item.FileName;
                    string fullPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("download/{formId}/{recordId}/{propertyName}/{title}")]
        public async Task<IActionResult> DownloadFile(string formId,string recordId, string propertyName, string title)
        {
            string root = _hostingEnvironment.WebRootPath;
            var path = Path.Combine(root,formId, recordId, propertyName, title);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, MimeTypes.GetMimeType(path), title);
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
        [Route("record/{formId}/{id}")]
        public async Task<ActionResult<bool>> DeleteRecord(string formId, string id)
        {
            var isDeleted = await _repo.DeleteRecord(id);
            //check if record has any files
            string path = Path.Combine(_hostingEnvironment.WebRootPath, formId, id);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            return isDeleted;
        }

        [HttpPost]
        [Route("autocomplete")]
        public async Task<IActionResult> AddWordsToAutoDictionaries([FromBody] AutoCompleteWords words)
        {
            await _repo.AddWordsToAutos(words);
            return Ok();
        }

        [HttpGet]
        [Route("recordby/{formId}")]
        public async Task<ActionResult<Record>> GetLatestRecord(string formId)
        {
            return await _repo.GetLatestRecord(formId);
        }

    }
}