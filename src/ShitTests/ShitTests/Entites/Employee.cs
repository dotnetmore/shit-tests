namespace ShitTests.Entites;

public record Employee
{
    public required string Name { get; init; }
    public required string Position { get; init; }
    public required decimal Allowance { get; init; }
}