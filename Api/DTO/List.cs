namespace BcsBackendTest.DTO;

public sealed record ListUserSuccessfulResponse(
    string Name,
    List<string> Messages,
    byte Status = 1
);

public sealed record ListSuccessfulResponse(
    List<ListSuccessfulResponseItem> Items,
    byte Status = 1
);

public sealed record ListSuccessfulResponseItem(
    string Name,
    List<string> Messages
);