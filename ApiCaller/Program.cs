using System.Text.Json;
using System.Text;
using BcsBackendTest.DTO;

const string APP_BASE_URL = "http://localhost:5233";
using var httpClient = new HttpClient();

const string userName1 = "John";
const string userName2 = "Ann";

var serializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// POST /add
var json = JsonSerializer.Serialize(new AddRequestBody(
    Name: userName1,
    Message: "Hello!"
));
var content = new StringContent(json, Encoding.UTF8, "application/json");
Console.WriteLine($"Sending request to POST /add with {json}");
var response = await httpClient.PostAsync($"{APP_BASE_URL}/add", content);
var responseAsString = await response.Content.ReadAsStringAsync();
var userId1 = JsonSerializer.Deserialize<AddSuccessfulResponse>(responseAsString, serializerOptions)!.UserId;
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

json = JsonSerializer.Serialize(new AddRequestBody(
    Name: userName2,
    Message: "Ola!"
));
content = new StringContent(json, Encoding.UTF8, "application/json");
Console.WriteLine($"Sending request to POST /add with {json}");
response = await httpClient.PostAsync($"{APP_BASE_URL}/add", content);
responseAsString = await response.Content.ReadAsStringAsync();
var userId2 = JsonSerializer.Deserialize<AddSuccessfulResponse>(responseAsString, serializerOptions)!.UserId;
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

json = JsonSerializer.Serialize(new AddRequestBody(
    Name: userName1,
    Message: "How R U?"
));
content = new StringContent(json, Encoding.UTF8, "application/json");
Console.WriteLine($"Sending request to POST /add with {json}");
response = await httpClient.PostAsync($"{APP_BASE_URL}/add", content);
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

var nonExistedUserId = userId2 + 1;

// GET /list
Console.WriteLine($"Sending request to GET /list/{nonExistedUserId}, fail expected");
response = await httpClient.GetAsync($"{APP_BASE_URL}/list/{nonExistedUserId}");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

Console.WriteLine($"Sending request to GET /list/{userId2}");
response = await httpClient.GetAsync($"{APP_BASE_URL}/list/{userId2}");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

Console.WriteLine($"Sending request to GET /list/{userId1}");
response = await httpClient.GetAsync($"{APP_BASE_URL}/list/{userId1}");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

Console.WriteLine($"Sending request to GET /list");
response = await httpClient.GetAsync($"{APP_BASE_URL}/list");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();


// DELETE /delete
Console.WriteLine($"Sending request to DELETE /delete/{nonExistedUserId}, fail expected");
response = await httpClient.DeleteAsync($"{APP_BASE_URL}/delete/{nonExistedUserId}");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

Console.WriteLine($"Sending request to DELETE /delete/{userId2}");
response = await httpClient.DeleteAsync($"{APP_BASE_URL}/delete/{userId2}");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();


// GET /list
Console.WriteLine($"Sending request to GET /list/{userId2}, fail expected");
response = await httpClient.GetAsync($"{APP_BASE_URL}/list/{userId2}");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

Console.WriteLine($"Sending request to GET /list");
response = await httpClient.GetAsync($"{APP_BASE_URL}/list");
responseAsString = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Received response with {responseAsString}");
Console.WriteLine();

// Clean created users
await httpClient.DeleteAsync($"{APP_BASE_URL}/delete/{userId1}");
Console.WriteLine($"All created users cleaned ");
