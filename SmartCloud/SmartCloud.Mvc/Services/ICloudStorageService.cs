using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCloud.Mvc.Services
{
    public interface ICloudStorageService
    {
        Task<IEnumerable<string>> ListAll(string container);
        Task<Stream> GetBlob(string container,string blob);
    }
}
