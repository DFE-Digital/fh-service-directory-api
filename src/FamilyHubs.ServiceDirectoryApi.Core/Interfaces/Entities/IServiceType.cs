namespace fh_service_directory_api.core.Interfaces.Entities;

public interface IServiceType : IEntityBase<string>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
