using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebPerformanceMeter;
using WebPerformanceMeter.Users;
using System.IO;

namespace RestWebApplication.WebPerformanceTest.Users
{
    public class UploadFileUser : HttpUser
    {
        private readonly byte[] _file;

        public UploadFileUser(HttpClient client, string? userName = null) 
            : base(client, userName)
        {
            this._file = File.ReadAllBytes("Users/UploadFile_testfile.txt");
        }

        protected override async Task Performance()
        {
            await UploadFile("File/UploadFile", "testfile.txt", this._file);
        }
    }
}
