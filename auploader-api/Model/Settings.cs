using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auploader.api.Model
{
    public class Settings
    {
        public string MaxFileSize { get; set; }

        public string[] FileFormats { get; set; }

    }
}
