using System;

namespace auploader.api.Model
{
    public class FileUploaded
    {
        public string ObjectKey { get; set; }
        public string ObjectLocation { get; set; }
        public string ETag { get; set; }
        public Exception Exception { get; set; }
    }
}