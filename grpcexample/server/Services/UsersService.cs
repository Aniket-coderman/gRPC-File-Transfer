using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using server;
using System.IO;
using Microsoft.Extensions.Logging;
using server.Services;
namespace server.Services{
    public class UsersService : Users.UsersBase
    {
        private readonly ILogger<UsersService> _logger;
        public UsersService(ILogger<UsersService> logger)
        {
            _logger = logger;
        }

        byte[] ConvertFileToBytes(string path){
            //int byteSize = 1024*20;
            //FileStream file = File.Open(path,FileMode.Open,FileAccess.Read);
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            //file.Read(bytes,0,System.Convert.ToInt32(file.Length));
            //file.Close();
            return bytes;
        }
        List<UserResponse> GetFileFromDb(){
            List<UserResponse> users = new List<UserResponse>();
            string path = @"C:\Users\t-anigupta\Downloads\WhatsApp Chat with The Pentagon.txt";
            byte[] buffer = ConvertFileToBytes(path);
            users.Add(new UserResponse(){
                File = Google.Protobuf.ByteString.CopyFrom(buffer)
            }); 
            return users;
        }
        public override async Task GetFiles(UserRequest request, IServerStreamWriter<UserResponse> responseStream,ServerCallContext context)
        {
            var files = GetFileFromDb();
            foreach(var file in files){
                await responseStream.WriteAsync(file);
            }
        }
    }

}
