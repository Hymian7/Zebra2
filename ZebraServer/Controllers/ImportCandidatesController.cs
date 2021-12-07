using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Zebra.Library;
using Zebra.Library.PdfHandling;
using Zebra.Library.Services;

namespace ZebraServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportCandidatesController : ControllerBase
    {
        private IImportCandidateImporter Importer;

        private FileNameService FileNameService;

        private ZebraContext Context;

        public ImportCandidatesController(IImportCandidateImporter importer, FileNameService fileNameService, ZebraContext context)
        {
            Importer = importer;
            FileNameService = fileNameService;
            Context = context;
        }

        // POST api/<ImportCandidatesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ImportCandidate>> PostPdfFileAndGetImportCandidate(IFormFile file)
        {
            long length = file.Length;
            if (length < 0)
                return BadRequest();

            var guid = Guid.NewGuid();
            var filepath = FileNameService.GetFilePath(FolderType.Temp, guid);

            using var fileStream = file.OpenReadStream();
            byte[] bytes = new byte[length];
            fileStream.Read(bytes, 0, (int)file.Length);

            await System.IO.File.WriteAllBytesAsync(filepath, bytes);

            PreviewablePdfDocument doc = new PreviewablePdfDocument(filepath);

            var pages = new List<ImportPage>();

            for (int i = 0; i < doc.PageCount; i++)
            {
                ImportPage page = new ImportPage(i + 1);
                pages.Add(page);
            }

            var ic = new ImportCandidate(guid, pages) { FileName = (new FileInfo(filepath)).Name };

            return ic;
        }

        // PUT api/<ImportCandidatesController>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SheetDTO>> ImportImportCandidate(ImportCandidate ic)
        {
            try
            {
                await Importer.ImportImportCandidateAsync(ic);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
