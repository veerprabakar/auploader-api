using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using auploader.api.Model;
using Newtonsoft.Json.Linq;

namespace auploader.api.Service
{
    public class FileProcesser
    {
        public static UploadFileInfo GetUploadFileInfo(Stream input, string fileType)
        {
            input.Position = 0;

            return new UploadFileInfo
            {
                MimeType = fileType,
                 FileConent = input
            };
        }
    }
}
