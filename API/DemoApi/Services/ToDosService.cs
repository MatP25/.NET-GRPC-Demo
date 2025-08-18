using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

public class ToDoService : ProtoToDoService.ToDoService.ToDoServiceBase
{
    private readonly IRepository _repository;
    private readonly ILogger<ToDoService> _logger;

    public ToDoService([FromServices] IRepository repository, ILogger<ToDoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public override Task<ProtoToDoService.CreateToDoResponse> CreateToDo(ProtoToDoService.CreateToDoRequest request, ServerCallContext context)
    {
        try
        {
            ToDo toDo = _repository.AddToDo(request.UserId, request.Task);
            _logger.LogInformation($"Created ToDo with id {toDo.Id} for user {request.UserId}.");

            ProtoToDoService.CreateToDoResponse response = new ProtoToDoService.CreateToDoResponse
            {
                ToDo = new ProtoToDoService.ToDo
                {
                    Id = toDo.Id,
                    Task = toDo.Task,
                    IsCompleted = toDo.IsCompleted
                },
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            // Custom trailer
            context.ResponseTrailers.Add("Custom-Trailer-Info", "Custom value");

            return Task.FromResult(response);
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.NotFound, e.Message));
        }
    }

    public override Task<Empty> UpdateToDo(ProtoToDoService.UpdateToDoRequest request, ServerCallContext context)
    {
        try
        {
            _repository.UpdateToDo(request.UserId, request.ToDoId, request.Status);
            _logger.LogInformation($"Updated ToDo with id {request.ToDoId} for user {request.UserId}.");

            return Task.FromResult(new Empty());
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.NotFound, e.Message));
        }
    }
}