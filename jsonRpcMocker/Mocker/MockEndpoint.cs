using System;
using System.IO;
namespace jsonRpcMocker.Mocker
{
    public class MockEndpoint
    {
        public string Method { get; private set; }
        public jsonRpcResponse Response {get; private set;}

        private MockEndpoint(string method, string response)
        {
            this.Method = method;
            this.Response = Newtonsoft.Json.JsonConvert.DeserializeObject<jsonRpcResponse>(response);            
        }

        public static MockEndpoint FromFile(string path)
        {
            if (File.Exists(path))
            {
                var text = System.IO.File.ReadAllText(path);
                var fileName = Path.GetFileName(path);
                return new MockEndpoint(fileName.Replace(".endpoint",""),text);
            }
            else throw new FileNotFoundException(path);
        }
    }
}
