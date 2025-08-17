
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IRepository, Repository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
// Add gRPC services
app.MapGrpcService<UserService>();
app.MapGrpcService<ToDoService>();

app.Run();
