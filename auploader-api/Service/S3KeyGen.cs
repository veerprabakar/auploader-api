using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace auploader.api.Service
{
    public class S3KeyGen
    {
        private static readonly Random Random = new Random();

        public static string GenerateObjectKey(string fileName)
        {
            return $"{GetRandomPrefix(8)}/{DateTime.UtcNow:s}/{UrlEncoder.Default.Encode(fileName)}"
                .Replace(':', '-');
        }

        private static string GetRandomPrefix(int length)
        {
            string[] result = new string[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = Random.Next(16).ToString("X");
            }
            return string.Concat(result);
        }
    }
}
