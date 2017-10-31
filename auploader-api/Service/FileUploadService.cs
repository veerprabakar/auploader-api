using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using auploader.api.Model;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace auploader.api.Service
{
    public class FileUploadService : IFileUploadService
    {
        public IAmazonS3 S3Clinet { get; set; }

        private IAmazonS3 GetS3Config(S3Settings s3settings)
        {
            var region = RegionEndpoint.GetBySystemName(s3settings.AWSRegion);
            return( new AmazonS3Client(
                new BasicAWSCredentials(s3settings.AWSAccessKey,
                    s3settings.AWSSecretKey), region));
        }

        public FileUploadService(IOptions<S3Settings> s3settings)
        {
            S3Clinet = GetS3Config(s3settings.Value);
        }

        public FileUploadService()
        {
            // hard-coded config for POC
            S3Settings tempSetting = new S3Settings
            {
                OriginalBucketName = "praba.bucket",
                OriginalBucketUrl = "https://s3-ap-southeast-2.amazonaws.com/praba.bucket/",
                AWSRegion = "ap-southeast-2",
                AWSAccessKey = "AKIAJAXHRPK5IGGDAMJA",
                AWSSecretKey = "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"
            };
            S3Clinet = GetS3Config(tempSetting);
        }

        public async Task<FileUploaded> UploadFile(
            string bucketName, 
            string bucketUrl, 
            string objectKey, 
            S3StorageClass storageClass, 
            S3CannedACL permissions, UploadFileInfo file)
        {
            FileUploaded model = new FileUploaded();

            try
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    StorageClass = storageClass,
                    CannedACL = permissions,
                    ContentType = file.MimeType
                };

                request.InputStream = file.FileConent;

                //MD-HASH
              /*  byte[] md5Hash = file.FileConent.Md5Hash();
                request.MD5Digest = md5Hash.ToBase64String();*/

                PutObjectResponse response = await S3Clinet.PutObjectAsync(request);

               /* string eTag = response.ETag.Trim('"').ToLowerInvariant();
                string expectedETag = md5Hash.ToS3ETagString();*/

     /*           if (eTag != expectedETag)
                {
                    throw new Exception("The eTag received from S3 doesn't match the eTag computed before uploading. This usually indicates that the file has been corrupted in transit.");
                }*/

                model.ObjectKey = objectKey;
                //model.ETag = eTag;
                model.ObjectLocation = bucketUrl + objectKey;
            }
            catch (Exception ex)
            {
                model.Exception = ex;
            }
            return model;
        }
    }
    
}
