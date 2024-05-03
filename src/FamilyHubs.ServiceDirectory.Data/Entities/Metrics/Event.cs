namespace FamilyHubs.ServiceDirectory.Data;

public class Event
{
    public short Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
