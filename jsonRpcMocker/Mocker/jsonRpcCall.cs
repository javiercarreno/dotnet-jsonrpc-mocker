using System;
namespace jsonRpcMocker.Mocker
{
    public class jsonRpcCall
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public object @params {get;set;}
        public jsonRpcCall()
        {
            
        }
    }
}
