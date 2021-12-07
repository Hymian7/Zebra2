﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZebraServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // GET api/<FilesController>/5
        [HttpGet("{id}")]
        public Task<FileContentResult> Get(int id)
        {
            var bytes = System.IO.File.ReadAllBytes(@"D:\Desktop\ZebraTemp\ServerArchive\" + id.ToString().PadLeft(8, '0') + ".pdf");
            
            string mimeType = "application/pdf";
            return Task.FromResult(new FileContentResult(bytes, mimeType)
            {
                FileDownloadName = id.ToString().PadLeft(8, '0') + ".pdf"
            });
        }

        // POST api/<FilesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FilesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FilesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
