// See https://aka.ms/new-console-template for more information
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Grpc.Net.Client;
using server;

namespace client{
    class Program{
        static async Task Main(string[] args){
            Console.WriteLine("Calling a GRPC Service!");

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = 
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress("https://localhost:7193",
            new GrpcChannelOptions { HttpClient = httpClient });
            var client =  new Users.UsersClient(channel);
            try
            {
                UserRequest request = new UserRequest();
                using (var call = client.GetFiles(request))
                {
                    while (await call.ResponseStream.MoveNext(CancellationToken.None))
                    {
                        var currentFileChunk = call.ResponseStream.Current;
                        Console.WriteLine("Uploading File");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}

