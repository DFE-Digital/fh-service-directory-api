using fh_service_directory_api.core.Interfaces.Entities;
using LocalAuthorityInformationServices.SharedKernel;

namespace fh_service_directory_api.core.Concretions.Entities;

public class Funding : EntityBase<string>, IFunding
{
    private Funding() { }

    public Funding(string source)
    {
        Source = source;
    }

    public string Source { get; init; } = default!;
}
