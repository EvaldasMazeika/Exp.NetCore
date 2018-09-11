using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Core.Interfaces;
using ExpE.Domain;
using ExpE.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ExpE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IExcelExport _excelExport;

        public FormsController(IRepository repo,
            IHostingEnvironment hostingEnvironment,
            IExcelExport excelExport)
        {
            _repo = repo;
            _hostingEnvironment = hostingEnvironment;
            _excelExport = excelExport;
        }

        [HttpGet]
        public async Task<IEnumerable<MyForm>> GetForms()
        {
            return await _repo.GetAllForms();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteForm(string id)
        {
            //check if form has any files
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", id);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            await _repo.DeleteFormById(id);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddForm([FromBody] MyForm myForm)
        {
            var isExists = _repo.ExistsFormName(myForm.Name);

            if (isExists)
                throw new Exception("Not unique name");

            var result = await _repo.AddForm(myForm);

            if (result != null)
            {
                return CreatedAtAction(nameof(GetForm), new { id = result.Id }, result);
            }

            throw new Exception("Error saving form");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<MyForm>> GetForm(string id)
        {
            var form = await _repo.GetFormById(id);
            if (form == null)
                return NotFound();

            return form;
        }

        [HttpGet]
        [Route("{id}/export")]
        public async Task<IActionResult> ExportTable(string id)
        {
            var form = await _repo.GetFormById(id);
            var records = await _repo.GetRecords(id);

            MemoryStream excelStream = _excelExport.ExportSimpleExcel(form, records);

            return File(excelStream, MimeTypes.GetMimeType($"{form.Name}.xlsx"), $"{form.Name}.xlsx");
        }

        [HttpPost]
        [Route("{id}/export")]
        public async Task<IActionResult> UseTemplate(string id)
        {
            var form = await _repo.GetFormById(id);
            var records = await _repo.GetRecords(id);

            var files = Request.Form.Files;
            var file = files.Single();

            var templateStream = new MemoryStream();
            await file.CopyToAsync(templateStream);

            MemoryStream memory = _excelExport.ExportUsingTemplate(templateStream, form, records);

            return File(memory, MimeTypes.GetMimeType($"{form.Name}.xlsx"), $"{form.Name}.xlsx");
        }
    }
}