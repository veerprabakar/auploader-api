using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using auploader.api.Model;
using auploader.api.Service;
using Amazon.S3;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using StackExchange.Redis;

namespace auploader.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    public class UploadController : BaseController
    {
        private const long MAX_SIZE = 1000000000;

        private S3Settings S3Settings { get; }

        private Settings Settings { get; }

        private FileUploadService FileUploadService { get; set; }

        public UploadController(IOptions<Settings> settings,  
                                IOptions<S3Settings> s3Settings, 
                                IOptions<FileUploadService> fileUploadService)
        {
            Settings = settings.Value;
            S3Settings = s3Settings.Value;
            FileUploadService = fileUploadService.Value;
        }

        //API to upload the file into S3
        [Route("/api/UploadFileToS3"), HttpPost, RequestSizeLimit(100_000_000_000)]
        public async Task<IActionResult> UploadFileToS3(IFormFile file)
        {
            var fileName = Path.GetFileName(
                                ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName.Value);

            string objectKey = S3KeyGen.GenerateObjectKey(fileName);

            using (var stream = file.OpenReadStream())
            {
                UploadFileInfo info =  FileProcesser.GetUploadFileInfo(stream, file.ContentType);

                FileUploaded model = await FileUploadService.UploadFile(
                    S3Settings.OriginalBucketName, 
                    S3Settings.OriginalBucketUrl,
                    objectKey,
                    S3StorageClass.Standard, //Need to check with Ginu
                    S3CannedACL.Private, info);
                if (model.Exception != null)
                {
                    //logg the error:
                    return StatusCode((int) HttpStatusCode.InternalServerError);
                }
                return Created(model.ObjectLocation, model);
            }
        }

        //TEST CODE: copies the file into web server
        //POC : Code for file upload: enable CORS to test 
        [Route("/api/UploadFile"), HttpPost, RequestSizeLimit(MAX_SIZE)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok();
        }
    }
}
