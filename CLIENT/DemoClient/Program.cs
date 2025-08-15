using Grpc.Net.Client;
using DemoClient;

using var channel = GrpcChannel.ForAddress("https://localhost:5279");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Pepe" });
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

