using Grpc.Net.Client;
using GrpcServiceExample;
using System;
using System.Threading.Tasks;

namespace CityPuzzle
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var sqlKeyChannel = GrpcChannel.ForAddress("https://localhost:5001");
            var sqlKeyClient = new SqlKey.SqlKeyClient(sqlKeyChannel);
            var getKey = sqlKeyClient.GetSqlKeyAsync(
                              new GetToken { Token = "Token_1" });
            Console.WriteLine(getKey.GetAwaiter().GetResult().Message);
            Console.WriteLine(getKey.GetAwaiter().GetResult().Message1);



            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}