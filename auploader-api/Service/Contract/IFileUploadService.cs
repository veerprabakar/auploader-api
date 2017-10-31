using System.Threading.Tasks;
using auploader.api.Model;
using Amazon.S3;

namespace auploader.api.Service
{
    public interface IFileUploadService
    {
        IAmazonS3 S3Clinet { get; set; }

        Task<FileUploaded> UploadFile(
            string bucketName, 
            string bucketUrl, 
            string objectKey, 
            S3StorageClass storageClass, 
            S3CannedACL permissions, UploadFileInfo file);
    }
}