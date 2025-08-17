using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
public class UserService : ProtoUserService.UserService.UserServiceBase
{
    private readonly ILogger<UserService> _logger;
    private readonly IRepository _repository;

    public UserService([FromServices] IRepository repository, ILogger<UserService> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public override Task<ProtoUserService.ListUsersResponse> ListUsers(ProtoUserService.ListUsersRequest request, ServerCallContext context)
    {
        List<User> users = _repository.GetUsers(request.Offset, request.Limit);
        ProtoUserService.ListUsersResponse response = new ProtoUserService.ListUsersResponse();
        _logger.LogInformation($"Listing {request.Limit} users from {request.Offset}, found {users.Count} users.");

        response.Offset = request.Offset;
        response.Amount = users.Count;

        foreach (var user in users)
        {
            response.Users.Add(new ProtoUserService.User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ToDos = { user.ToDos.Select(t => new ProtoUserService.ToDo
                {
                    Id = t.Id,
                    Task = t.Task,
                    IsCompleted = t.IsCompleted
                })}
            });
        }

        return Task.FromResult(response);
    }

    public override Task<ProtoUserService.GetUserResponse> GetUser(ProtoUserService.GetUserRequest request, ServerCallContext context)
    {
        User? user = _repository.GetUserById(request.Id);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        _logger.LogInformation($"Getting user with id {request.Id}, found user {user.Name}.");

        ProtoUserService.GetUserResponse response = new ProtoUserService.GetUserResponse
        {
            User = new ProtoUserService.User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ToDos = { user.ToDos.Select(t => new ProtoUserService.ToDo
                {
                    Id = t.Id,
                    Task = t.Task,
                    IsCompleted = t.IsCompleted
                })}
            }
        };

        return Task.FromResult(response);
    }

    public override Task<ProtoUserService.CreateUserResponse> CreateUser(ProtoUserService.CreateUserRequest request, ServerCallContext context)
    {
        User user = new User(request.Name, request.Email);

        User createdUser = _repository.AddUser(user);
        _logger.LogInformation($"Created user with id {createdUser.Id}, name {createdUser.Name}.");

        ProtoUserService.CreateUserResponse response = new ProtoUserService.CreateUserResponse
        {
            User = new ProtoUserService.User
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email
            },
            CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
        };

        return Task.FromResult(response);
    }

    public override Task<Empty> DeleteUser(ProtoUserService.DeleteUserRequest request, ServerCallContext context)
    {
        try
        {
            int deletedId = _repository.DeleteUser(request.Id);
            _logger.LogInformation($"Deleted user with id {request.Id}, result id {deletedId}.");
            return Task.FromResult(new Empty());
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.NotFound, e.Message));
        }
    }
}