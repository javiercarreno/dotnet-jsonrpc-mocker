using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Extensions.PlatformAbstractions;

namespace jsonRpcMocker.Mocker
{
    public class MockerServer
    {
        private List<MockEndpoint> endpoints;
        public MockerServer()
        {
            this.endpoints = new List<MockEndpoint>();
            LoadMockEndpoints();
            if (this.endpoints.Count>0)
            {
                StartServer();
            }
            else {
                Console.WriteLine("No endpoints defined. Server shutdown");
            }
        }

        private void StartServer()
        {
            CreateWebServer();
        }

        private void LoadMockEndpoints()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath + "/endpoints";
            foreach (string file in System.IO.Directory.GetFiles(path,"*.endpoint")) {
                try{
                    var endpoint = MockEndpoint.FromFile(file);
                    this.endpoints.Add(endpoint);
                }catch(Exception ex){
                    Console.WriteLine(String.Format("Error ocurred loading endpoint: {0}",ex.Message));
                }   
            }
        }

		void CreateWebServer()
		{
			HttpListener listener = new HttpListener();
			listener.Prefixes.Add("http://*:8081/");
			listener.Start();
            Console.WriteLine("Starting listening at port 8080");
			new Thread(
				() =>
				{
					while (true)
					{
						HttpListenerContext ctx = listener.GetContext();
						ThreadPool.QueueUserWorkItem((_) => ProcessRequest(ctx));
					}
				}
			).Start();
		}

		void ProcessRequest(HttpListenerContext ctx)
		{
            var response = new jsonRpcResponse { id = "1", jsonrpc = "2.0", result = "Not mocked endpoint/method" };

			try{
				// Get the data from the HTTP stream
				var body = new StreamReader(ctx.Request.InputStream).ReadToEnd();
                jsonRpcCall call = Newtonsoft.Json.JsonConvert.DeserializeObject<jsonRpcCall>(body);
                if(this.endpoints.Any(x=>x.Method==call.method)) {
                    response = this.endpoints.First().Response;
                }
            }catch(Exception ex){
                response  = new jsonRpcResponse { id = "1", jsonrpc = "2.0", result = "Not valid JsonRPC call" }; 
            }

			var responseText = Newtonsoft.Json.JsonConvert.SerializeObject(response);

			byte[] buf = Encoding.UTF8.GetBytes(responseText);

			Console.WriteLine(ctx.Request.Url);

			ctx.Response.ContentEncoding = Encoding.UTF8;
			ctx.Response.ContentType = "text/json";
			ctx.Response.ContentLength64 = buf.Length;


			ctx.Response.OutputStream.Write(buf, 0, buf.Length);
			ctx.Response.Close();
		}
    }
}
