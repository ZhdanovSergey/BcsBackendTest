namespace BcsBackendTest.Models;

sealed class Message
{
    public int Id { get; init; }
    public int UserId { get; set; }
    public User? User { get; init; }
    public required string Text { get; set; }
}