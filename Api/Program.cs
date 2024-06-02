using Api.DTO;
using BcsBackendTest.DTO;
using BcsBackendTest.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5233");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=BcsBackendTestDB; Trusted_Connection=True"));

builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/add", async (AddRequestBody body, AppDbContext dbContext, ILogger<Program> logger) =>
{
    try
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(user => user.Name == body.Name);

        if (user is null)
        {
            user = new User() { Name = body.Name };
            dbContext.Users.Add(user);
        }

        var message = new Message() { Text = body.Message };
        user.Messages.Add(message);

        await dbContext.SaveChangesAsync();

        logger.LogInformation($"{DateTime.UtcNow} - Successfully add message from {body.Name}");
        return Results.Json(new AddSuccessfulResponse(user.Id));
    }
    catch
    {
        logger.LogError($"{DateTime.UtcNow} - Failed to add message from {body.Name}");
        return Results.Json(new FailedResponse());
    }
});

app.MapGet("/list/{user_id}", async (int user_id, AppDbContext dbContext, ILogger<Program> logger) =>
{
    try
    {
        var user = await dbContext.Users
            .Include(user => user.Messages)
            .FirstAsync(user => user.Id == user_id);

        var response = new ListUserSuccessfulResponse(
            Name: user.Name,
            Messages: user.Messages.Select(message => message.Text).ToList()
        );

        logger.LogInformation($"{DateTime.UtcNow} - Successfully get messages from {nameof(user_id)} {user_id}");
        return Results.Json(response);
    }
    catch
    {
        logger.LogError($"{DateTime.UtcNow} - Failed to get messages from {nameof(user_id)} {user_id}");
        return Results.Json(new FailedResponse());
    }
});

app.MapGet("/list", async (AppDbContext dbContext, ILogger<Program> logger) =>
{
    try
    {
        var messages = await dbContext.Messages
            .Include(message => message.User)
            .ToArrayAsync();

        var responseItems = messages.Aggregate(new List<ListSuccessfulResponseItem>(), (responseItems, message) =>
        {
            if (responseItems.Count > 0 && responseItems[^1].Name == message.User.Name)
                responseItems[^1].Messages.Add(message.Text);
            else
                responseItems.Add(new ListSuccessfulResponseItem(
                    Name: message.User.Name,
                    Messages: new List<string>() { message.Text })
                );

            return responseItems;
        });

        logger.LogInformation($"{DateTime.UtcNow} - Successfully get all messages");
        return Results.Json(new ListSuccessfulResponse(responseItems));
    }
    catch
    {
        logger.LogError($"{DateTime.UtcNow} - Failed to get all messages");
        return Results.Json(new FailedResponse());
    }
});

app.MapDelete("/delete/{user_id}", async (int user_id, AppDbContext dbContext, ILogger<Program> logger) =>
{
    try
    {
        var user = await dbContext.Users
            .FirstAsync(user => user.Id == user_id);

        dbContext.Users.Remove(user);

        await dbContext.SaveChangesAsync();

        logger.LogInformation($"{DateTime.UtcNow} - Successfully delete {nameof(user_id)} {user_id}");
        return Results.Json(new DeleteSuccessfulResponse());
    }
    catch
    {
        logger.LogError($"{DateTime.UtcNow} - Failed to delete {nameof(user_id)} {user_id}");
        return Results.Json(new FailedResponse());
    }
});

app.Run();
