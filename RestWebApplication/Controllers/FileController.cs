using Microsoft.AspNetCore.Mvc;
using RestWebApplication.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestWebApplication.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace RestWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FileController : ControllerBase
    {
        private readonly DataAccess _data;

        public FileController(DataAccess data)
        {
            this._data = data;
        }

        [HttpGet]
        public async Task<IActionResult> GetFileList()
        {
            var files = await _data.FileStorage.AsNoTracking().ToListAsync();
            return Ok(files);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            using var fileStream = file.OpenReadStream();
            using var binaryFileStream = new BinaryReader(fileStream);
            byte[] fileData = binaryFileStream.ReadBytes((int)file.Length);
            var dbFile = new FileStorage
            {
                Name = file.FileName, 
                Content = fileData, 
                Type = file.ContentType
            };

            await _data.FileStorage.AddAsync(dbFile);
            await _data.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] IFormFileCollection files)
        {
            if (files == null)
            {
                return BadRequest();
            }

            foreach (var file in files)
            {
                using var fileStream = file.OpenReadStream();
                using var binaryStream = new BinaryReader(fileStream);
                byte[] fileData = binaryStream.ReadBytes((int)file.Length);

                var dbFile = new FileStorage
                {
                    Name = file.FileName,
                    Content = fileData,
                    Type = file.ContentType
                };

                await _data.FileStorage.AddAsync(dbFile);
            }

            await _data.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _data.FileStorage.SingleOrDefaultAsync(x => x.Id == id);

            if (file == null)
            {
                return NotFound();
            }

            return File(file.Content, file.Type, file.Name);
        }
    }
}
