using System;
namespace jsonRpcMocker.Mocker
{
    public class jsonRpcResponse
    {
        public string jsonrpc { get; set; }
        public string id { get; set; }
        public object result { get; set; }
        public jsonRpcResponse()
        {
            
        }
    }
}
