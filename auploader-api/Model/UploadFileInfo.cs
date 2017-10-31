using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace auploader.api.Model
{
    public class UploadFileInfo
    {
        public string MimeType { get; set; }

        public Stream FileConent { get; set; }
    }
}
