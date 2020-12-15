using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Controllers
{
    public class FileUploadController : Controller
    {
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
                long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileExName = Path.GetExtension(formFile.FileName);
                    var fileName = GetFileName(DateTime.Now, fileExName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"public/earthquake_data", fileName);

                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { countFile = files.Count, status= "success" });
        }

        public string GetFileName(DateTime date, string exName)
        {
            var day = date.Day > 9 ? date.Day.ToString() : $"0{date.Day}";
            var month = date.Month > 9 ? date.Month.ToString() : $"0{date.Month}";

            return $"{day}{month}{date.Year}{exName}";
        }

    }
}