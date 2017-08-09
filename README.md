# dotnet-jsonrpc-mocker
A dot net JSON Rpc console mocker

Usage:
- Install dotnetcore 2.0 (https://www.microsoft.com/net/core/preview)
- Clone git repository
- Add the desired jsonrpc methods in /endpoint folder (see example). Files must end with ".endpoint" extension
- run 'dotnet run'
- Now you can do JsonRPC calls to http://localhost:8081

Examples:

Simple JsonRPC response:
file 'method.example.search.endpoint'
{
    "jsonrpc":"2.0",
    "id":"1",
    "result":{
        "delivery_id":"1",
        "send_at":"01-01-2017 14:00:00",
        "method":"some_method",
        "endpoint":"some_endpoint",
        "status":"Pending",
    	"attempts_count":0
    }
}

