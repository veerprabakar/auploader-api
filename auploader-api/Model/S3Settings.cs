using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auploader.api.Model
{
    public class S3Settings
    {
        public string OriginalBucketName { get; set; }
        public string OriginalBucketUrl { get; set; }
        public string AWSRegion { get; set; }
        public string AWSAccessKey { get; set; }
        public string AWSSecretKey { get; set; }
    }
}
