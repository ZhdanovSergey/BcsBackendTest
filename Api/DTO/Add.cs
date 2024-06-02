namespace BcsBackendTest.DTO;

public sealed record AddRequestBody(
    string Name,
    string Message
);

public sealed record AddSuccessfulResponse(
    int UserId,
    byte Status = 1
);
