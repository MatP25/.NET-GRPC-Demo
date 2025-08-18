using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using ProtoUserService;
using ProtoToDoService;
using Grpc.Core;

// Note that the it uses HTTP instead of HTTPS to avoid certificate issues in this demo.
using var channel = GrpcChannel.ForAddress("http://localhost:5279");

// New client instance
UserService.UserServiceClient userClient = new UserService.UserServiceClient(channel);
ToDoService.ToDoServiceClient todoClient = new ToDoService.ToDoServiceClient(channel);

// Listing users
Console.WriteLine("Listing users...");
ListUsersResponse response = await userClient.ListUsersAsync(
    new ListUsersRequest{ Offset = 0, Limit = 5 }
);
foreach (var user in response.Users)
{
    Console.WriteLine($"Id: {user.Id}, Name: {user.Name}, Email: {user.Email}");
}
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
Console.Clear();

// Getting a user
Console.WriteLine("Getting user with id = 1...");
GetUserResponse singleUserResponse = await userClient.GetUserAsync(new GetUserRequest { Id = 1 });
Console.WriteLine($"Id: {singleUserResponse.User.Id}, ");
Console.WriteLine($"Name: {singleUserResponse.User.Name}, "); 
Console.WriteLine($"Email: {singleUserResponse.User.Email}");
Console.WriteLine("ToDos for this user:");
foreach (var todo in singleUserResponse.User.ToDos)
{
    Console.WriteLine($"ToDo: {todo.Id}, Title: {todo.Task}, Completed: {todo.IsCompleted}");
}
Console.WriteLine("Press any key to continue..."); 
Console.ReadKey();
Console.Clear();

// Creating a new user
Console.WriteLine("Creating a new user...");
CreateUserRequest createUserRequest = new CreateUserRequest
{
    Name = "New User",
    Email = "newuser@mail.com"
};
CreateUserResponse createUserResponse = await userClient.CreateUserAsync(createUserRequest);
Console.WriteLine($"Created User Id: {createUserResponse.User.Id}, ");
Console.WriteLine($"Name: {createUserResponse.User.Name}, ");
Console.WriteLine($"Email: {createUserResponse.User.Email}, "); 
Console.WriteLine($"Created At: {createUserResponse.CreatedAt.ToDateTime()}");
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
Console.Clear();

// Adding a ToDo to the user
Console.WriteLine("Adding a ToDo to the user with id = 1...");
CreateToDoResponse createToDoResponse = await todoClient.CreateToDoAsync(new CreateToDoRequest
{
    UserId = 1,
    Task = "New ToDo Task"
});
Console.WriteLine($"Created ToDo Id: {createToDoResponse.ToDo.Id}, ");
Console.WriteLine($"Task: {createToDoResponse.ToDo.Task}, ");
Console.WriteLine($"Created At: {createToDoResponse.CreatedAt.ToDateTime()}");
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
Console.Clear();

// Getting the user again to see the new ToDo
Console.WriteLine("Getting user with id = 1...");
GetUserResponse singleUserResponse2 = await userClient.GetUserAsync(new GetUserRequest { Id = 1 });
Console.WriteLine($"Id: {singleUserResponse2.User.Id}, ");
Console.WriteLine($"Name: {singleUserResponse2.User.Name}, "); 
Console.WriteLine($"Email: {singleUserResponse2.User.Email}");
Console.WriteLine("ToDos for this user:");
foreach (var todo in singleUserResponse2.User.ToDos)
{
    Console.WriteLine($"ToDo: {todo.Id}, Title: {todo.Task}, Completed: {todo.IsCompleted}");
}
Console.WriteLine("Press any key to continue..."); 
Console.ReadKey();
Console.Clear();

// Adding another ToDo to the user 2
Console.WriteLine("Adding a ToDo to the user with id = 2...");
AsyncUnaryCall<CreateToDoResponse> createToDoCall2 = todoClient.CreateToDoAsync(new CreateToDoRequest
{
    UserId = 2,
    Task = "New ToDo Task"
});
CreateToDoResponse createToDoResponse2 = await createToDoCall2;
Console.WriteLine($"Created ToDo Id: {createToDoResponse2.ToDo.Id}, ");
Console.WriteLine($"Task: {createToDoResponse2.ToDo.Task}, ");
Console.WriteLine($"Created At: {createToDoResponse2.CreatedAt.ToDateTime()}");

// Getting response trailers
Metadata trailers = createToDoCall2.GetTrailers();
Console.WriteLine("Response trailers: ");
foreach (var trailer in trailers)
{
    Console.WriteLine($"{trailer.Key}: {trailer.Value}");
}
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
Console.Clear();

//Updating a ToDo for user 2
Console.WriteLine("Updating a ToDo for the user with id = 2...");
AsyncUnaryCall<Empty> updateToDoCall = todoClient.UpdateToDoAsync(new UpdateToDoRequest
{
    UserId = 2,
    ToDoId = createToDoResponse2.ToDo.Id,
    Status = true // Marking the ToDo as completed
});
// Getting the headers 
var updateToDoHeaders = await updateToDoCall.ResponseHeadersAsync;
Console.WriteLine($"Updated ToDo Id: {createToDoResponse2.ToDo.Id}");
Console.WriteLine($"Response headers: ");
foreach (var header in updateToDoHeaders)
{
    Console.WriteLine($"{header.Key}: {header.Value}");
}
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
Console.Clear();

// Getting the user 2 again to see the new ToDo
Console.WriteLine("Getting user with id = 2...");
GetUserResponse singleUserResponse3 = await userClient.GetUserAsync(new GetUserRequest { Id = 2 });
Console.WriteLine($"Id: {singleUserResponse3.User.Id}, ");
Console.WriteLine($"Name: {singleUserResponse3.User.Name}, "); 
Console.WriteLine($"Email: {singleUserResponse3.User.Email}");
Console.WriteLine("ToDos for this user:");
foreach (var todo in singleUserResponse3.User.ToDos)
{
    Console.WriteLine($"ToDo: {todo.Id}, Title: {todo.Task}, Completed: {todo.IsCompleted}");
}
Console.WriteLine("Press any key to continue..."); 
Console.ReadKey();
Console.Clear();