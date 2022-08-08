using Aspose.Cells;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IRS.DTO;
using IRS.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class FilesController : ApiControllerBase
    {

        public FilesController()
        {
        }
       
        byte[] ConvertExcelToArray(IFormFile file)
        {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        return fileBytes;
                    }
                }
            return new byte[] { };
        }
      
        [HttpPost]
        public async Task<IActionResult> ExcelExportToDOS([FromForm]ExcelExportFileDto model)
        {
            using var ms = new MemoryStream();
            await model.File.CopyToAsync(ms);
            using Workbook wb = new Workbook(ms);
            using var output = new MemoryStream();
            wb.Save(output, SaveFormat.ODS);
            var fileBytes = output.ToArray();
            return File(fileBytes, "application/octet-stream", $"{model.FunctionName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.ods");
        }

    }
}
