using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities.Aggregates
{
    public interface IContact
    {
        IEnumerable<IPhone> ContactPhones { get; }
        string Name { get; init; }
        string Title { get; init; }
    }
}