namespace ShitTests.Entites;

public record User
{
    public required string Name { get; init; }
    public required int Age { get; init; }
}