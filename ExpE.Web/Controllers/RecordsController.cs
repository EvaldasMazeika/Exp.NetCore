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
    [Route("api/forms/{formId}/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Record>>> GetAllRecords(string formId)
        {
            return await _repo.GetRecords(formId);
        }

        [HttpPost]
        public async Task<ActionResult<Record>> InsertRecord(string formId, [FromBody] Record record)
        {
            var result = await _repo.AddRecord(record);

            if (result != null)
            {
                return CreatedAtAction(nameof(GetRecord), new { formId, id = result.Id }, result);
            }

            throw new Exception("Cannot insert record");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Record>> GetRecord(string formId, string id)
        {
            return await _repo.GetRecord(id);
        }

        [HttpPut]
        public async Task<ActionResult<Record>> UpdateRecord(string formId, [FromBody] Record record)
        {
            var result = await _repo.UpdateRecord(record);

            if (result != null)
            {
                return CreatedAtAction(nameof(GetRecord), new { formId, id = result.Id }, result);
            }

            throw new Exception("Cannot update record");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteRecord(string formId, string id)
        {
            await _repo.DeleteRecord(id);
            //check if record has any files
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", formId, id);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("latest")]
        public async Task<ActionResult<Record>> GetLatestRecord(string formId)
        {
            return await _repo.GetLatestRecord(formId);
        }

        [HttpPost]
        [Route("{recordId}/{propertyName}")]
        public async Task<ActionResult> UploadFile(string formId, string recordId, string propertyName)
        {
            try
            {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", formId, recordId, propertyName);
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

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{recordId}/{propertyName}/{title}")]
        public async Task<IActionResult> DownloadFile(string formId, string recordId, string propertyName, string title)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", formId, recordId, propertyName, title);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, MimeTypes.GetMimeType(path), title);
        }
    }
}