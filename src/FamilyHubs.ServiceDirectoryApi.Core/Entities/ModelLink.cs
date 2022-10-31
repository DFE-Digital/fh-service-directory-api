using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class ModelLink : EntityBase<string>, IModelLink, IAggregateRoot
{
    private ModelLink() { }
    public ModelLink(string id, string linkType, string modelOneId, string modelTwoId)
    {
        Id = id;
        LinkType = linkType;
        ModelOneId = modelOneId;
        ModelTwoId = modelTwoId;
    }

    public string LinkType { get; set; } = default!;
    public string ModelOneId { get; set; } = default!;
    public string ModelTwoId { get; set; } = default!;
}
